package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/models"
	"url-shortener/internal/repository"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/render"
)

type URLGetter interface {
	GetURL(alias string) (models.Url, error)
}

func Redirect(log slog.Logger, URLGetter URLGetter) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		alias := chi.URLParam(r, "alias")
		if alias == "" {
			render.JSON(w, r, resp.Error("not found"))
			return
		}

		url, err := URLGetter.GetURL(alias)
		if err != nil {
			if errors.Is(err, repository.ErrorURLNotFound) {
				render.JSON(w, r, resp.Error("url does not exist"))
				return
			}
			if errors.Is(err, repository.ErrorURLExpired) {
				render.JSON(w, r, resp.Error("url expired"))
				return
			}
			log.Error("internal err", slog.String("err", err.Error()))
			render.JSON(w, r, resp.Error("internal error"))
		}

		log.Info("test", slog.String("url", url.RedirectUrl))
		http.Redirect(w, r, url.RedirectUrl, http.StatusFound)

	}
}
