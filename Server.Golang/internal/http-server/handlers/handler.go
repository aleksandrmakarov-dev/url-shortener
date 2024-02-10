package handlers

import (
	"log/slog"
	"url-shortener/internal/service"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
)

type Handler struct {
	Log      slog.Logger
	Services *service.Service
}

func NewHandler(log slog.Logger, services *service.Service) *Handler {
	return &Handler{
		Log:      log,
		Services: services,
	}
}

func (h *Handler) InitRoutes() *chi.Mux {
	router := chi.NewRouter()

	router.Use(middleware.RequestID)
	router.Use(middleware.RealIP)
	router.Use(middleware.Logger)
	router.Use(middleware.Recoverer)
	router.Use(middleware.URLFormat)

	router.Route("/api/v1", func(r chi.Router) {
		r.Post("/sign-up", h.Signup())
		//r.Post("/sign-in")
	})

	return router
}