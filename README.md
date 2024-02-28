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
         "accessToken": "access_token_string",
         "userId": "user_id_string"
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

## Short URL
The `ShortUrlsController` class manages short URL operations in the API. Below is the documentation for each endpoint:

### Create
- **Route:** `POST api/v1/short-urls`
- **Description:** Creates a new short URL.
- **Request Body:** 
    ```json
    {
        "original": "https://example.com",
        "customAlias": "custom_alias_string",
        "userId": "user_id_string",
        "expiresAt": "optional_expiration_date"
    }
    ```
- **Success Response (200 OK):** 
    ```json
    {
        "id": "short_url_id",
        "original": "https://example.com",
        "alias": "short_url_alias",
        "domain": "http://localhost:5173",
        "expiresAt": "optional_expiration_date",
        "userId": "user_id_string"
    }
    ```
- **Error Response:** 
    - **400 Bad Request:** If the custom alias is already taken or if the user is not authorized to use a custom alias.

### GetByAlias
- **Route:** `GET api/v1/short-urls/{alias}`
- **Description:** Retrieves a short URL by its alias.
- **URL Parameter:** `alias` - The alias of the short URL.
- **Success Response (200 OK):** 
    ```json
    {
        "id": "short_url_id",
        "original": "https://example.com",
        "alias": "short_url_alias",
        "domain": "http://localhost:5173",
        "expiresAt": "optional_expiration_date",
        "userId": "user_id_string"
    }
    ```
- **Error Response:** 
    - **404 Not Found:** If the short URL with the provided alias does not exist.

### GetAll Short URLs

- **Route:** `GET api/v1/short-urls`
- **Description:** Retrieves all short URLs.
- **Query Parameters:**
  - `userId` (optional): Filters short URLs by user ID.
  - `page` (optional, default: 1): Specifies the page number for pagination.
  - `size` (optional, default: 10): Specifies the number of items per page.
  - `query` (optional): Searches for short URLs by alias.
- **Success Response (200 OK):** 

```json
{
    "items": [
        {
            "id": "short_url_id",
            "original": "https://example.com",
            "alias": "short_url_alias",
            "domain": "http://localhost:5173",
            "expiresAt": "optional_expiration_date",
            "userId": "user_id_string"
        },
        {
            "id": "another_short_url_id",
            "original": "https://anotherexample.com",
            "alias": "another_short_url_alias",
            "domain": "http://localhost:5173",
            "expiresAt": "optional_expiration_date",
            "userId": "user_id_string"
        }
    ],
    "pagination": {
        "page": 1,
        "size": 10,
        "query": "optional_query_string",
        "userId": "optional_user_id_string",
        "hasNextPage": true
    }
}
```

### UpdateById
- **Route:** `PUT api/v1/short-urls/{id}`
- **Description:** Updates an existing short URL by its ID.
- **URL Parameter:** `id` - The ID of the short URL.
- **Request Body:** 
    ```json
    {
        "original": "https://updatedexample.com",
        "customAlias": "updated_custom_alias_string",
        "expiresAt": "optional_updated_expiration_date"
    }
    ```
- **Success Response (204 No Content):** No content.
- **Error Response:** 
    - **400 Bad Request:** If the custom alias is already taken.
    - **404 Not Found:** If the short URL with the provided ID does not exist.

### DeleteById
- **Route:** `DELETE api/v1/short-urls/{id}`
- **Description:** Deletes a short URL by its ID.
- **URL Parameter:** `id` - The ID of the short URL.
- **Success Response (204 No Content):** No content.
- **Error Response:** 
    - **404 Not Found:** If the short URL with the provided ID does not exist.

#### Request Models
- **CreateShortUrlRequest:** Contains details for creating a short URL.
- **UpdateShortUrlRequest:** Contains details for updating a short URL.

#### Response Models
- **ShortUrlResponse:** Contains details of a short URL.

#### Errors
- **UnauthorizedException:** Indicates that the user is not authorized to perform the operation.
- **BadRequestException:** Indicates a malformed request.
- **NotFoundException:** Indicates that a requested resource was not found.
