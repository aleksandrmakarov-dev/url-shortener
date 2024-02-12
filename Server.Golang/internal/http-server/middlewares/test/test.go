package test

import (
	"context"
	"net/http"
)

type test struct {
	Response string
}

func TestMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		ctx := context.WithValue(r.Context(), "auth", true)
		r = r.WithContext(ctx)
		next.ServeHTTP(w, r)
	})
}
