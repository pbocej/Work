USE MyWork
GO
-- create table
CREATE TABLE Projects (
	[ProjectId] int NOT NULL IDENTITY PRIMARY KEY,
	[ProjectName] nvarchar(100) NOT NULL,
	[ProjectDescription] ntext)
GO
-- insert data
INSERT INTO Projects (ProjectName, ProjectDescription)
VALUES ('Dochadzka', 'Interny dochadzkovy system');
INSERT INTO Projects (ProjectName, ProjectDescription)
VALUES ('Pokladna', 'Interny pokladnovy system');
INSERT INTO Projects (ProjectName, ProjectDescription)
VALUES ('Autopark', 'Sprava autoparku');
INSERT INTO Projects (ProjectName, ProjectDescription)
VALUES ('Pracovny dennik', 'Interny pracovny dennik');
GO