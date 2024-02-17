package models

import "time"

type EmailVerification struct {
	Email     string
	Token     string
	ExpiresAt time.Time
}

func (u *EmailVerification) IsExpired() bool {
	return u.ExpiresAt.Compare(time.Now()) == -1
}
