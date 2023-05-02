EMedicalRecordAPISystemTask

EMedicalRecordAPISystemTask is a RESTful API built with ASP.NET Core that allows users to manage their information in Clinics.

Summary

The EMedicalRecordAPISystemTask provides a platform for users to manage their information in Clinics. 
This includes the ability to update their personal information, such as personal code, name, address, and phone number, as well as their profile picture. 
Additionally, users can create a new account or login with an existing one. The API also provides an option for admin users to delete other users.

Architecture and Design

The EMedicalRecordAPISystemTask follows a Clean architecture pattern (Is layered in 5 different projects).
The project uses Entity Framework Core to interact with a Microsoft SQL Server database. 
The API is secured using JSON Web Tokens (JWT) for authentication and authorization.

Installation

To run this application, you need to have .NET Core installed on your machine.
Set Up Database

    Open the appsettings.json file and update the connection string to your SQL Server instance.
    Open the Package Manager Console and run Update-Database to create the necessary database tables.

Running the Application

    Clone the repository: git clone https://github.com/zygis01/EMedicalRecordAPISystemTask.git
    Change directory: cd EMedicalRecordAPISystemTask
    Run the application: dotnet run

Usage

The API provides the following endpoints:
Authentication

To access the API, you need to authenticate with a JSON Web Token (JWT) by sending a POST request to the /api/Authentication/Login or /api/Authentication/LoginAdmin endpoint with the following payload:

json

{
  "username": "your_username",
  "password": "your_password"
}

The endpoint will return a JWT token that you can use to access protected endpoints by including it in the Authorization header of your requests.
Endpoints

    POST /api/Authentication/Login - login with existing credentials as User.

    POST /api/Authentication/LoginAdmin - login with existing credentials as Admin.

    POST /api/Authentication/Register - create a new user

    PUT /api/User/PersonalCode - update personal code.

    PUT /api/User/Update/Firstname - update firstname.

    PUT /api/User/Update/LastName - update lastname

    PUT /api/User/Update/PhoneNumber - update phone number.

    PUT /api/User/Update/Email - update email.

    PUT /api/User/Update/ProfilePicture - update profile picture.

    PUT /api/User/Update/Address/City - update address city.

    PUT /api/User/Update/Address/Street - update address street.

    PUT /api/User/Update/Address/HouseNum - update address house number.

    PUT /api/User/Update/Address/AppartmentNum - update address apartment number.

    GET /api/users/{id} - get a user by ID // only allowed with Admin role.

    DELETE /api/users/{id} - delete a user by ID // only allowed with Admin role.

Contributing and Issue Reporting

Contributions to the EMedicalRecordAPISystemTask project are always welcome. If you would like to contribute, please fork the repository and submit a pull request.

If you encounter any issues or have questions about the project, please create a new issue on the GitHub repository.

Tests

To run the unit tests, use the following command:

dotnet test

Built With

    ASP.NET Core 
    Entity Framework Core 
    Microsoft SQL Server
    xUnit