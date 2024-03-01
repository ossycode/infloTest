## What I have done.
1. Implemented the Active Only interactive state – This should show only users where their IsActive property is set to true
2. Implemented Non Active interactive state – This should show only users where their IsActive property is set to false
3. Added DateOfBirth as part of the User entity
4. Implemented the code and UI flows for CRUD actions on a user
5. Extend the application to capture log information regarding primary actions performed on each user in the app.
6. The User view page contains a list of all actions that have been performed against that user.
7. Inmplemented a logs page containing a list of log entries across the application.
8. In the Logs page, the user is be able to click into each entry to see more detail about it. Logs details page
9. Provided pagination for the users and logs page,to provide a good user experience - even when there are many log or user entries.
10. Re-implement the UI. Created a new Blazor project as the frontend
11. Added a Web API project. To serve the Blazor project. Can be used for different frontend clients.
12. Updated the data access layer to support asynchronous operations.
13. Updated the data access layer to use a real database, and implement database schema migrations.

## Things I could have done.
1. Implement authentication and login based on the users being stored.
2. Implement bundling of static assets.
3. More Unit Testing
3. Introduce a Repository layer to decouple the service layer and data access layer
4. On the client, improve user experience by providing proper notification when actions are completing and completed or when resources is loading

# Project Setup Guide

## Prerequisites
- Visual Studio 2019 (or later) 
- .NET SDK installed (version 5.0 or later).

## Step 1: Clone the Repository
1. Open a terminal or command prompt.
2. Navigate to the directory where you want to clone the repository.
3. Run the following command to clone the repository:

  git clone <https://github.com/ossycode/infloTest.git>

## Step 2: Setting up the Local SQL Server Database
1. Navigate to the solution directory on your local machine.
2. Open the Solution in Visual Studio.
3. Go to the "View" menu.
4. Select "SQL Server Object Explorer" from the dropdown.
5. In SQL Server ->  `(localdb)\MSSQLLocalDB`  -> Databases, right-click on Databases
6. Select "Add New Database...".
7. Enter a name for the new database in the dialog box.
8. Click "OK" to create the database.

### Step 3: Retrieve Connection String
1. In Visual Studio, navigate to the "Server Explorer" or "SQL Server Object Explorer" window.
2. Find the database you created under the connected SQL Server instance.
3. Right-click on the database and select "Properties".
4. In the Properties window, locate the "Connection string" field.
5. Copy the connection string to your clipboard.

### Step 4: Update AppSettings.json
2. Locate the `appsettings.json` file in the `UserManagement.WebAPI` project.
3. Open the `appsettings.json` file.
4. Find the section where the connection string is defined. It may look like this:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=YourDatabaseName;Trusted_Connection=True;MultipleActiveResultSets=true;"
     }
   }

5. Replace the existing connection string value with the one you copied from SQL Server Object Explorer (Step 3).
6. Save the appsettings.json file.

### Step 4.1: Update Serilog Connfig in AppSettings.json
1. Locate the `appsettings.json` file in the `UserManagement.WebAPI` project.
1. Inside `appsettings.json`, find the section where Serilog is configured.
2. Locate the WriteTo section under Serilog.
3. In the `MSSqlServer` sink, replace the existing connection string value with the one you copied from SQL Server Object Explorer (Step 3).
4. Save the appsettings.json file.

```
NOTE: Follow the same instrustion to update the connection string in the UserManagement.Web project if you want to also run the MVC view
```

### Step 5: Performing Initial Code-First Database Migration with Entity Framework

After setting up your database and configuring the connection string in the `appsettings.json` file, you'll need to perform an initial migration to create the database schema. Follow these steps to do so:

1. **Open Package Manager Console**: In Visual Studio, navigate to `Tools` > `NuGet Package Manager` > `Package Manager Console`.
2. **Create Migration**: In the Package Manager Console, run the following command to create a new migration:
   ```bash
   Add-Migration InitialCreate
3. **Apply Migration**: Once the migration is created successfully, run the following command to apply the migration and update the database:
   ```bash
   Update-Database

### Step 6: Setting Up Startup Projects
To configure Visual Studio to start both your Blazor project and Web API project simultaneously, follow these steps:

1. Right-click on the solution in Visual Studio.
2. Select "Properties" from the context menu.
3. In the properties window, navigate to the "Startup Project" tab.
4. Choose the "Multiple startup projects" option.
5. Set the action for the `UserManagement.UI` project to "Start" and the `UserManagement.WebAPI` project to "Start" as well. Ensure both projects are set to start.

Click "Apply" and then "OK" to save the changes.

### Step 7: Start project
1. **Run Solution**: Press F5 or click on the "Start" button in Visual Studio toolbar to run the solution. This will start both the UserMangement.UI client and the UserMangement.WebAPI projects.
