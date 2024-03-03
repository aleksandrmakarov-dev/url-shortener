package dberrs

import "errors"

var (
	ErrorEmailExists        = errors.New("email already exists")
	ErrorInvalidCredentials = errors.New("invalid credentials")

	ErrorInvalidOrExpToken = errors.New("invalid or expired refresh token")

	ErrorEmailDoesNotVerified   = errors.New("email does not verified")
	ErrorEmailVerifTokenExpired = errors.New("email verif token expired")

	ErrorURLAliasExists = errors.New("alias already exists")
	ErrorURLExpired     = errors.New("url expired")
	ErrorURLNotFound    = errors.New("url not found")
	ErrorURLSNotFound   = errors.New("urls not found")
)
