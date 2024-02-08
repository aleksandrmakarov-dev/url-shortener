package handlers

import (
	"log/slog"
	"net/http"
	"url-shortener/internal/lib/api/response"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type EmailVerifCreator interface {
	VerifEmailCreate(email string) error
}

type ResendVerificationReq struct {
	Email string `json:"email" validate:"required,email"`
}

func (h *Handler) ResendVerification(log slog.Logger, emailVerifCreator EmailVerifCreator) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req ResendVerificationReq

		err := render.DecodeJSON(r.Body, &req)
		if err != nil {
			render.JSON(w, r, response.Error("invalid request"))
			return
		}

		if err := validator.New().Struct(req); err != nil {
			valErr := err.(validator.ValidationErrors)
			render.JSON(w, r, response.ValidationError(valErr))

			return
		}

		err = emailVerifCreator.VerifEmailCreate(req.Email)
		if err != nil {
			render.JSON(w, r, response.Error("internal error"))
			return
		}

		render.JSON(w, r, response.OK())

	}
}
