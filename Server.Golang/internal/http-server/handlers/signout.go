package handlers

import (
	"net/http"
	"time"
	resp "url-shortener/internal/lib/api/response"

	"github.com/go-chi/render"
)

func (h *Handler) Signout() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {

		refTokenCookie, err := r.Cookie("refreshToken")
		if err != nil {
			w.WriteHeader(http.StatusUnauthorized)
			render.JSON(w, r, resp.ErrorResp(http.StatusUnauthorized, resp.ErrUnauth, "Invalid or expired refresh token"))
			return
		}

		err = h.Services.DeleteRefreshToken(refTokenCookie.Value)
		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			render.JSON(w, r, resp.ErrorResp(http.StatusBadRequest, resp.ErrBadReq, "Invalid or expired token"))
			return
		}

		c, _ := r.Cookie("refreshToken")
		c.Value = ""
		c.Expires = time.Now()
		http.SetCookie(w, c)

		w.WriteHeader(http.StatusNoContent)
	}
}
