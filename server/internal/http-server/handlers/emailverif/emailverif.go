package emailverif

import (
	"errors"
	"log/slog"
	"net/http"
	"url-shortener/internal/lib/api/response"
	"url-shortener/internal/storage"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/render"
)

type EmailVerifer interface {
	VerifEmail(token string) error
}

func New(log slog.Logger, emailVerifer EmailVerifer) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		verifToken := chi.URLParam(r, "veriftoken")

		if verifToken == "" {
			render.JSON(w, r, response.Error("invalid request"))
			return
		}

		err := emailVerifer.VerifEmail(verifToken)
		if err != nil {
			if errors.Is(err, storage.ErrorEmailVerifTokenExpired) {
				render.JSON(w, r, response.Error("verification expired"))
				return
			}
			render.JSON(w, r, response.Error("internal error"))
			log.Error("some err", slog.String("err", err.Error()))
			return
		}

		render.JSON(w, r, response.OK())

	}
}
