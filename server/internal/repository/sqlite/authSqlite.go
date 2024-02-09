package sqlite

import (
	"database/sql"
	"url-shortener/internal/models"
)

type AuthSqlite struct {
	db *sql.DB
}

func NewAuthSqlite(db *sql.DB) *AuthSqlite {
	return &AuthSqlite{
		db: db,
	}
}

func (r *AuthSqlite) CreateUser(u models.User) error {

	return nil
}

func (r *AuthSqlite) GetUser(username, passwordHash string) (models.User, error) {

	return models.User{}, nil
}
