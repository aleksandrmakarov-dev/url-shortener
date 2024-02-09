package repository

import (
	"database/sql"

	"url-shortener/internal/models"
	"url-shortener/internal/repository/sqlite"
)

type Auth interface {
	CreateUser(u *models.User) error
	GetUser(email, passHash string) (models.User, error)
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
