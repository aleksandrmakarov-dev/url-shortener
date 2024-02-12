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
