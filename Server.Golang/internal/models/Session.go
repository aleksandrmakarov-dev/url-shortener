package models

import "time"

type Session struct {
	ID           int
	UserID       int
	RefreshToken string
	ExpiresAt    time.Time
}
