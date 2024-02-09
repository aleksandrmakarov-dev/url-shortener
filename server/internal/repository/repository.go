package repository

import (
	"database/sql"
	"errors"
	"url-shortener/internal/models"
	"url-shortener/internal/repository/sqlite"
)

var (
	ErrorEmailExists            = errors.New("email already exists")
	ErrorWrongPassword          = errors.New("wrong password")
	ErrorEmailDoesNotVerified   = errors.New("email does not verified")
	ErrorEmailVerifTokenExpired = errors.New("email verif token expired")

	ErrorURLAliasExists = errors.New("alias already exists")
	ErrorURLExpired     = errors.New("url expired")
	ErrorURLNotFound    = errors.New("url not found")
)

type Auth interface {
	CreateUser(u models.User) error
	GetUser(username, passwordHash string) (models.User, error)
}

type Url interface {
}

type Repository struct {
	Auth
	Url
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{
		Auth: sqlite.NewAuthSqlite(db),
		Url:  sqlite.NewUrlSqlite(db),
	}
}
