package storage

import "errors"

var (
	ErrorEmailExists            = errors.New("email already exists")
	ErrorWrongPassword          = errors.New("wrong password")
	ErrorEmailDoesNotVerified   = errors.New("email does not verified")
	ErrorEmailVerifTokenExpired = errors.New("email verif token expired")

	ErrorURLAliasExists = errors.New("alias already exists")
	ErrorURLExpired     = errors.New("url expired")
	ErrorURLNotFound    = errors.New("url not found")
)
