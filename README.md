# Timeable app

Timeable is a digital signature application created for Tallinn University of Technology IT College as Bachelor's final thesis project. 

## Project technologies

- Timeable uses .NET Core 3.1 as web project framework and .NET Standard 2.1 as class library projects' framework. 
- SQL Server Express is used by default.
- EntityFramework Core provides ORM functionality.

## Starting with Timeable

#### Database connection

Timeable uses SQL Express Server by default. Connection string is specified in *appsettings.json* file in TimeableAppWeb project. *Microsoft.EntityFrameworkCore.SqlServer* NuGet package was used to establish connection to a database.

Changing database might need to add additional NuGet package.

#### Database migration and data seeding

Database migration and identity/data seed on application start can be turned on/off in *appsettings.json* file in TimeableAppWeb project.

AppDataInitialization object's parameters:

 - **DropDatabase** - drops the database (*default - false*).
 - **MigrateDatabase** - creates the database using migrations (*default - true*).
 - **SeedIdentity** - adds specified roles and users to database (*default - true*).
 - **SeedData** - downloads todays schedule from oit.ttu.ee and stores it into the database (*default - true*). 
 
 ### **NB!** Turning on SeedData makes first start of the application slower as it takes about 20 seconds to download the schedule from ois.ttu.ee. Second start checks if there is schedule with the same prefix and skips the update.

 #### Prefix

 Prefix is used to search for the lectures which are hapening in the rooms that names start with the specified prefix. 
 Prefix can be specified in the *appsettings.json* file at first and then it could be changed from the application.

#### Initial roles and user credentials

Initially roles and user are created with SeedIdentity method in DAL.App.Helpers.DataInitializers class. Names of user roles and credentials used for initial user are specified there. Initial user gets the HeadAdmin role.

#### E-mail sender

Email sender function was to restore forgotten password and to notify user if he has been registered and needs to verify his account. E-mail uses SMTP client which can be configured in *appsettings.json* file. 

**If any other bug was found, please contact Elina Antonova: jelisa106@gmail.com** 
