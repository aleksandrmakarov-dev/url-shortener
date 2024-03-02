package handlers

import (
	"net/http"
	"strconv"
	resp "url-shortener/internal/lib/api/response"
	"url-shortener/internal/models"
	"url-shortener/internal/service"

	"github.com/go-chi/render"
)

type GetAllUrlsByUserIDRes struct {
	Urls []models.Url      `json:"items"`
	Pag  models.Pagination `json:"pagination"`
}

func (h *Handler) GetAllUrlsByUserId() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		page, err := strconv.Atoi(r.URL.Query().Get("page"))
		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "page must be an integer"))
			return
		}
		size, err := strconv.Atoi(r.URL.Query().Get("size"))
		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "size must be an integer"))
			return
		}
		query := r.URL.Query().Get("query")

		AuthDataContext := r.Context().Value("Auth")

		if AuthDataContext == nil {
			w.WriteHeader(http.StatusUnauthorized)
			render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Access token required"))
			return
		}

		AuthData := AuthDataContext.(service.AccessTokenData)
		userId := AuthData.UserID

		urls, pag := h.Services.Url.GetAllUrls(userId, page, size, query, h.Cfg.ReactDomain)

		w.WriteHeader(http.StatusOK)
		render.JSON(w, r, GetAllUrlsByUserIDRes{
			Urls: urls,
			Pag:  pag,
		})

	}
}
