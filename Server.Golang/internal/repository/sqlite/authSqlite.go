package sqlite

import (
	"database/sql"
	"errors"
	"fmt"
	"time"
	emailveriftoken "url-shortener/internal/lib/random/emailVerifToken"
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

	stmt, err := r.db.Prepare("INSERT INTO Users(email,role,passwordHash,createdAt,emailVerified) VALUES(?,?,?,?,?)")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(u.Email, u.Role, u.PassHash, u.CreatedAt, u.EmailVerified)
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

	stmt, err := r.db.Prepare("SELECT ID, Email,role, passwordHash, CreatedAt, EmailVerified FROM Users WHERE Email = ? AND passwordHash = ?")
	if err != nil {
		return models.User{}, fmt.Errorf("%s: %w", opr, err)
	}

	user := models.User{}

	err = stmt.QueryRow(email, passHash).Scan(&user.ID, &user.Email, &user.Role, &user.PassHash, &user.CreatedAt, &user.EmailVerified)
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

func (r *AuthSqlite) CheckRefreshToken(token string) (models.User, error) {
	const opr = "repository.sqlite.authSqlite.RefreshToken"

	stmt, err := r.db.Prepare("SELECT userId,expiresAt FROM Sessions WHERE refreshToken = ?")
	if err != nil {
		return models.User{}, fmt.Errorf("%s: %w", opr, err)
	}

	session := models.Session{
		RefreshToken: token,
	}

	err = stmt.QueryRow(token).Scan(&session.UserID, &session.ExpiresAt)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return models.User{}, fmt.Errorf("%s: %w", opr, dberrs.ErrorInvalidOrExpToken)
		}
		return models.User{}, fmt.Errorf("%s: %w", opr, err)
	}

	if session.ExpiresAt.Compare(time.Now()) == -1 {
		r.DeleteRefreshToken(session.RefreshToken, session.UserID)
		return models.User{}, fmt.Errorf("%s: %w", opr, dberrs.ErrorInvalidOrExpToken)
	}

	stmt, err = r.db.Prepare("SELECT email,role FROM Users WHERE id = ?")
	if err != nil {
		return models.User{}, fmt.Errorf("%s: %w", opr, err)
	}

	user := models.User{
		ID: session.UserID,
	}

	err = stmt.QueryRow(session.UserID).Scan(&user.Email, &user.Role)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return models.User{}, fmt.Errorf("%s: %w", opr, dberrs.ErrorInvalidOrExpToken)
		}
		return models.User{}, fmt.Errorf("%s: %w", opr, err)
	}

	return user, nil
}

func (r *AuthSqlite) DeleteRefreshToken(token string, userID int) error {
	const opr = "repository.sqlite.authSqlite.DeleteRefreshToken"

	stmt, err := r.db.Prepare("DELETE FROM Sessions WHERE refreshToken = ? AND userId = ?")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	sl, err := stmt.Exec(token, userID)
	if count, _ := sl.RowsAffected(); count == 0 {
		return fmt.Errorf("%s: %w", opr, dberrs.ErrorURLNotFound)
	}
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}

func (r *AuthSqlite) CreateEmailVerification(email string, EmailVerifTokenTTL time.Duration) (models.EmailVerification, error) {
	const opr = "repository.sqlite.authSqlite.CreateEmailVerification"

	token, err := emailveriftoken.GenerateToken()
	if err != nil {
		return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, err)
	}

	EmailVerif := models.EmailVerification{
		Email:     email,
		Token:     token,
		ExpiresAt: time.Now().Add(EmailVerifTokenTTL),
	}

	stmt, err := r.db.Prepare("INSERT INTO EmailVerification(email,token,expiresAt) VALUES(?,?,?)")
	if err != nil {
		return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(EmailVerif.Email, EmailVerif.Token, EmailVerif.ExpiresAt)
	if err != nil {
		return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, err)
	}

	return EmailVerif, nil
}

func (r *AuthSqlite) GetVerifEmail(token, email string) (models.EmailVerification, error) {
	const opr = "repository.sqlite.authSqlite.VerifEmail"

	stmt, err := r.db.Prepare("SELECT expiresAt FROM EmailVerification WHERE token = ? AND email = ?")
	if err != nil {
		return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, err)
	}

	EmailVerif := models.EmailVerification{
		Email: email,
		Token: token,
	}

	err = stmt.QueryRow(token, email).Scan(&EmailVerif.ExpiresAt)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, dberrs.ErrorEmailVerifTokenExpired)
		}
		return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, err)
	}

	if EmailVerif.IsExpired() {
		err = r.DeleteEmailVerification(EmailVerif.Token)
		if err != nil {
			return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, err)
		}
		return models.EmailVerification{}, fmt.Errorf("%s: %w", opr, dberrs.ErrorEmailVerifTokenExpired)
	}

	return EmailVerif, nil
}

func (r *AuthSqlite) VerifEmail(EmailVerif models.EmailVerification) error {
	const opr = "repository.sqlite.authSqlite.VerifEmail"

	stmt, err := r.db.Prepare("UPDATE Users SET EmailVerified = true WHERE email = ?")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(EmailVerif.Email)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	err = r.DeleteEmailVerification(EmailVerif.Token)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}

func (r *AuthSqlite) DeleteEmailVerification(token string) error {
	const opr = "repository.sqlite.authSqlite.DeleteEmailVerification"

	stmt, err := r.db.Prepare("DELETE FROM EmailVerification WHERE token = ?")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(token)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}
