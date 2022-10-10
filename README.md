# Work hours
Work hours basic evidence
## Requirements
* Visual Studio 2017 +
* MS SQL Server 2008 +
* .Net 4.8.1
## Installation
1. Create database on SQL Server with script /SQL/00 - Databasse prepare.sql
2. Create tables in database with all scripts in /SQL/Tables
3. Create views in database with all scripts in /SQL/Views
4. Setup connection string in /Work/App.config connectionStrings section
## Using
After starting program, You must login with blank passwords as:
* Admin - administrator
* Peter - me
* Ivana - test user
 
In administrator mode You can manage users, projects and work hours for any user.

In user modo You can only manage Your work hours.
