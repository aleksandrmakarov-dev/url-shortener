package handlers

import (
	"errors"
	"log/slog"
	"net/http"
	"time"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/models"
	dberrs "url-shortener/internal/repository/dbErrs"
	"url-shortener/internal/service"

	"github.com/go-chi/render"
	"github.com/go-playground/validator/v10"
)

type shortUrlReq struct {
	Url      string `json:"original" validate:"required,url"`
	Alias    string `json:"alias"`
	Lifetime string `json:"lifetime"`
}

type shortUrlRes struct {
	Url      string `json:"original"`
	ShortUrl string `json:"shortened"`
	Alias    string `json:"alias"`
}

func (h *Handler) ShortUrl() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {

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

		lt := h.Cfg.DefaultUrlLifatimeA
		if req.Lifetime != "" {
			lt, err = time.ParseDuration(req.Lifetime)
			if err != nil {
				w.WriteHeader(http.StatusBadRequest)
				render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Invalid request"))
				return
			}
		}

		AuthDataContext := r.Context().Value("Auth")
		if AuthDataContext != nil {

			AuthData := AuthDataContext.(service.AccessTokenData)
			url := models.Url{
				Alias:       req.Alias,
				RedirectUrl: req.Url,
				UserID:      AuthData.UserID,
				CreatedAt:   time.Now(),
				ExpiresAt:   time.Now().Add(lt),
			}
			alias, err := h.Services.Url.CreateShortUrl(&url)
			if err != nil {
				if errors.Is(err, dberrs.ErrorURLAliasExists) {
					w.WriteHeader(http.StatusBadRequest)
					render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Alias already exists"))
					return
				}
				w.WriteHeader(http.StatusInternalServerError)
				render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal server error"))
				h.Log.Error("Internal error", slog.String("err", err.Error()))
				return
			}

			w.WriteHeader(http.StatusOK)
			render.JSON(w, r, shortUrlRes{
				Url:      req.Url,
				ShortUrl: h.Cfg.ServerDomain + "/" + alias,
				Alias:    alias,
			})
			return
		}

		if req.Alias != "" {
			w.WriteHeader(http.StatusUnauthorized)
			render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Access token required to set custom alias"))
			return
		}

		url := models.Url{
			Alias:       "",
			RedirectUrl: req.Url,
			UserID:      0,
			CreatedAt:   time.Now(),
			ExpiresAt:   time.Now().Add(h.Cfg.DefaultUrlLifatimeUA),
		}

		alias, err := h.Services.Url.CreateShortUrl(&url)
		if err != nil {
			w.WriteHeader(http.StatusInternalServerError)
			render.JSON(w, r, resp.ErrorResp(http.StatusInternalServerError, resp.ErrInternal, "Internal server error"))
			h.Log.Error("Internal error", slog.String("err", err.Error()))
			return
		}

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, shortUrlRes{
			Url: req.Url,
			//мб в конфиг придётся добавить
			ShortUrl: `https://` + h.Cfg.Address + `/` + alias,
			Alias:    alias,
		})

	}
}
