package storagetypes

import "time"

type Url struct {
	ID          int
	Alias       string
	RedirectUrl string
	UserID      int
	CreatedAt   time.Time
	ExpiresAt   time.Time
	Navigations int
}

type User struct {
	ID              int
	Email           string
	Pass            string
	CreatedAt       time.Time
	EmailVerifiedAt time.Time
}

type Account struct {
	ID           int
	UserID       int
	RefreshToken string
	CreatedAt    time.Time
	ExpiresAt    time.Time
}

func (u *Url) IsExpired() bool {
	if u.CreatedAt.Compare(u.ExpiresAt) == -1 {
		return true
	}
	return false
}

func (a *Account) IsExpired() bool {
	if a.CreatedAt.Compare(a.ExpiresAt) == -1 {
		return true
	}
	return false
}
