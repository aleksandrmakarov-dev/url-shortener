package models

import "time"

type Sessions struct {
	ID           int
	UserID       int
	RefreshToken string
	ExpiresAt    time.Time
}
