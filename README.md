# App

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

### Sign Up with Email and Password
- **Method:** POST
- **Route:** `/sign-up`
- **Description:** Create a new user account with email and password.
- **Request Body:**
```json
{
  "email": "user@example.com",
  "password": "securepassword"
}
```
- **Response Body:** None

#### Error Responses:
- **Status Code:** 400 Bad Request
```json
{
  "status": 400,
  "error": "Bad Request",
  "message": "Invalid email format or password strength"
}
```
- **Status Code:** 409 Conflict
```json
{
  "status": 409,
  "error": "Conflict",
  "message": "User with this email already exists"
}
```


### Sign In with Email and Password
- **Method:** POST
- **Route:** `/sign-in`
- **Description:** Authenticate a user with email and password.
- **Request Body:**
```json
{
  "email": "user@example.com",
  "password": "securepassword"
}
```
- **Response Body:**
```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

- **Response Cookie:**
  - **Name:** refreshToken
  - **Value:** [Refresh Token Value]
  - **HttpOnly:** true

#### Error Responses:
- **Status Code:** 401 Unauthorized
```json
{
  "status": 401,
  "error": "Unauthorized",
  "message": "Invalid credentials"
}
```
### Verify Email
- **Method:** POST
- **Route:** `/verify-email`
- **Description:** Verify user's email with a verification token.
- **Request Body:**
```json
{
  "email": "user@example.com",
  "token": "verification_token"
}
```
- **Response Body:** None

#### Error Responses:
- **Status Code:** 404 Not Found
```json
{
  "status": 404,
  "error": "Not Found",
  "message": "User or verification token not found"
}
```
- **Status Code:** 401 Unauthorized
```json
{
  "status": 401,
  "error": "Unauthorized",
  "message": "Verification token has expired"
}
```

### Refresh Token
- **Method:** POST
- **Route:** `/refresh-token`
- **Description:** Refresh access token using refresh token stored in cookie.
- **Request Cookie:**
  - **Name:** refreshToken
  - **Value:** [Refresh Token Value]
- **Response Body (Success):**
```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 123,
    "email": "user@example.com",
    "name": "User Name"
  }
}
```
- **Response Body (Error):**
```json
{
  "status": 401,
  "error": "Unauthorized",
  "message": "Invalid or expired refresh token"
}
```

### Create Short URL
- **Method:** POST
- **Route:** `/short-url`
- **Description:** Create a shortened URL with an optional alias.
- **Request Body:**
```json
{
  "original": "https://www.example.com/long/url",
  "alias": "custom_alias"
}
```
- **Request Headers:** (Optional) Access Token
- **Response Body:**
```json
{
  "original": "https://www.example.com/long/url",
  "shortened": "https://example.com/abc123",
  "alias": "custom_alias"
}
``` 

#### Error Responses:
- **Status Code:** 400 Bad Request
```json
{
  "status": 400,
  "error": "Bad Request",
  "message": "Invalid URL format"
}
```
- **Status Code:** 401 Unauthorized
```json
{
  "status": 401,
  "error": "Unauthorized",
  "message": "Access token required to set custom alias"
}
```

#### Update Short URL
- **Method:** `PUT`
- **Route:** `/short-url/:id`
- **Description:** Update the original URL or alias of a shortened URL.
- **Request Parameters:**
  - **id:** The ID of the short URL to be updated.
- **Request Body:**
```json
{
  "original": "https://www.example.com/new/long/url",
  "alias": "new_alias"
}
```
- **Request Headers:** `Access Token` (Optional)
- **Response Body:**
```json
{
  "original": "https://www.example.com/new/long/url",
  "shortened": "https://example.com/abc123",
  "alias": "new_alias"
}
```

#### Error Responses:

- **Status Code:** `401 Unauthorized`
```json
{
  "status": 401,
  "error": "Unauthorized",
  "message": "Access token required to update short URL"
}
```
- **Status Code:** `404 Not Found`
```json
{
  "status": 404,
  "error": "Not Found",
  "message": "Short URL not found"
}
```

#### Delete Short URL
- **Method:** `DELETE`
- **Route:** `/short-url/:id`
- **Description:** Delete a shortened URL.

#### Error Responses:
- **Status Code:** `401 Unauthorized`
```json
{
  "status": 401,
  "error": "Unauthorized",
  "message": "Access token required to delete short URL"
}
```
- **Status Code:** `404 Not Found`
```json
{
  "status": 404,
  "error": "Not Found",
  "message": "Short URL not found"
}
```
