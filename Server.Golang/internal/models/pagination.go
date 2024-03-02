package models

type Pagination struct {
	Page        int    `json:"page"`
	Size        int    `json:"size"`
	Query       string `json:"query"`
	UserID      int    `json:"userId"`
	HasNextPage bool   `json:"hasNextPage"`
}
