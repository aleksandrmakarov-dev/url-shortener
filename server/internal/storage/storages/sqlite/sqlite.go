package sqlite

import (
	"database/sql"
	"fmt"

	_ "github.com/mattn/go-sqlite3"
)

type Storage struct {
	db *sql.DB
}

var queries = []string{
	`CREATE TABLE IF NOT EXISTS Users (
		id VARCHAR(255) PRIMARY KEY,
		email VARCHAR(255) NOT NULL UNIQUE,
		password VARCHAR(255) NOT NULL,
		createdAt DATETIME NOT NULL,
		emailVerifiedAt DATETIME NULL
	);
	CREATE INDEX IF NOT EXISTS idx_user_id ON User(id);`,

	`CREATE TABLE IF NOT EXISTS Accounts (
		id VARCHAR(255) PRIMARY KEY,
		userId VARCHAR(255) REFERENCES User(id),
		refreshToken VARCHAR(255) NOT NULL UNIQUE,
		createdAt DATETIME NOT NULL,
		expiresAt DATETIME NOT NULL
	);
	CREATE INDEX IF NOT EXISTS idx_account_userId ON Account(userId);
	CREATE INDEX IF NOT EXISTS idx_account_refreshToken ON Account(refreshToken);`,

	`CREATE TABLE IF NOT EXISTS Urls (
		id VARCHAR(255) PRIMARY KEY,
		alias TEXT NOT NULL UNIQUE,
		redirect VARCHAR(255) NOT NULL,
		userId VARCHAR(255) REFERENCES User(id),
		createdAt DATETIME NOT NULL,
		expiresAt DATETIME NULL,
		navigations INT NOT NULL
	);
	CREATE INDEX IF NOT EXISTS idx_url_userId ON Url(userId);
	CREATE INDEX IF NOT EXISTS idx_url_alias ON Url(alias);`,
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
