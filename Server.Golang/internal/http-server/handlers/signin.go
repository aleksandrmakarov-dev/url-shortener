package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/lib/hashgen"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type signinReq struct {
	Email string `json:"email" validate:"required,email"`
	Pass  string `json:"password" validate:"required"`
}

type signinRes struct {
	AccsesToken string `json:"accessToken"`
	UserID      int    `json:"userId"`
	Email       string `json:"email"`
	Role        string `json:"role"`
}

func (h *Handler) Singin() http.HandlerFunc {
	const opr = "internal.http-server.handlers.Singin"
	return func(w http.ResponseWriter, r *http.Request) {
		var req signinReq
		err := render.DecodeJSON(r.Body, &req)
		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Invalid request"))
			return
		}

		if err := validator.New().Struct(req); err != nil {
			valErr := err.(validator.ValidationErrors)
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ValidationError(valErr))
			return
		}

		hs := hashgen.New(h.Cfg.Salt)

		session, accToken, user, err := h.Services.Auth.Signin(
			req.Email,
			hs.GenHash(req.Pass),
			h.Cfg.SigningKey,
			h.Cfg.RefreshTokenTokenTTL,
			h.Cfg.AccsesTokenTokenTTL,
		)

		if err != nil {
			if errors.Is(err, dberrs.ErrorInvalidCredentials) {
				w.WriteHeader(http.StatusUnauthorized)
				render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Invalid credentials"))
				return
			}
			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
			h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
			return
		}

		if h.Cfg.EmailVerifRequired {
			if !user.EmailVerified {
				w.WriteHeader(http.StatusUnauthorized)
				render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Email not verified"))
				return
			}
		}

		http.SetCookie(w, &http.Cookie{
			Name:     "refreshToken",
			Value:    session.RefreshToken,
			HttpOnly: true,
			Expires:  session.ExpiresAt,
		})

		render.JSON(w, r, signinRes{
			AccsesToken: accToken,
			UserID:      session.UserID,
			Email:       req.Email,
			Role:        "unknown",
		})

	}
}
