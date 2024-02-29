package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/go-chi/render"
)

type refreshtokenRes struct {
	AccessToken string `json:"accessToken"`
	UserID      int    `json:"userId"`
	Email       string `json:"email"`
	Role        string `json:"role"`
}

func (h *Handler) RefreshToken() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.RefreshToken"
		refTokenCookie, err := r.Cookie("refreshToken")
		if err != nil {
			w.WriteHeader(http.StatusUnauthorized)
			render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Invalid or expired refresh token"))
			return
		}

		user, accessToken, err := h.Services.Auth.RefreshToken(refTokenCookie.Value, h.Cfg.SigningKey, h.Cfg.AccsesTokenTokenTTL)
		if err != nil {
			if errors.Is(err, dberrs.ErrorInvalidOrExpToken) {
				w.WriteHeader(http.StatusUnauthorized)
				render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Invalid or expired refresh token"))
				return
			}

			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
			h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
			return
		}

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, refreshtokenRes{
			AccessToken: accessToken,
			UserID:      user.ID,
			Email:       user.Email,
			Role:        user.Role,
		})
	}
}
