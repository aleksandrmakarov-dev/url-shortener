package sqlite

import "database/sql"

type UrlSqlite struct {
	db *sql.DB
}

func NewUrlSqlite(db *sql.DB) *UrlSqlite {
	return &UrlSqlite{
		db: db,
	}
}
