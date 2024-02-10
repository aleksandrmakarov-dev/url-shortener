package handlers

import (
	"log/slog"
	"net/http"

	"github.com/go-chi/chi/v5"
)

type EmailVerifer interface {
	VerifEmail(token string) error
}

func (h *Handler) EmailVerification(log slog.Logger, emailVerifer EmailVerifer) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		verifToken := chi.URLParam(r, "veriftoken")

		if verifToken == "" {
			//render.JSON(w, r, resp.Error("invalid request"))
			return
		}

		err := emailVerifer.VerifEmail(verifToken)
		if err != nil {
			//if errors.Is(err, repository.ErrorEmailVerifTokenExpired) {
			//	render.JSON(w, r, resp.Error("verification expired"))
			//	return
			//}
			//render.JSON(w, r, resp.Error("internal error"))
			//log.Error("some err", slog.String("err", err.Error()))
			//return
		}

		//render.JSON(w, r, resp.OK())

	}
}