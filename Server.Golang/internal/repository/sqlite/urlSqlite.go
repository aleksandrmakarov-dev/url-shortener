package sqlite

import (
	"database/sql"
	"fmt"
	randomalias "url-shortener/internal/lib/random/randomAlias"
	"url-shortener/internal/models"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/mattn/go-sqlite3"
)

type UrlSqlite struct {
	db *sql.DB
}

func NewUrlSqlite(db *sql.DB) *UrlSqlite {
	return &UrlSqlite{
		db: db,
	}
}

func (r *UrlSqlite) CreateShortUrl(u *models.Url) (string, error) {
	const opr = "repository.sqlite.authSqlite.CreateShortUrl"

	stmt, err := r.db.Prepare("INSERT INTO Urls(alias,redirect,userId,expiresAt,navigations) VALUES(?,?,?,?,?)")
	if err != nil {
		return "", fmt.Errorf("%s: %w", opr, err)
	}

	if u.Alias == "" {
		for {

			alias, err := randomalias.GenRandomAlias()
			if err != nil {
				return "", fmt.Errorf("%s: %w", opr, err)
			}

			existingAlias, err := r.checkAliasExists(alias)
			if err != nil {
				return "", fmt.Errorf("%s: %w", opr, err)
			}

			if !existingAlias {
				u.Alias = alias
				_, err = stmt.Exec(u.Alias, u.RedirectUrl, u.UserID, u.ExpiresAt, u.Navigations)
				if err != nil {
					return "", fmt.Errorf("%s: %w", opr, err)
				}
				return alias, nil
			}
		}
	}

	_, err = stmt.Exec(u.Alias, u.RedirectUrl, u.UserID, u.ExpiresAt, u.Navigations)
	if err != nil {
		if sqlError, ok := err.(sqlite3.Error); ok && sqlError.ExtendedCode == sqlite3.ErrConstraintUnique {
			return "", fmt.Errorf("%s: %w", opr, dberrs.ErrorURLAliasExists)
		}

		return "", fmt.Errorf("%s: %w", opr, err)
	}

	return u.Alias, nil
}

func (r *UrlSqlite) checkAliasExists(alias string) (bool, error) {
	const opr = "storage.storages.sqlite.checkAliasExists"

	stmt, err := r.db.Prepare("SELECT COUNT(*) FROM Urls WHERE alias = ?")
	if err != nil {
		return false, fmt.Errorf("%s: %w", opr, err)
	}

	var count int
	err = stmt.QueryRow(alias).Scan(&count)
	if err != nil {
		return false, fmt.Errorf("%s: %w", opr, err)
	}

	return count > 0, nil
}
