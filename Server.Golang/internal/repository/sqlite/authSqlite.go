package sqlite

import (
	"database/sql"
	"errors"
	"fmt"
	"time"
	refreshtoken "url-shortener/internal/lib/random/refreshToken"
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

	stmt, err := r.db.Prepare("SELECT ID, Email, passwordHash, CreatedAt, EmailVerified FROM Users WHERE Email = ? AND passwordHash = ?")
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

func (r *AuthSqlite) GenRefreshToken(u *models.User, tokenExp time.Duration) (models.Session, error) {
	const opr = "repository.sqlite.authSqlite.GenRefreshToken"
	reftoken, err := refreshtoken.GenerateRefreshToken()
	if err != nil {
		return models.Session{}, fmt.Errorf("%s: %w", opr, err)
	}

	session := models.Session{
		UserID:       u.ID,
		RefreshToken: reftoken,
		ExpiresAt:    time.Now().Add(tokenExp),
	}

	stmt, err := r.db.Prepare("INSERT INTO Sessions(userId,refreshToken,expiresAt) VALUES(?,?,?)")
	if err != nil {
		return models.Session{}, fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(session.UserID, session.RefreshToken, session.ExpiresAt)
	if err != nil {
		return models.Session{}, fmt.Errorf("%s: %w", opr, err)
	}

	return session, nil
}

func (r *AuthSqlite) CheckRefreshToken(token string) (int, error) {
	const opr = "repository.sqlite.authSqlite.RefreshToken"

	stmt, err := r.db.Prepare("SELECT userId, expiresAt FROM Sessions WHERE refreshToken = ?")
	if err != nil {
		return 0, fmt.Errorf("%s: %w", opr, err)
	}

	session := models.Session{
		RefreshToken: token,
	}

	err = stmt.QueryRow(token).Scan(&session.UserID, &session.ExpiresAt)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return 0, fmt.Errorf("%s: %w", opr, dberrs.ErrorInvalidOrExpToken)
		}
		return 0, fmt.Errorf("%s: %w", opr, err)
	}

	if session.ExpiresAt.Compare(time.Now()) == -1 {
		return 0, fmt.Errorf("%s: %w", opr, dberrs.ErrorInvalidOrExpToken)
	}

	return session.UserID, nil
}
