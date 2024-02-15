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
	if u.ExpiresAt.Compare(time.Now()) == -1 {
		return true
	}
	return false
}
