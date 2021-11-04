IF OBJECT_ID(N'dbo.v_UserWorkList', N'V') IS NOT NULL
BEGIN
	DROP VIEW v_UserWorkList
END
GO
CREATE VIEW v_UserWorkList
AS
SELECT wh.WorkHourID, wh.[Date], wh.[From], wh.[To], wh.[Hours], wh.ProjectId, p.ProjectName, 
	wh.[Subject], wh.[Description], wh.UserId
FROM dbo.WorkHours wh LEFT JOIN dbo.Projects p ON wh.ProjectId = p.ProjectId
GO

