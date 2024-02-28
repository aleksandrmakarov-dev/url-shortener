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
	"github.com/go-playground/validator/v10"
)

type UpdateShortUrlReq struct {
	Original string `json:"original" validate:"url"`
	Alias    string `json:"alias"`
}

type UpdateShortUrlRes struct {
	Original  string
	Shortened string
	Alias     string
}

func (h *Handler) UpdateShortUrl() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		const opr = "internal.http-server.handlers.UpdateShortUrl"

		AuthDataContex := r.Context().Value("Auth")
		if AuthDataContex == nil {
			w.WriteHeader(http.StatusUnauthorized)
			render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Access token required to update short url"))
		}

		id, err := strconv.Atoi(chi.URLParam(r, "id"))
		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Invalid request"))
			return
		}

		var req UpdateShortUrlReq
		err = render.DecodeJSON(r.Body, &req)
		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Invalid request"))
			return
		}

		if req.Original != "" {
			if err := validator.New().Struct(req); err != nil {
				valErr := err.(validator.ValidationErrors)
				w.WriteHeader(http.StatusBadRequest)
				render.JSON(w, r, resp.ValidationError(valErr))
				return
			}
		}

		if req.Alias == "" && req.Original == "" {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Invalid request"))
			return
		}

		AuthData := AuthDataContex.(service.AccessTokenData)

		if req.Alias != "" {
			err := h.Services.Url.UpdateURLAliasByID(id, req.Alias, AuthData.UserID)
			if err != nil {
				if errors.Is(err, dberrs.ErrorURLNotFound) {
					w.WriteHeader(http.StatusNotFound)
					render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Short URL not found"))
					return
				}
				if errors.Is(err, dberrs.ErrorURLAliasExists) {
					w.WriteHeader(http.StatusBadRequest)
					render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Alias already exists"))
					return
				}
				w.WriteHeader(http.StatusInternalServerError)
				render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal server error"))
				h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
				return
			}
		}

		if req.Original != "" {
			err := h.Services.Url.UpdateURLOriginalByID(id, req.Original, AuthData.UserID)
			if err != nil {
				if errors.Is(err, dberrs.ErrorURLNotFound) {
					w.WriteHeader(http.StatusNotFound)
					render.JSON(w, r, resp.ErrorResp(http.StatusNotFound, resp.ErrNotFound, "Short URL not found"))
					return
				}
				w.WriteHeader(http.StatusInternalServerError)
				render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal server error"))
				h.Log.Error("Internal error", slog.String("opr", opr), slog.String("err", err.Error()))
				return
			}
		}

		url, err := h.Services.Url.GetUrlById(id, AuthData.UserID)
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

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, UpdateShortUrlRes{
			Original:  url.RedirectUrl,
			Shortened: h.Cfg.ServerDomain + "/" + url.Alias,
			Alias:     url.Alias,
		})
	}
}
