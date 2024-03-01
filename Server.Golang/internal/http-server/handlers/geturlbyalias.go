package handlers

import (
	"errors"
	"net/http"
	"time"
	resp "url-shortener/internal/lib/api/response"
	dberrs "url-shortener/internal/repository/dbErrs"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/render"
)

type GetUrlByAliasRes struct {
	ID          int       `json:"id"`
	OriginalUrl string    `json:"original"`
	Alias       string    `json:"alias"`
	Domain      string    `json:"domain"`
	ExpiresAt   time.Time `json:"expiresAt"`
	UserID      int       `json:"userId"`
}

func (h *Handler) GetUrlByAlias() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		alias := chi.URLParam(r, "alias")
		if alias == "" {
			w.WriteHeader(http.StatusNotFound)
			render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Short URL with the provided alias does not exist"))
			return
		}

		url, err := h.Services.GetUrl(alias)
		if err != nil {
			if errors.Is(err, dberrs.ErrorURLExpired) {
				w.WriteHeader(http.StatusNotFound)
				render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Short URL with the provided alias expired"))
				return
			}

			if errors.Is(err, dberrs.ErrorURLNotFound) {
				w.WriteHeader(http.StatusNotFound)
				render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Short URL with the provided alias does not exist"))
				return
			}

			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal server error"))
			return
		}

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, GetUrlByAliasRes{
			ID:          url.ID,
			OriginalUrl: url.RedirectUrl,
			Alias:       url.Alias,
			Domain:      h.Cfg.ReactDomain,
			ExpiresAt:   url.ExpiresAt,
			UserID:      url.UserID,
		})
	}
}
