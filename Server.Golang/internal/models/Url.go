package models

import "time"

type Url struct {
	ID          int       `json:"id"`
	Alias       string    `json:"alias"`
	RedirectUrl string    `json:"original"`
	Domain      string    `json:"domain"`
	UserID      int       `json:"userId"`
	CreatedAt   time.Time `json:"createdAt"`
	ExpiresAt   time.Time `json:"expiresAt"`
}

func (u *Url) IsExpired() bool {
	return u.ExpiresAt.Compare(time.Now()) == -1
}
