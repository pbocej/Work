USE master
-- =============================================
-- Create database
-- =============================================

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases
		WHERE name = N'MyWork'
)
DROP DATABASE [MyWork]
GO

CREATE DATABASE [MyWork]
GO
ALTER DATABASE [MyWork] SET COMPATIBILITY_LEVEL = 110
GO
ALTER DATABASE [MyWork] COLLATE Slovak_CI_AS
GO
ALTER DATABASE [MyWork] SET RECOVERY SIMPLE 
GO
