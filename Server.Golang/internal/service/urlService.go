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
	return s.repo.CreateShortUrl(u)
}

func (s *UrlService) GetUrl(alias string) (models.Url, error) {
	return s.repo.GetUrl(alias)
}

func (s *UrlService) UpdateURLAliasByID(id int, alias string) error {
	return s.repo.UpdateUrlAliasByID(id, alias)
}

func (s *UrlService) UpdateURLOriginalByID(id int, original string) error {
	return s.repo.UpdateUrlOriginalByID(id, original)
}

func (s *UrlService) DeleteURLByID(id, userId int) error {
	return s.repo.DeleteUrlByID(id, userId)
}
