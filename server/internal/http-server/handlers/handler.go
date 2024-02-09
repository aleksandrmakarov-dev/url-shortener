package handlers

import (
	"log/slog"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/render"
)

type Handler struct {
	Log slog.Logger
}

func NewHandler(log slog.Logger) *Handler {
	return &Handler{
		Log: log,
	}
}

func (h *Handler) InitRoutes() *chi.Mux {
	router := chi.NewRouter()

	router.Route("/api/v1", func(r chi.Router) {
		r.Post("/test", func(w http.ResponseWriter, r *http.Request) {
			render.JSON(w, r, "test work!")
		})

		//r.Post("/sign-up", h.Signup())
		//r.Post("/sign-in")

	})

	return router
}
