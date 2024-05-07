declare @FirstName nvarchar(200) = 'Namejon'
declare @LastName nvarchar(200) = 'LastNamejon'
declare @Title nvarchar(30) = 'Miss'
declare @BirthDate  datetime = '2020-01-01'

insert into Employee(FirstName, LastName, Title, BirthDate)
output inserted.EmployeeId
values(@FirstName, @LastName, @Title, @BirthDate)

select SCOPE_IDENTITY()
select * from Employee
------------  ----------------

declare @FirstName nvarchar(200) = 'Namejon'
declare @LastName nvarchar(200) = 'LastNamejon'
declare @Title nvarchar(30) = 'Miss'
declare @BirthDate  datetime = '2020-01-01'
update Employee set 
FirstName = @FirstName, 
LastName = @LastName,
Title = @Title,
@BirthDate = @BirthDate
where EmployeeId = 2

------------------------------------
delete from Employee where EmployeeId = 1


