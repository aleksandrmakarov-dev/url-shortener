package service

import (
	"errors"
	"time"
	"url-shortener/internal/models"
	"url-shortener/internal/repository"

	"github.com/dgrijalva/jwt-go"
)

var (
	ErrorInvalidSigningMethod = errors.New("invalid signing method")
	ErrorTokenFormat          = errors.New("Error token format")
)

type tokenClaims struct {
	jwt.StandardClaims
	UserId int `json:"user_id"`
}

type AuthService struct {
	repo repository.Auth
}

type AccessTokenData struct {
	UserID int
}

func NewAuthService(repo repository.Auth) *AuthService {
	return &AuthService{
		repo: repo,
	}
}

func (s *AuthService) CreateUser(u *models.User) error {
	return s.repo.CreateUser(u)
}

func (s *AuthService) Signin(email, passHash, signingKey string, RefTokenExp time.Duration, AccessTokenExp time.Duration) (models.Session, string, error) {
	//Get user
	user, err := s.repo.GetUser(email, passHash)
	if err != nil {
		return models.Session{}, "", err
	}

	session, err := s.repo.GenRefreshToken(&user, RefTokenExp)
	if err != nil {
		return models.Session{}, "", err
	}

	//Accesstoken
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, &tokenClaims{
		jwt.StandardClaims{
			ExpiresAt: time.Now().Add(AccessTokenExp).Unix(),
			IssuedAt:  time.Now().Unix(),
		},
		user.ID,
	})

	ts, err := token.SignedString([]byte(signingKey))
	if err != nil {
		return models.Session{}, "", err
	}

	return session, ts, nil
}

func (s *AuthService) RefreshToken(token string, signingKey string, AccessTokenExp time.Duration) (int, string, error) {
	userId, err := s.repo.CheckRefreshToken(token)
	if err != nil {
		return 0, "", err
	}

	AccessToken := jwt.NewWithClaims(jwt.SigningMethodHS256, &tokenClaims{
		jwt.StandardClaims{
			ExpiresAt: time.Now().Add(AccessTokenExp).Unix(),
			IssuedAt:  time.Now().Unix(),
		},
		userId,
	})

	AccessTokenString, err := AccessToken.SignedString([]byte(signingKey))
	if err != nil {
		return 0, "", err
	}

	return userId, AccessTokenString, nil

}

func (s *AuthService) ParseToken(token string, signingKey string) (AccessTokenData, error) {
	t, err := jwt.ParseWithClaims(token, &tokenClaims{}, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, ErrorInvalidSigningMethod
		}

		return []byte(signingKey), nil
	})

	if err != nil {
		return AccessTokenData{}, err
	}

	claims, ok := t.Claims.(*tokenClaims)
	if !ok {
		return AccessTokenData{}, ErrorTokenFormat
	}

	return AccessTokenData{UserID: claims.UserId}, nil
}
