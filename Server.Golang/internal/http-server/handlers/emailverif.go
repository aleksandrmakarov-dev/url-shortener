package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type emailverifReq struct {
	Email string `json:"email" validate:"required,email"`
	Token string `json:"token" validate:"required"`
}

func (h *Handler) EmailVerification() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.EmailVerification"

		var req emailverifReq
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

		err = h.Services.Auth.VerifEmail(req.Email, req.Token)
		if err != nil {
			if errors.Is(err, dberrs.ErrorEmailVerifTokenExpired) {
				w.WriteHeader(http.StatusUnauthorized)
				render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Verification token has expired"))
				return
			}
			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
			h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
			return
		}

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, resp.Resp("Registration is completed", "Your email address is verified. You can sign in to your account"))
	}
}
