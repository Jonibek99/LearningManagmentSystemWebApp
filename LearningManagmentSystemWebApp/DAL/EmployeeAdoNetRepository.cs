using LearningManagmentSystemWebApp.DAL.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace LearningManagmentSystemWebApp.DAL
{
    public class EmployeeAdoNetRepository : IEmployeeRepository
    {
        
        private const string SQL_SELECT_ALL = @"select EmployeeId, FirstName, LastName, Title, BirthDate
                                                                        from Employee";
        private const string SQL_SELECT_BY_ID = @"select EmployeeId, FirstName, LastName, Title, BirthDate
                                                                          from Employee
                                                                          where EmployeeId = @EmployeeId";
        private const string SQL_INSERT = @"insert into Employee(FirstName, LastName, Title, BirthDate)
                                                                    output inserted.EmployeeId
                                                                    values(@FirstName, @LastName, @Title, @BirthDate)";
        private const string SQL_UPDATE = @"update Employee set 
                                                                    FirstName = @FirstName, 
                                                                    LastName = @LastName,
                                                                    Title = @Title,
                                                                    @BirthDate = @BirthDate
                                                                    where EmployeeId = @EmployeeId";
        private const string SQL_DELETE = @"delete from Employee where EmployeeId = @EmployeeId";
        public readonly string _connStr;
        public EmployeeAdoNetRepository(string connStr)
        {
            _connStr = connStr;
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_DELETE;
            cmd.Parameters.AddWithValue("@EmployeeId", id);
            conn.Open();
            int numberOfActuallyUpdated = cmd.ExecuteNonQuery();
            if (numberOfActuallyUpdated == 0)
            {
                throw new Exception($"Employee doesn't exist! {id}");
            }
        }

        public async Task<IList<Employee>> GetAllAsync()
        {
           var list = new List<Employee>();
           using var conn = new SqlConnection(_connStr); 
           using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_SELECT_ALL;
            await conn.OpenAsync();
            using var rdr  = await cmd.ExecuteReaderAsync();
            while( await rdr.ReadAsync())
            {
                var emp =  await MapDbDataReaderToEmployeeAsync(rdr);
                list.Add(emp);
            }
            return list; 
        }


        public async IAsyncEnumerable<Employee> GetAllAsync2()
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_SELECT_ALL;
            await conn.OpenAsync();
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                yield return await MapDbDataReaderToEmployeeAsync(rdr);
            }
        }

        public Employee? GetById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_SELECT_BY_ID;
            cmd.Parameters.AddWithValue("@EmployeeID", id);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return MapDbDataReaderToEmployee(rdr);
            }
            return null;
        }

        public int Insert(Employee employee)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_INSERT;
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@Title", employee.Title ?? (object)DBNull.Value );
            cmd.Parameters.AddWithValue("@BirthDate", employee.BirthDate ?? (object)DBNull.Value);
            conn.Open();
            int id = (int)cmd.ExecuteScalar();
            employee.EmployeeId = id; 
            return id;
        }

        public void Update(Employee employee)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_UPDATE;
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@Title", employee.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BirthDate", employee.BirthDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
            conn.Open();
            cmd.ExecuteNonQuery();
            int numberOfActuallyUpdated = cmd.ExecuteNonQuery();
            if(numberOfActuallyUpdated == 0)
            {
                throw new Exception($"Employee doesn't exist! {employee.EmployeeId}");
            }
        }

        public Employee MapDbDataReaderToEmployee(DbDataReader rdr)
        {
            return new Employee()
            {
                EmployeeId = rdr.GetInt32("EmployeeId"),
                FirstName = rdr.GetString("FirstName"),
                LastName = rdr.GetString("LastName"),
                Title = rdr.IsDBNull("Title") ? null : rdr.GetString("Title"),
                BirthDate = rdr.IsDBNull("BirthDate") ? null : rdr.GetDateTime("BirthDate")
            };
        }
         
        private async Task<Employee> MapDbDataReaderToEmployeeAsync(DbDataReader rdr)
        {
            return new Employee()
            {
                EmployeeId = await rdr.GetFieldValueAsync<int>("EmployeeId"),
                FirstName = await rdr.GetFieldValueAsync<string>("FirstName"),
                LastName = await rdr.GetFieldValueAsync<string>("LastName"),
                Title = await rdr.IsDBNullAsync("Title") ? null : await rdr.GetFieldValueAsync<string>("Title"),
                BirthDate = await rdr.IsDBNullAsync("BirthDate") ? null : await rdr.GetFieldValueAsync<DateTime>("BirthDate")


            };
        }

    }
}
