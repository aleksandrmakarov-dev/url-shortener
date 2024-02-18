package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	"strconv"
	resp "url-shortener/internal/lib/api/response"
	dberrs "url-shortener/internal/repository/dbErrs"
	"url-shortener/internal/service"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/render"
)

func (h *Handler) DeleteShortURL() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.DeleteShortURLAlias"
		AuthDataContext := r.Context().Value("Auth")

		if AuthDataContext == nil {
			w.WriteHeader(http.StatusUnauthorized)
			render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Access token required to delete short URL"))
			return
		}

		id, err := strconv.Atoi(chi.URLParam(r, "id"))
		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "id should be an integer"))
			return
		}

		AuthData := AuthDataContext.(service.AccessTokenData)
		err = h.Services.Url.DeleteURLByID(id, AuthData.UserID)
		if err != nil {
			if errors.Is(err, dberrs.ErrorURLNotFound) {
				w.WriteHeader(http.StatusNotFound)
				render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Short URL not found"))
				return
			}
			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal Server Error"))
			h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
		}

		w.WriteHeader(http.StatusOK)

	}
}
