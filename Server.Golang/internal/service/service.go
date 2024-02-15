package service

import (
	"time"
	"url-shortener/internal/models"
	"url-shortener/internal/repository"
)

type Auth interface {
	CreateUser(u *models.User) error
	Signin(email, passHash, signingKey string, RefTokenExp time.Duration, AccessTokenExp time.Duration) (models.Session, string, error)
	RefreshToken(token string, signingKey string, AccessTokenExp time.Duration) (int, string, error)
	ParseToken(token string, signingKey string) (AccessTokenData, error)
}

type Url interface {
	CreateShortUrl(u *models.Url) (string, error)
	GetUrl(alias string) (models.Url, error)
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
