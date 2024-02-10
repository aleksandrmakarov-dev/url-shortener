package service

import (
	"url-shortener/internal/models"
	"url-shortener/internal/repository"
)

type AuthService struct {
	repo repository.Auth
}

func NewAuthService(repo repository.Auth) *AuthService {
	return &AuthService{
		repo: repo,
	}
}

func (s *AuthService) CreateUser(u *models.User) error {
	return s.repo.CreateUser(u)
}

func (s *AuthService) GenToken(email, passHash string) (string, error) {
	//TODO
	return "", nil
}
