package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/repository"

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

type signupReq struct {
	Email string `json:"email" validate:"required,email"`
	Pass  string `json:"pass"`
}

type signupRes struct {
	resp.Response
	Email              string `json:"email"`
	EmailVerifRequired bool
}

func (h *Handler) Signup(log slog.Logger, userSignup UserSignup, passHasher PassHasher, emailverif bool) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		opr := "handlers.signup.New"

		log.With(slog.String("opr", opr))

		var req signupReq
		err := render.DecodeJSON(r.Body, &req)
		if err != nil {
			render.JSON(w, r, resp.Error("invalid request"))
			return
		}
		if err := validator.New().Struct(req); err != nil {
			valErr := err.(validator.ValidationErrors)
			render.JSON(w, r, resp.ValidationError(valErr))

			return
		}

		err = userSignup.Register(req.Email, passHasher.GenHash(req.Pass))
		if err != nil {
			if errors.Is(err, repository.ErrorEmailExists) {
				render.JSON(w, r, resp.Error("email already exists"))
				return
			}

			render.JSON(w, r, resp.Error("internal error"))

			log.Error("internal error", slog.String("opr", opr), slog.String("err", err.Error()))

			return
		}

		if emailverif {
			err := userSignup.VerifEmailCreate(req.Email)
			if err != nil {
				render.JSON(w, r, resp.Error("internal error"))
				log.Error("internal error", slog.String("opr", opr), slog.String("err", err.Error()))
				return
			}

			render.JSON(w, r, signupRes{
				Response:           resp.OK(),
				Email:              req.Email,
				EmailVerifRequired: true,
			})
			return
		}

		render.JSON(w, r, signupRes{
			Response:           resp.OK(),
			Email:              req.Email,
			EmailVerifRequired: false,
		})

	}
}
