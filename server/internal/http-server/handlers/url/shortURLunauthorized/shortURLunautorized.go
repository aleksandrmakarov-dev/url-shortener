package shorturlunauthorized

import (
	"log/slog"
	"net/http"
	"url-shortener/internal/lib/api/response"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type URLSaver interface {
	SaveUrlUnauthorized(URLtoSave string) (string, error)
}

type Request struct {
	Url string `json:"url" validate:"required,url"`
}

type Response struct {
	response.Response
	Alias string
}

func New(log slog.Logger, URLSaver URLSaver) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
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

		alias, err := URLSaver.SaveUrlUnauthorized(req.Url)
		if err != nil {
			render.JSON(w, r, response.Error("internal error"))
			return
		}

		render.JSON(w, r, Response{
			Response: response.OK(),
			Alias:    alias,
		})
	}
}
