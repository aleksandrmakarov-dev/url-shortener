package service

import (
	"time"
	"url-shortener/internal/models"
	"url-shortener/internal/repository"
)

type Auth interface {
	CreateUser(u *models.User) error
	Signin(email, passHash, signingKey string, RefTokenExp time.Duration, AccessTokenExp time.Duration) (models.Session, string, models.User, error)
	RefreshToken(token string, signingKey string, AccessTokenExp time.Duration) (int, string, error)
	ParseToken(token string, signingKey string) (AccessTokenData, error)
	VerifEmail(email string, token string) error
	CreteEmailVerification(email string, EmailVerifTokenTTL time.Duration) (models.EmailVerification, error)
}

type Url interface {
	CreateShortUrl(u *models.Url) (string, error)
	GetUrl(alias string) (models.Url, error)
	GetUrlById(id, userId int) (models.Url, error)
	UpdateURLAliasByID(id int, alias string, userId int) error
	UpdateURLOriginalByID(id int, original string, userId int) error
	DeleteURLByID(id, userId int) error
	SubTimeToUrlByID(id int, t time.Duration, expiresAt time.Time, userId int) error
	AddTimeToUrlByID(id int, t time.Duration, expiresAt time.Time, userId int) error
}

type Service struct {
	Auth
	Url
}

func NewService(repos *repository.Repository) *Service {
	return &Service{
		Auth: NewAuthService(repos),
		Url:  NewUrlService(repos),
	}
}
