package repository

import (
	"database/sql"
	"time"

	"url-shortener/internal/models"
	"url-shortener/internal/repository/sqlite"
)

type Auth interface {
	CreateUser(u *models.User) error
	GetUser(email, passHash string) (models.User, error)
	GenRefreshToken(u *models.User, tokenExp time.Duration) (models.Session, error)
	CheckRefreshToken(token string) (models.User, error)
	DeleteRefreshToken(token string, userID int) error
	VerifEmail(EmailVerif models.EmailVerification) error
	GetVerifEmail(token, email string) (models.EmailVerification, error)
	DeleteEmailVerification(token string) error
	CreateEmailVerification(email string, EmailVerifTokenTTL time.Duration) (models.EmailVerification, error)
}

type Url interface {
	CreateShortUrl(u *models.Url) (string, error)
	GetUrl(alias string) (models.Url, error)
	GetUrlById(id, userId int) (models.Url, error)
	GetAllUrls(userId, page, size int, query string) ([]models.Url, models.Pagination, error)
	UpdateUrlOriginalByID(id int, original string, userId int) error
	UpdateUrlAliasByID(id int, alias string, userId int) error
	DeleteUrlByID(id, userId int) error
	SubTimeToUrlByID(id int, t time.Duration, expiresAt time.Time, userId int) error
	AddTimeToUrlByID(id int, t time.Duration, expiresAt time.Time, userId int) error
}

type Repository struct {
	Auth
	Url
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{
		Auth: sqlite.NewAuthSqlite(db),
		Url:  sqlite.NewUrlSqlite(db),
	}
}
