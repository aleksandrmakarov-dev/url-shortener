package handlers

import (
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	emailverificationsender "url-shortener/internal/lib/messageSender/emailVerificationSender"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type resendVerificationReq struct {
	Email string `json:"email" validate:"required,email"`
}

func (h *Handler) ResendVerification() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.ResendVerification"

		var req resendVerificationReq

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

		EmailVerif, err := h.Services.Auth.CreteEmailVerification(req.Email, h.Cfg.EmailVerifTokenTTL)
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

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, resp.Resp("New email verification", "New email verification token is sent to your email address"))
	}
}
