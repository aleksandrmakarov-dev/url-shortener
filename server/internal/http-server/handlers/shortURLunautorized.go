package handlers

import (
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type URLSaver interface {
	SaveUrlUnauthorized(URLtoSave string) (string, error)
}

type shortUrlReq struct {
	Url   string `json:"url" validate:"required,url"`
	Alias string `json:"alias"`
}

type shortUrlRes struct {
	resp.Response
	Alias string
}

func (h *Handler) ShortUrl(log slog.Logger, URLSaver URLSaver) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req shortUrlReq
		err := render.DecodeJSON(r.Body, &req)
		if err != nil {
			//render.JSON(w, r, resp.Error("invalid request"))
			return
		}

		if err := validator.New().Struct(req); err != nil {
			valErr := err.(validator.ValidationErrors)
			render.JSON(w, r, resp.ValidationError(valErr))
			return
		}

		alias, err := URLSaver.SaveUrlUnauthorized(req.Url)
		if err != nil {
			///render.JSON(w, r, resp.Error("internal error"))
			return
		}

		render.JSON(w, r, shortUrlRes{
			//Response: resp.OK(),
			Alias: alias,
		})
	}
}
