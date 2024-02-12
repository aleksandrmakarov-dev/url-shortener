package models

import "time"

type User struct {
	ID            int
	Email         string
	PassHash      string
	CreatedAt     time.Time
	EmailVerified bool
}
