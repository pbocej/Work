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
ALTER TABLE dbo.Users ADD CONSTRAINT
	IX_Users_UserName UNIQUE NONCLUSTERED 
	(
	UserName
	) ON [PRIMARY]
GO
-- insert data (Admin)
INSERT INTO [Users] ([UserName],[Password],[FirstName],[LastName],[Email],[Phone],[UserGroupId])
     VALUES('Admin', '', 'Administrator', '', 'admin@pb.local', '+421955123456', 0);
INSERT INTO [Users] ([UserName],[Password],[FirstName],[LastName],[Email],[Phone],[UserGroupId])
     VALUES('Peter', '', 'Peter', 'Bocej', 'pbocej@gmail.com', '+421955444444', 1);
INSERT INTO [Users] ([UserName],[Password],[FirstName],[LastName],[Email],[Phone],[UserGroupId])
     VALUES('Ivana', '', 'Ivana', 'Vlasova', 'ivanav@gmail.com', '+421955111111', 1);
GO
