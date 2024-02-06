package storagetypes

import "time"

type Url struct {
	ID          int
	Alias       string
	RedirectUrl string
	UserID      int
	ExpiresAt   time.Time
	Navigations int
}

type User struct {
	ID            int
	Email         string
	Pass          string
	CreatedAt     time.Time
	EmailVerified bool
}

type Sessions struct {
	ID           int
	UserID       int
	RefreshToken string
	ExpiresAt    time.Time
}

func (u *Url) IsExpired() bool {
	if time.Now().Compare(u.ExpiresAt) == 1 {
		return true
	}
	return false
}

func (a *Sessions) IsExpired() bool {
	if time.Now().Compare(a.ExpiresAt) == 1 {
		return true
	}
	return false
}
