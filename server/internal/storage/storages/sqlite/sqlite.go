package sqlite

import (
	"database/sql"
	"errors"
	"fmt"
	"time"
	emailverificationsender "url-shortener/internal/lib/messageSender/emailVerificationSender"
	emailveriftoken "url-shortener/internal/lib/random/emailVerifToken"
	"url-shortener/internal/storage"
	storagetypes "url-shortener/internal/storage/storageTypes"

	"github.com/mattn/go-sqlite3"
	_ "github.com/mattn/go-sqlite3"
)

type Storage struct {
	db *sql.DB
}

var queries = []string{
	`CREATE TABLE IF NOT EXISTS Users (
		id INTEGER PRIMARY KEY,
		email VARCHAR(255) NOT NULL UNIQUE,
		password VARCHAR(255) NOT NULL,
		createdAt DATETIME NOT NULL,
		emailVerified BOOLEAN NOT NULL
	);
	CREATE INDEX IF NOT EXISTS idx_user_id ON User(id);`,

	`CREATE TABLE IF NOT EXISTS Sessions (
		id INTEGER PRIMARY KEY,
		userId INTEGER REFERENCES User(id),
		refreshToken VARCHAR(255) NOT NULL UNIQUE,
		expiresAt DATETIME NOT NULL
	);
	CREATE INDEX IF NOT EXISTS idx_account_userId ON Account(userId);
	CREATE INDEX IF NOT EXISTS idx_account_refreshToken ON Account(refreshToken);`,

	`CREATE TABLE IF NOT EXISTS Urls (
		id INTEGER PRIMARY KEY,
		alias TEXT NOT NULL UNIQUE,
		redirect VARCHAR(255) NOT NULL,
		userId INTEGER REFERENCES User(id),
		expiresAt DATETIME NULL,
		navigations INT NOT NULL
	);
	CREATE INDEX IF NOT EXISTS idx_url_userId ON Url(userId);
	CREATE INDEX IF NOT EXISTS idx_url_alias ON Url(alias);`,

	`CREATE TABLE IF NOT EXISTS EmailVerification (
		email TEXT NOT NULL,
		token TEXT NOT NULL,
		expiresAt DATETIME NULL
	);
	CREATE INDEX IF NOT EXISTS idx_EmailVerification_token ON EmailVerification(token);
	`,
}

func New(storagePath string) (*Storage, error) {
	const opr = "storage.storages.sqlite.New"

	db, err := sql.Open("sqlite3", storagePath)
	if err != nil {
		return nil, fmt.Errorf("%s: %w", opr, err)
	}

	for _, v := range queries {
		stmt, err := db.Prepare(v)
		if err != nil {
			return nil, fmt.Errorf("%s: %w", opr, err)
		}

		_, err = stmt.Exec()

		if err != nil {
			return nil, fmt.Errorf("%s: %w", opr, err)
		}
	}

	return &Storage{db: db}, nil

}

func (s *Storage) SaveURL(UrlToSave storagetypes.Url) error {
	const opr = "storage.storages.sqlite.SaveURL"

	stmt, err := s.db.Prepare("INSERT INTO Urls(alias,redirect,userId,createdAt,expiresAt,navigations) VALUES(?,?,?,?,?,?)")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	if UrlToSave.UserID == 0 {
		_, err := stmt.Exec(UrlToSave.Alias, UrlToSave.RedirectUrl, UrlToSave.UserID, time.Now(), time.Now().Add(time.Hour*48), 0)
		if err != nil {
			if sqliteErr, ok := err.(sqlite3.Error); ok && sqliteErr.ExtendedCode == sqlite3.ErrConstraintUnique {
				return fmt.Errorf("%s: %w", opr, storage.ErrorURLAliasExists)
			}
			return fmt.Errorf("%s: %w", opr, err)
		}
		return nil
	}

	_, err = stmt.Exec(UrlToSave.Alias, UrlToSave.UserID, UrlToSave.UserID, time.Now(), UrlToSave.ExpiresAt, 0)
	if err != nil {
		if sqliteErr, ok := err.(sqlite3.Error); ok && sqliteErr.ExtendedCode == sqlite3.ErrConstraintUnique {
			return fmt.Errorf("%s: %w", opr, storage.ErrorURLAliasExists)
		}
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}

func (s *Storage) GetURL(alias string) (storagetypes.Url, error) {
	const opr = "storage.storages.sqlite.GetURL"

	stmt, err := s.db.Prepare("SELECT redirect, userId, createdAt, expiresAt, navigations FROM Urls WHERE alias = ?")
	if err != nil {
		return storagetypes.Url{}, fmt.Errorf("%s: %w", opr, err)
	}

	var (
		redirect    string
		userId      int
		createdAt   time.Time
		expiresAt   time.Time
		navigations int
	)
	err = stmt.QueryRow(alias).Scan(&redirect, &userId, &createdAt, &expiresAt, &navigations)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return storagetypes.Url{}, fmt.Errorf("%s: %w", opr, err)
		}
		return storagetypes.Url{}, fmt.Errorf("%s: %w", opr, err)
	}

	url := storagetypes.Url{
		Alias:       alias,
		UserID:      userId,
		ExpiresAt:   expiresAt,
		Navigations: navigations,
	}

	if url.IsExpired() {
		s.DeleteURL(url.Alias)
		return storagetypes.Url{}, fmt.Errorf("%s: %w", opr, storage.ErrorURLExpired)
	}

	return url, nil
}

func (s *Storage) DeleteURL(alias string) error {
	const opr = "storage.storages.sqlite.DeleteURL"

	stmt, err := s.db.Prepare("DELETE FROM Urls WHERE alias = ?")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(alias)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}

func (s *Storage) Login(email string, passHash string) error {
	const opr = "storage.storages.sqlite.Login"
	//TODO
	return nil
}

func (s *Storage) Register(email string, passHash string) error {
	const opr = "storage.storages.sqlite.Register"

	stmt, err := s.db.Prepare("INSERT INTO Users(email,password,createdAt,emailVerified) VALUES(?,?,?,?)")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(email, passHash, time.Now(), false)
	if err != nil {
		if sqliteErr, ok := err.(sqlite3.Error); ok && sqliteErr.ExtendedCode == sqlite3.ErrConstraintUnique {
			return fmt.Errorf("%s: %w", opr, storage.ErrorEmailExists)
		}
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}

func (s *Storage) VerifEmailCreate(email string) error {
	const opr = "storage.storages.sqlite.VerifEmail"

	stmt, err := s.db.Prepare("INSERT INTO EmailVerification VALUES(?,?,?)")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	token, err := emailveriftoken.GenerateRefreshToken()
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	fmt.Println("token:", token)

	_, err = stmt.Exec(email, token, time.Now().Add(time.Hour*1))
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	err = emailverificationsender.SendMessage(email, token)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}

func (s *Storage) VerifEmail(token string) error {
	const opr = "storage.storages.sqlite.VerifEmail"
	stmt, err := s.db.Prepare("SELECT email,expiresAt FROM EmailVerification WHERE token = ?")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	var email string
	var expiresAt time.Time

	err = stmt.QueryRow(token).Scan(&email, &expiresAt)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	if time.Now().Compare(expiresAt) == 1 {
		stmt, err := s.db.Prepare("DELETE FROM EmailVerification WHERE token = ?")
		if err != nil {
			fmt.Println(1)
			return fmt.Errorf("%s: %w", opr, err)
		}

		_, err = stmt.Exec(token)
		if err != nil {
			fmt.Println(2)
			return fmt.Errorf("%s: %w", opr, err)
		}

		return fmt.Errorf("%s: %w", opr, storage.ErrorEmailVerifTokenExpired)
	}

	stmt, err = s.db.Prepare("UPDATE Users SET emailVerified = ? WHERE email = ?")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(true, email)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	stmt, err = s.db.Prepare("DELETE FROM EmailVerification WHERE email = ?")
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	_, err = stmt.Exec(email)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}
