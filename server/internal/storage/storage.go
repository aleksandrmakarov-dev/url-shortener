package storage

import "errors"

var (
	ErrorURLNotFound    = errors.New("url not found")
	ErrorEmailExists    = errors.New("email already exists")
	ErrorURLAliasExists = errors.New("alias already exists")
	ErrURLExpired       = errors.New("url expired")
)
