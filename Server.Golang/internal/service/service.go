package service

import (
	"url-shortener/internal/models"
	"url-shortener/internal/repository"
)

type Auth interface {
	CreateUser(u *models.User) error
	GenToken(email, passHash string) (string, error)
}

type Url interface {
}

type Service struct {
	Auth
	Url
}

func NewService(repos *repository.Repository) *Service {
	return &Service{
		Auth: NewAuthService(repos),
	}
}
