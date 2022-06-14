# Student time manager

20104681POE Version 3.0 17/11/2021

Usage notes
------------------------------------------------------------------------------

--Program Description
- This web application is a personal time planner. It allows for a user to 
specify a semester duration, add multiple modules with criteria such as 
module credits and module hours, then select on which days they wan't to
study on and how many hours they would like to study. The web application
will then show the user a calendar with information on which days they
plan to study and how many hours of self study are left for each module
based on their planned hours for the week they choose to study in. This 
web application also allows for a user to view their data on a graph that
displays the total amount of hours they spend on each module each week.

- Does not support Windows Server
- Does not support macos

--Program Prerequisites
- Visual Studio 2019
- Microsoft SQL Server Management Studio

--To Run

- Navigate to 20104681JoshMkhariPOE\20104681JoshMkhariPOE and then run the
Visual Studio Solution named "20104681JoshMkhariPOE.sln"
-Once Visual Studio has launched the solution, simply select the green
compile option
------------------------------------------------------------------------------

Installation notes
------------------------------------------------------------------------------
- This program does not require installation.
- Watch the first video on youtube  for a demonstration on how to setup 
and connect the database

    	===	    https://youtu.be/QgrQZ_kZFmU     ===

- Watch the second video for a detailed demonstration of the web application 
in use

    	===	    https://youtu.be/O0LV6DePvK8     ===




Above video includes setup process.
And program running with dummy data.

Steps to setup program if avoiding the above video 

1) Start Microsoft SQL Server Management Studio
2) Select Databases after connecting to a database engine 
	Then Import Data Tier application
	Select the bacpac file found in 20104681JoshMkhariPROG6212Task2\Database
3) Once import is complete, run the .sln file found in 
	20104681JoshMkhariPOE\20104681JoshMkhariPOE
4) Navigate through the folder Model
	Select the class file named "ProgramDal"
5) Using the "Server Explorer"
	Right click "Data Connections"
	Click on "Add Connection"
	Data Source is "Microsoft SQL Sever (SqlClient)
	Server name:
		To find the name, return to Microsoft SQL Server Management Studio
		Under Object Explorer click "Connect"
		Select "Database Engine"
		Now copy the Server Name
		Return to Visual Studio
	Paste the copied Server Name
	Select or enter database name "20104681PROGTask2JoshMkhari"
	Click "Ok"
6) Using the "Server Explorer"
	Right click on the newly created data connection
	Click on "Properties"
	Copy the connection String
7) Navigate to line 9 of the "ProgramDal"
	Paste the copied connection string.
8) Run the program
9) For further help, refer to either
	The youtube video === https://youtu.be/QgrQZ_kZFmU ===
	The PDF document titled "Manual_20104681JoshMkhariPOE.pdf"
	
------------------------------------------------------------------------------

Usability notes
------------------------------------------------------------------------------
- There is a dedicated manual named "Manual_20104681JoshMkhariPOE.pdf" which can
be used to navigate the web-app once it is running.
------------------------------------------------------------------------------

Contact information
E-mail: 20104681@vcconnect.co.za

This program is the final task in an ongoing POE (portfolio of evidence).
