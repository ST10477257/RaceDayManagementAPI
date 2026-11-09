# StudentManagementAPI
1. Project Overview

RaceDay Management System is a web-based application developed to manage sporting events, race categories, routes and participant activities.

The system uses an ASP.NET Core Web API connected to a SQL Server database, together with an ASP.NET Core MVC application that provides users with an interactive web interface.

The application supports two main user roles: Organisers and Participants.

2. Technologies Used

C#

ASP.NET Core Web API

ASP.NET Core MVC

.NET 10

Entity Framework Core

Microsoft SQL Server

HTML5

CSS3

Bootstrap

JavaScript

Visual Studio 2026

Git and GitHub

3. Main Features

Authentication

User registration.

User login and logout.

Session-based authentication.

Role-based access for Organisers and Participants.

Organiser Features

Organiser dashboard.

Manage race categories.

Manage sporting events.

Manage event routes.

Create, view, update and delete management records.

Participant Features

Participant dashboard.

Browse sporting events.

View race categories.

Explore available routes.

4. Project Structure

The solution contains three main projects:

StudentManagementAPI: ASP.NET Core Web API responsible for handling application data and database operations.

StudentManagementAPI.Tests: Contains automated tests for the API.

RaceDayManagementMVC: ASP.NET Core MVC application providing the user interface.

5. Database

The application uses Microsoft SQL Server with Entity Framework Core.

The database contains tables for:

Users

Events

Categories

EventCategories

Enrolments

Results

Routes

6. How to Run the Application

Requirements

Visual Studio 2026.

.NET 10 SDK.

Microsoft SQL Server.

SQL Server Management Studio.

Setup Instructions

Clone the GitHub repository.

Open StudentManagementAPI.slnx in Visual Studio.

Configure the SQL Server connection string in the API project's appsettings.json.

Ensure the RaceDay database is available.

Configure the solution to start both StudentManagementAPI and RaceDayManagementMVC.

Run the solution.

Open the MVC application in your browser.

Register a Participant account or log in using an existing account.

API Documentation

The API provides Swagger documentation during development.

API URL:
https://localhost:7078/swagger

MVC Application:
https://localhost:7149

7. Authentication and Security

The system uses session-based authentication and role-based access control.

Public registration is restricted to Participant accounts. Organiser access is managed separately.

Passwords are hashed before being stored in the database.

8. Version Control

Git and GitHub were used to manage the development process.

The project includes a dedicated development branch:

part-3-mvc

This branch contains the MVC implementation and Part 3 development changes.

9. Developer

Name: Mbhoni Vernon Maluleke

Student Number: ST10477257

Course: Diploma in Software Development

Institution: Rosebank International

10. Repository

GitHub Repository:

https://github.com/ST10477257/RaceDayManagementAPI

Development Branch:

https://github.com/ST10477257/RaceDayManagementAPI/tree/part-3-mvc
