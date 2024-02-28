package service

import (
	"time"
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

func (s *UrlService) GetUrlById(id, userId int) (models.Url, error) {
	return s.repo.GetUrlById(id, userId)
}

func (s *UrlService) UpdateURLAliasByID(id int, alias string, userId int) error {
	return s.repo.UpdateUrlAliasByID(id, alias, userId)
}

func (s *UrlService) UpdateURLOriginalByID(id int, original string, userId int) error {
	return s.repo.UpdateUrlOriginalByID(id, original, userId)
}

func (s *UrlService) DeleteURLByID(id, userId int) error {
	return s.repo.DeleteUrlByID(id, userId)
}

func (s *UrlService) SubTimeToUrlByID(id int, t time.Duration, expiresAt time.Time, userId int) error {
	return s.repo.SubTimeToUrlByID(id, t, expiresAt, userId)
}

func (s *UrlService) AddTimeToUrlByID(id int, t time.Duration, expiresAt time.Time, userId int) error {
	return s.repo.AddTimeToUrlByID(id, t, expiresAt, userId)
}
