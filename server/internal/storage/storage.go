package storage

import "errors"

var (
	ErrorURLNotFound            = errors.New("url not found")
	ErrorEmailExists            = errors.New("email already exists")
	ErrorURLAliasExists         = errors.New("alias already exists")
	ErrorURLExpired             = errors.New("url expired")
	ErrorEmailVerifTokenExpired = errors.New("email verif token expired")
)
