package models

import "time"

type Url struct {
	ID          int
	Alias       string
	RedirectUrl string
	UserID      int
	ExpiresAt   time.Time
	Navigations int
}

func (u *Url) IsExpired() bool {
	return u.ExpiresAt.Compare(time.Now()) == -1
}
