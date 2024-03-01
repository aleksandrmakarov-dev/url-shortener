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
	ErrorTokenFormat          = errors.New("error token format")
)

type tokenClaims struct {
	jwt.StandardClaims
	UserId   int    `json:"user_id"`
	UserRole string `json:"role"`
}

type AuthService struct {
	repo repository.Auth
}

type AccessTokenData struct {
	UserID   int
	UserRole string
}

func NewAuthService(repo repository.Auth) *AuthService {
	return &AuthService{
		repo: repo,
	}
}

func (s *AuthService) CreateUser(u *models.User) error {
	return s.repo.CreateUser(u)
}

func (s *AuthService) Signin(email, passHash, signingKey string, RefTokenExp time.Duration, AccessTokenExp time.Duration) (models.Session, string, models.User, error) {
	//Get user
	user, err := s.repo.GetUser(email, passHash)
	if err != nil {
		return models.Session{}, "", models.User{}, err
	}

	session, err := s.repo.GenRefreshToken(&user, RefTokenExp)
	if err != nil {
		return models.Session{}, "", models.User{}, err
	}

	//Accesstoken
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, &tokenClaims{
		jwt.StandardClaims{
			ExpiresAt: time.Now().Add(AccessTokenExp).Unix(),
			IssuedAt:  time.Now().Unix(),
		},
		user.ID,
		user.Role,
	})

	ts, err := token.SignedString([]byte(signingKey))
	if err != nil {
		return models.Session{}, "", models.User{}, err
	}

	return session, ts, user, nil
}

func (s *AuthService) RefreshToken(token string, signingKey string, AccessTokenExp time.Duration) (models.User, string, error) {
	user, err := s.repo.CheckRefreshToken(token)
	if err != nil {
		return models.User{}, "", err
	}

	AccessToken := jwt.NewWithClaims(jwt.SigningMethodHS256, &tokenClaims{
		jwt.StandardClaims{
			ExpiresAt: time.Now().Add(AccessTokenExp).Unix(),
			IssuedAt:  time.Now().Unix(),
		},
		user.ID,
		user.Role,
	})

	AccessTokenString, err := AccessToken.SignedString([]byte(signingKey))
	if err != nil {
		return models.User{}, "", err
	}

	return user, AccessTokenString, nil

}

func (s *AuthService) DeleteRefreshToken(token string) error {
	user, err := s.repo.CheckRefreshToken(token)
	if err != nil {
		return err
	}

	err = s.repo.DeleteRefreshToken(token, user.ID)
	if err != nil {
		return err
	}

	return nil
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

	return AccessTokenData{UserID: claims.UserId, UserRole: claims.UserRole}, nil
}

func (s *AuthService) VerifEmail(email string, token string) error {
	EmailVerif, err := s.repo.GetVerifEmail(token, email)
	if err != nil {
		return err
	}

	err = s.repo.VerifEmail(EmailVerif)
	if err != nil {
		return err
	}

	return nil
}

func (s *AuthService) CreteEmailVerification(email string, EmailVerifTokenTTL time.Duration) (models.EmailVerification, error) {
	EmailVerif, err := s.repo.CreateEmailVerification(email, EmailVerifTokenTTL)
	if err != nil {
		return models.EmailVerification{}, err
	}
	return EmailVerif, nil
}
