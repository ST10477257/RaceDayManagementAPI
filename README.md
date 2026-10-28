# StudentManagementAPI
1. Project Overview

The RaceDay Management API is a RESTful web service developed using ASP.NET Core Web API, C#, Entity Framework Core, and Microsoft SQL Server.

The system is designed to support the management of running and cycling events. It provides an API structure for managing users, event categories, events, enrolments, results, and routes.

The API uses Swagger for interactive documentation and GitHub Actions to automate building and testing the solution.

2. Technologies Used
C#
ASP.NET Core Web API
.NET 10
Entity Framework Core
Microsoft SQL Server
Swagger UI
xUnit or the test framework configured in the test project
Moq for mocking dependencies where required
Git and GitHub
GitHub Actions
3. Project Structure

The solution contains the following main components:

StudentManagementAPI/Controllers contains API controllers.
StudentManagementAPI/Data contains the database context.
StudentManagementAPI/Models contains the application's data models.
StudentManagementAPI/Properties contains application launch settings.
StudentManagementAPI/appsettings.json contains application configuration.
StudentManagementAPI.Tests contains unit tests.
.github/workflows/dotnet.yml contains the GitHub Actions workflow.

The project currently retains the StudentManagementAPI project name from its original setup, while the application is being developed for RaceDay event management.

4. Database Configuration

The application uses Microsoft SQL Server with Entity Framework Core.

The connection string is configured in appsettings.json.

Example:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=RaceDay;Trusted_Connection=True;TrustServerCertificate=True;"
}

Before running the application:

Install Microsoft SQL Server and SQL Server Management Studio if they are not already installed.
Ensure that the SQL Server instance is running.
Create or restore the RaceDay database using the project's database script.
Verify that the database tables and sample data exist.
Update the connection string if your SQL Server instance uses a different server name or authentication method.

Do not run database creation scripts against an existing database without checking their contents first.

5. Running the Application
Requirements
Visual Studio with ASP.NET and web development tools.
.NET 10 SDK.
Microsoft SQL Server.
Git.
Steps
Clone the repository or open the existing project folder.
Open StudentManagementAPI.slnx in Visual Studio.
Restore the NuGet packages.
Check the connection string in appsettings.json.
Build the solution using Build > Build Solution.
Set StudentManagementAPI as the startup project.
Run the application using the HTTPS launch profile.

The application should start if the project builds successfully and the required configuration is available.

6. Swagger API Documentation

Swagger provides an interactive interface for viewing and testing API endpoints.

After starting the application, open the Swagger URL shown in the application output. In the current development configuration, the URL is:

https://localhost:7078/swagger/index.html

The port may differ depending on the launch settings.

Swagger can be used to inspect endpoints, enter request data, submit requests, and view HTTP responses.

7. Unit Testing

The solution includes a separate test project named StudentManagementAPI.Tests.

To run the tests in Visual Studio:

Open Test > Test Explorer.
Select Run All Tests.
Review the test results.

Alternatively, run the following commands from the solution directory:

dotnet restore StudentManagementAPI.slnx

dotnet build StudentManagementAPI.slnx --configuration Release

dotnet test StudentManagementAPI.slnx --configuration Release

The tests help verify that the implemented application functionality behaves as expected.

8. GitHub Actions CI/CD

The repository includes a GitHub Actions workflow at:

.github/workflows/dotnet.yml

The workflow runs automatically when changes are pushed to the master branch or when a pull request targets that branch.

The workflow performs the following tasks:

Checks out the repository.
Sets up the .NET SDK.
Restores project dependencies.
Builds the solution.
Runs the automated tests.

Successful workflow runs are displayed in the repository's Actions tab.

Repository:

https://github.com/ST10477257/RaceDayManagementAPI

9. Security Considerations

The application is being developed with security and access control in mind.

Security features should include password hashing, validation of user input, appropriate participant and organiser permissions, and protection of restricted API endpoints.

Database credentials and other sensitive configuration values should not be committed to a public repository. Authentication and authorisation features must be tested before the application is considered ready for production.

10. Future Improvements

Potential improvements include:

Completing and testing all required event management endpoints.
Completing authentication and role-based authorisation.
Expanding unit test coverage.
Improving error handling and validation.
Adding further automated integration tests.
Improving API documentation and deployment configuration.
11. Author

Student project developed for the Programming 2B practical coursework.

12. Repository

GitHub: https://github.com/ST10477257/RaceDayManagementAPI
