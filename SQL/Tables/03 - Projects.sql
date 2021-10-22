USE MyWork
GO
CREATE TABLE Projects (
	[ProjectId] int NOT NULL IDENTITY PRIMARY KEY,
	[ProjectName] nvarchar(100) NOT NULL,
	[ProjectDescription] ntext)
GO