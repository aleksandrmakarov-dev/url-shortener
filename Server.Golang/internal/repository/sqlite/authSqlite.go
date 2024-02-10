package sqlite

import (
	"database/sql"
	"errors"
	"fmt"
	"url-shortener/internal/models"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/mattn/go-sqlite3"
)

type AuthSqlite struct {
	db *sql.DB
}

func NewAuthSqlite(db *sql.DB) *AuthSqlite {
	return &AuthSqlite{
		db: db,
	}
}

func (r *AuthSqlite) CreateUser(u *models.User) error {
	const opr = "repository.sqlite.authSqlite.CreateUser"

	stmt, err := r.db.Prepare("INSERT INTO Users(email,passwordHash,createdAt,emailVerified) VALUES(?,?,?,?)")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(u.Email, u.PassHash, u.CreatedAt, u.EmailVerified)
	if err != nil {
		if sqliteErr, ok := err.(sqlite3.Error); ok && sqliteErr.ExtendedCode == sqlite3.ErrConstraintUnique {
			return fmt.Errorf("%s: %w", opr, dberrs.ErrorEmailExists)
		}

		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}

func (r *AuthSqlite) GetUser(email, passHash string) (models.User, error) {
	const opr = "repository.sqlite.authSqlite.GetUser"

	stmt, err := r.db.Prepare("SELECT ID, Email, PassHash, CreatedAt, EmailVerified FROM users WHERE Email = ? AND PassHash = ?")
	if err != nil {
		return models.User{}, fmt.Errorf("%s: %w", opr, err)
	}

	user := models.User{}

	err = stmt.QueryRow(email, passHash).Scan(&user.ID, &user.Email, &user.PassHash, &user.CreatedAt, &user.EmailVerified)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return models.User{}, fmt.Errorf("%s: %w", opr, dberrs.ErrorInvalidCredentials)
		}
		return models.User{}, fmt.Errorf("%s: %w", opr, err)
	}

	return user, nil
}
