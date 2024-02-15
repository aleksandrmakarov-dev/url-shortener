package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	resp "url-shortener/internal/lib/api/response"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/render"
)

func (h *Handler) Redirect() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.Redirect"
		alias := chi.URLParam(r, "alias")
		if alias == "" {
			w.WriteHeader(http.StatusNotFound)
			render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Url does not exist"))
			return
		}

		url, err := h.Services.Url.GetUrl(alias)
		if err != nil {
			if errors.Is(err, dberrs.ErrorURLNotFound) {
				w.WriteHeader(http.StatusNotFound)
				render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Url does not exist"))
				return
			}

			if errors.Is(err, dberrs.ErrorURLExpired) {
				w.WriteHeader(http.StatusNotFound)
				render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Url expired"))
				return
			}

			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
			h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
			return
		}

		//Логика добавления статистики после перехода todo

		http.Redirect(w, r, url.RedirectUrl, http.StatusFound)
	}
}
