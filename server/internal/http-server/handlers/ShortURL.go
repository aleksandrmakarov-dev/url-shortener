package handlers

import (
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/models"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type shortUrlReq struct {
	Url   string `json:"original" validate:"required,url"`
	Alias string `json:"alias"`
}

func (h *Handler) ShortUrl() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.ShortUrl"

		var req shortUrlReq
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

		url := &models.Url{
			Alias:       req.Alias,
			RedirectUrl: req.Url,
		}

		alias, err := h.Services.Url.CreateShortUrl(url)

	}
}
