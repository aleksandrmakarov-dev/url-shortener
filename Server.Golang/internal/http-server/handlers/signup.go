package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	"time"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/lib/hashgen"
	emailverificationsender "url-shortener/internal/lib/messageSender/emailVerificationSender"
	"url-shortener/internal/models"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type signupReq struct {
	Email string `json:"email" validate:"required,email"`
	Pass  string `json:"password" validate:"required"`
}

func (h *Handler) Signup() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.Signup"

		var req signupReq
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

		user := models.User{
			Email:         req.Email,
			Role:          "U",
			PassHash:      hs.GenHash(req.Pass),
			CreatedAt:     time.Now(),
			EmailVerified: false,
		}

		err = h.Services.CreateUser(&user)
		if err != nil {
			if errors.Is(err, dberrs.ErrorEmailExists) {
				w.WriteHeader(http.StatusConflict)
				render.JSON(w, r, resp.ErrorResp(http.StatusConflict, resp.ErrConflict, "User with this email already exists"))
				return
			}

			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
			h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
			return
		}

		if h.Cfg.EmailVerifRequired {

			EmailVerif, err := h.Services.Auth.CreteEmailVerification(user.Email, h.Cfg.EmailVerifTokenTTL)
			if err != nil {
				w.WriteHeader(http.StatusInternalServerError)
				render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
				h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
				return
			}

			err = emailverificationsender.SendMessage(EmailVerif.Email, EmailVerif.Token, h.Cfg.ResendApiKey, h.Cfg.ReactDomain)
			if err != nil {
				w.WriteHeader(http.StatusInternalServerError)
				render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
				h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
				return
			}
		}

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, resp.Resp("Complete the registration", "You will receive verification code to the email address"))
	}
}
