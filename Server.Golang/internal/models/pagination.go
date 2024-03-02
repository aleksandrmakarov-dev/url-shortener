package models

type Pagination struct {
	Page            int  `json:"page"`
	Size            int  `json:"size"`
	HasNextPage     bool `json:"hasNextPage"`
	HasPreviousPage bool `json:"hasPreviousPage"`
}
