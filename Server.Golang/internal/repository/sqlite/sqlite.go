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
		role VARCHAR(255) NOT NULL,
		passwordHash VARCHAR(255) NOT NULL,
		createdAt DATETIME NOT NULL,
		emailVerified BOOLEAN NOT NULL
	);
	CREATE INDEX IF NOT EXISTS idx_user_id ON Users(id);`,

	`CREATE TABLE IF NOT EXISTS Sessions (
		id INTEGER PRIMARY KEY,
		userId INTEGER REFERENCES Users(id),
		refreshToken VARCHAR(255) NOT NULL UNIQUE,
		expiresAt DATETIME NOT NULL
	);
	CREATE INDEX IF NOT EXISTS idx_session_userId ON Sessions(userId);
	CREATE INDEX IF NOT EXISTS idx_session_refreshToken ON Sessions(refreshToken);`,

	`CREATE TABLE IF NOT EXISTS Urls (
		id INTEGER PRIMARY KEY,
		alias TEXT NOT NULL UNIQUE,
		redirect VARCHAR(255) NOT NULL,
		userId INTEGER REFERENCES Users(id),
		createdAt DATETIME NULL,
		expiresAt DATETIME NULL
	);
	CREATE INDEX IF NOT EXISTS idx_url_userId ON Urls(userId);
	CREATE INDEX IF NOT EXISTS idx_url_alias ON Urls(alias);`,

	`CREATE TABLE IF NOT EXISTS EmailVerification (
		email TEXT NOT NULL,
		token TEXT NOT NULL,
		expiresAt DATETIME NULL
	);
	CREATE INDEX IF NOT EXISTS idx_emailVerification_token ON EmailVerification(token);`,

	`CREATE TABLE IF NOT EXISTS Navigations(
		id INTEGER PRIMARY KEY,
		urlId INTEGER REFERENCES Urls(id),
		country VARCHAR(255) NOT NULL,
		ip VARCHAR(255) NOT NULL,
		Date DATETIME NULL
	);`,
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
