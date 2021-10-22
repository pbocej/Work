USE MyWork
GO
-- create table
CREATE TABLE [dbo].[WorkHours]
(
	[WorkHourID] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Date] DATETIME NOT NULL, 
    [From] DATETIME NOT NULL, 
    [To] DATETIME NULL, 
    [Hours] DATETIME NULL, 
    [ProjectId] int NULL,
    [Subject] VARCHAR(250) NULL, 
    [Description] VARCHAR(2000) NULL,
	[UserId] int NOT NULL
)
GO
-- time index
CREATE NONCLUSTERED INDEX IX_WorkHours_DateFrom ON dbo.WorkHours
	(
	[Date],
	[From]
	) ON [PRIMARY]
GO
-- project FK
ALTER TABLE [dbo].[WorkHours]  WITH CHECK ADD  CONSTRAINT [FK_WorkHours_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
GO
ALTER TABLE [dbo].[WorkHours] CHECK CONSTRAINT [FK_WorkHours_Projects]
GO
-- users FK
ALTER TABLE [dbo].[WorkHours]  WITH CHECK ADD  CONSTRAINT [FK_WorkHours_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[WorkHours] CHECK CONSTRAINT [FK_WorkHours_Users]
GO
-- insert data
USE [MyWork]
GO

