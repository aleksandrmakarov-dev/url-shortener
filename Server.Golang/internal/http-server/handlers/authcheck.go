package handlers

import (
	"context"
	"errors"
	"log/slog"
	"net/http"
	"strings"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/service"

	"github.com/go-chi/render"
)

func (h *Handler) AuthMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {

		authHeader := r.Header.Get("Authorization")
		if authHeader == "" {
			next.ServeHTTP(w, r)
			return
		}

		accessToken, ok := strings.CutPrefix(authHeader, "Bearer ")
		if !ok {
			next.ServeHTTP(w, r)
			return
		}

		AccessTokenData, err := h.Services.Auth.ParseToken(accessToken, h.Cfg.SigningKey)
		if err != nil {
			if errors.Is(err, service.ErrorInvalidSigningMethod) {
				w.WriteHeader(http.StatusUnauthorized)
				render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Invalid token or signing method"))
				return
			}

			if errors.Is(err, service.ErrorTokenFormat) {
				w.WriteHeader(http.StatusUnauthorized)
				render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Invalid token format"))
				return
			}

			w.WriteHeader(http.StatusUnauthorized)
			render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Invalid token"))
			h.Log.Error("Internal", slog.String("err", err.Error()))
			return
		}

		ctx := context.WithValue(r.Context(), "Auth", AccessTokenData)
		r = r.WithContext(ctx)
		next.ServeHTTP(w, r)

	})
}
