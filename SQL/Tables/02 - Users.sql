USE MyWork
GO
-- create table
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[Password] [varchar](256) NOT NULL,
	[FirstName] [varchar](20) NULL,
	[LastName] [varchar](30) NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[UserGroupId] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-- relation to UserGroups
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserGroups] FOREIGN KEY([UserGroupId])
REFERENCES [dbo].[UserGroups] ([UserGroupId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserGroups]
GO
-- insert data (Admin)
USE [MyWork]
GO

INSERT INTO [Users] ([UserName],[Password],[FirstName],[LastName],[Email],[Phone],[UserGroupId])
     VALUES('Admin', '', 'Administrator', '', 'admin@pb.local', '+421955123456', 0)
GO
