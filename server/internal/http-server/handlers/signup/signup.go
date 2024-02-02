package signup

import (
	"errors"
	"log/slog"
	"net/http"
	"url-shortener/internal/lib/api/response"
	"url-shortener/internal/storage"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type UserSignup interface {
	Register(email string, passHash string) error
	VerifEmailCreate(email string) error
}

type PassHasher interface {
	GenHash(pass string) string
}

type Request struct {
	Email string `json:"email" validate:"required,email"`
	Pass  string `json:"pass"`
}

type Response struct {
	response.Response
	Email              string `json:"email"`
	EmailVerifRequired bool
}

func New(log slog.Logger, userSignup UserSignup, passHasher PassHasher, emailverif bool) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		opr := "handlers.signup.New"

		log.With(slog.String("opr", opr))

		var req Request
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

		err = userSignup.Register(req.Email, passHasher.GenHash(req.Pass))
		if err != nil {
			if errors.Is(err, storage.ErrorEmailExists) {
				render.JSON(w, r, response.Error("email already exists"))
				return
			}

			render.JSON(w, r, response.Error("internal error"))

			log.Error("internal error", slog.String("opr", opr), slog.String("err", err.Error()))

			return
		}

		if emailverif {
			if err != nil {
				render.JSON(w, r, response.Error("internal error"))
				log.Error("internal error", slog.String("opr", opr), slog.String("err", err.Error()))
				return
			}

			err := userSignup.VerifEmailCreate(req.Email)
			if err != nil {
				render.JSON(w, r, response.Error("internal error"))
				log.Error("internal error", slog.String("opr", opr), slog.String("err", err.Error()))
				return
			}

			render.JSON(w, r, Response{
				Response:           response.OK(),
				Email:              req.Email,
				EmailVerifRequired: true,
			})
			return
		}

		render.JSON(w, r, Response{
			Response:           response.OK(),
			Email:              req.Email,
			EmailVerifRequired: false,
		})

	}
}
