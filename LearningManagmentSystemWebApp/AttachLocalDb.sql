USE master
CREATE DATABASE Chinook1
    ON (FILENAME = 'C:\Users\tjoni\Desktop\DBSD\LMSWebApp\LearningManagmentSystemWebApp\LearningManagmentSystemWebApp\AppData\Chinook.mdf'),   
             (FILENAME = 'C:\Users\tjoni\Desktop\DBSD\LMSWebApp\LearningManagmentSystemWebApp\LearningManagmentSystemWebApp\AppData\Chinook.ldf')   
    FOR ATTACH
    
--shrink DB log file
USE Chinook
--select * FROM sys.database_files
ALTER DATABASE Chinook SET RECOVERY SIMPLE
GO
DBCC SHRINKFILE (Chinook_log, 7)