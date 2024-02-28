# App

## Contributers

- Server.Golang - [Crysta1Cat](https://github.com/Crysta1Cat)
- Client.React and Server.Csharp - [Alex](https://github.com/aleksandrmakarov-dev)

## Application Structure

```
------------------                         --------------------
| React Client  | ------ HTTP Requests --> |    Web API        |
------------------                         --------------------
        |                                          |
        |            ----------------------        |          ------------------
        |            |   SQL Database    |  -------|--------- |    Redis Cache  |
        |            ----------------------                   ------------------
        |                                       
------------------                         
|   Browser      |                         
------------------                         
```

# API Documentation

## Structure

```
.
└── src
    ├── controllers
    │   ├── ShortURLsController
    │   ├── AuthController
    │   └── UsersController
    ├── models
    │   ├── ShortURL
    │   ├── Auth
    │   └── User
    ├── services
    │   ├── ShortURLsService
    │   ├── AuthService
    │   └── UsersService

```


## Common

### Models

#### Message Response
```json
{
  "title": "Message Title",
  "message": "Message Content"
}
```

#### Error Response
```json
{
  "status": "Error Status Code",
  "error": "Error Type",
  "message": "Error Message"
}
```


## Base URL
`https://api.example.com/api/v1`

## Authentication
For endpoints requiring authentication, include an access token in the request headers.

## Endpoints

The `AuthController` class handles authentication-related HTTP requests in the API. Below is the documentation for each endpoint:

### SignUp
- **Route:** `POST api/v1/auth/sign-up`
- **Description:** Registers a new user.
- **Request Body:** 
    ```json
    {
        "email": "user@example.com",
        "password": "password123"
    }
    ```
- **Success Response (200 OK):** 
    ```json
    {
        "title": "Complete the registration",
        "message": "You will receive verification code to the email address"
    }
    ```
- **Error Response:** 
    - **400 Bad Request:** If the email is already registered.

### SignIn
- **Route:** `POST api/v1/auth/sign-in`
- **Description:** Authenticates a user.
- **Request Body:** 
    ```json
    {
        "email": "user@example.com",
        "password": "password123"
    }
    ```
- **Success Response (200 OK):** 
    ```json
    {
        "refreshToken": "refresh_token_string",
        "session": {
            "accessToken": "access_token_string",
            "userId": "user_id_string"
        }
    }
    ```
- **Error Response:** 
    - **400 Bad Request:** If the email is not verified or invalid credentials.

### VerifyEmail
- **Route:** `POST api/v1/auth/verify-email`
- **Description:** Verifies a user's email address.
- **Request Body:** 
    ```json
    {
        "email": "user@example.com",
        "token": "verification_token_string"
    }
    ```
- **Success Response (200 OK):** 
    ```json
    {
        "title": "Registration is completed",
        "message": "Your email address is verified. You can sign in to your account"
    }
    ```
- **Error Response:** 
    - **400 Bad Request:** If the verification token is invalid or expired.

### RefreshToken
- **Route:** `POST api/v1/auth/refresh-token`
- **Description:** Generates a new access token using a refresh token.
- **Request Body:** None
- **Success Response (200 OK):** 
    ```json
    {
        "accessToken": "new_access_token_string",
        "userId": "user_id_string"
    }
    ```
- **Error Response:** 
    - **400 Bad Request:** If the refresh token is invalid or expired.

### NewEmailVerification
- **Route:** `POST api/v1/auth/new-email-verification`
- **Description:** Sends a new email verification token to the user's email address.
- **Request Body:** 
    ```json
    {
        "email": "user@example.com"
    }
    ```
- **Success Response (200 OK):** 
    ```json
    {
        "title": "New email verification",
        "message": "New email verification token is sent to your email address"
    }
    ```
- **Error Response:** 
    - **200 OK:** If the email is already verified.

### ExpireSession
- **Route:** `DELETE api/v1/auth/sign-out`
- **Description:** Expires the current user session.
- **Request Body:** 
    ```json
    {
        "token": "refresh_token_string"
    }
    ```
- **Success Response (204 No Content):** No content.
- **Error Response:** 
    - **400 Bad Request:** If the refresh token is invalid or expired.

#### Request Models
- **SignInRequest:** Contains user email and password.
- **SignOutRequest:** Contains a refresh token.
- **SignUpRequest:** Contains user email and password.
- **VerifyEmailRequest:** Contains user email and verification token.
- **NewEmailVerificationRequest:** Contains user email.

#### Response Models
- **MessageResponse:** Contains a title and a message.
- **SignInResponse:** Contains a refresh token and a session response.
- **SessionResponse:** Contains an access token and a user ID.

#### Errors
- **BadRequestException:** Indicates a malformed request.
- **UnauthorizedException:** Indicates authentication or authorization failure.
- **NotFoundException:** Indicates that a requested resource was not found.
