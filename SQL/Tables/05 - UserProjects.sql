USE [MyWork]
GO
-- create table
CREATE TABLE [dbo].[UserProjects](
	[UserId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_UserProjects] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
-- FK to projects
ALTER TABLE [dbo].[UserProjects]  WITH CHECK ADD  CONSTRAINT [FK_UserProjects_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
GO
ALTER TABLE [dbo].[UserProjects] CHECK CONSTRAINT [FK_UserProjects_Projects]
GO
-- FK to users
ALTER TABLE [dbo].[UserProjects]  WITH CHECK ADD  CONSTRAINT [FK_UserProjects_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserProjects] CHECK CONSTRAINT [FK_UserProjects_Users]
GO
-- insert data; add projects to users
INSERT INTO UserProjects (UserId, ProjectId)
VALUES (2, 1);
INSERT INTO UserProjects (UserId, ProjectId)
VALUES (2, 2);
INSERT INTO UserProjects (UserId, ProjectId)
VALUES (2, 3);
INSERT INTO UserProjects (UserId, ProjectId)
VALUES (3, 3);
INSERT INTO UserProjects (UserId, ProjectId)
VALUES (3, 4);
GO