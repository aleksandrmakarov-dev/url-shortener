package sqlite

import (
	"database/sql"
	"fmt"

	_ "github.com/mattn/go-sqlite3"
)

var queries = []string{
	`CREATE TABLE IF NOT EXISTS Users (
		id INTEGER PRIMARY KEY,
		email VARCHAR(255) NOT NULL UNIQUE,
		passwordHash VARCHAR(255) NOT NULL,
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

func NewSqliteDB(storagePath string) (*sql.DB, error) {
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

	return db, nil
}
