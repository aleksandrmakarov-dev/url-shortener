package service

import (
	"url-shortener/internal/models"
	"url-shortener/internal/repository"
)

type UrlService struct {
	repo repository.Url
}

func NewUrlService(repo repository.Url) *UrlService {
	return &UrlService{
		repo: repo,
	}
}

func (s *UrlService) CreateShortUrl(u *models.Url) (string, error) {
	return s.CreateShortUrl(u)
}