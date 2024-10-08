# Web API Project with Clean Architecture

This project is a Web API that implements a simple user management system using Clean Architecture principles, CQRS pattern, and Dapper ORM for database interactions. It includes user signup, authentication with JWT, and balance retrieval functionalities.

## Features

1. **User Signup API**
   - **Endpoint**: `~/users/signup`
   - **Method**: `POST`
   - **Request Body**:
     ```json
     {
       "username": "waseem@gmail.com",
       "password": "dfdffdfd",
       "firstname": "Waseem",
       "lastname": "Muhammad",
       "device": "12fdr112233",
       "ipaddress": "172.24.1.56"
     }
     ```
   - **Response**: 
     - **Status Code**: `200 OK`
     - **Description**: Successfully saves the user details in the database.

2. **User Authentication API**
   - **Endpoint**: `~/users/authenticate`
   - **Method**: `POST`
   - **Request Body**:
     ```json
     {
       "username": "waseem@gmail.com",
       "password": "dfdffdfd",
       "ipaddress": "172.23.5.67",
       "device": "12fdr112233",
       "browser": "chrome"
     }
     ```
   - **Response**: 
     - **Status Code**: `200 OK`
     - **Response Body**:
     ```json
     {
       "firstname": "Waseem",
       "lastname": "Muhammad",
       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
     }
     ```
     - **Description**: On successful authentication, a JWT is generated, and a balance of 5 GBP is added to the user's account if it's their first login.

3. **User Balance API**
   - **Endpoint**: `~/users/auth/balance`
   - **Method**: `POST`
   - **Request Body**:
     ```json
     {
       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
     }
     ```
   - **Response**: 
     - **Status Code**: `200 OK`
     - **Response Body**:
     ```json
     {
       "balance": 5.0
     }
     ```
     - **Description**: Returns the current balance of the user associated with the provided token.

## Architecture

- **Clean Architecture**: The project follows Clean Architecture principles to ensure separation of concerns, making the application easier to maintain and test.
- **CQRS Pattern**: The Command Query Responsibility Segregation (CQRS) pattern is used to separate read and write operations.
- **MediatR**: The MediatR library is used to implement the CQRS pattern, allowing for decoupled request handling.
- **Dapper ORM**: Dapper is utilized for data access, providing a lightweight and high-performance solution for executing SQL queries.

## Middleware and Error Handling

- **Custom Middleware**: The application includes middleware for global error handling to catch and respond to exceptions in a consistent manner.

