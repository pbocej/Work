IF object_id(N'v_UserProjects', 'V') IS NOT NULL
	DROP VIEW v_UserProjects
GO
CREATE VIEW v_UserProjects
AS
SELECT p.ProjectId, p.ProjectName
FROM Projects p
GO
