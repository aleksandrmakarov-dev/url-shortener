package storage

import "errors"

var (
	ErrorURLNotFound = errors.New("url not found")
	ErrorEmailExists = errors.New("email already exists")
	ErrorURLAlias    = errors.New("alias already exists")
)
