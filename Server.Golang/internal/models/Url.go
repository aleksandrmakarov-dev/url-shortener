package models

import "time"

type Url struct {
	ID          int
	Alias       string
	RedirectUrl string
	UserID      int
	CreatedAt   time.Time
	ExpiresAt   time.Time
}

func (u *Url) IsExpired() bool {
	return u.ExpiresAt.Compare(time.Now()) == -1
}
