package test

import (
	"net/http"

	"github.com/go-chi/render"
)

type test struct {
	Response string
}

func TestMiddleware() http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		render.JSON(w, r, test{Response: "test"})
	})
}
