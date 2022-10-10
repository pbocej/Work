USE MyWork
GO
-- create table
CREATE TABLE [UserGroups](
	[UserGroupId] [int] NOT NULL,
	[GroupName] [varchar](50) NOT NULL,
	[GroupType] [bit] NOT NULL,
 CONSTRAINT [PK_UserGroups] PRIMARY KEY CLUSTERED 
(
	[UserGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - administrators; 1 - users' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroups', @level2type=N'COLUMN',@level2name=N'GroupType'
GO

-- insert data
INSERT INTO UserGroups (UserGroupId, GroupName, GroupType)
VALUES (0, 'Administrators', 0);
INSERT INTO UserGroups (UserGroupId, GroupName, GroupType)
VALUES (1, 'Users', 1);
