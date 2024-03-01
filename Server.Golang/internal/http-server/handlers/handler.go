package handlers

import (
	"log/slog"
	"url-shortener/internal/config"
	"url-shortener/internal/service"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
)

type Handler struct {
	Log      slog.Logger
	Cfg      config.Config
	Services *service.Service
}

func NewHandler(log slog.Logger, services *service.Service, cfg config.Config) *Handler {
	return &Handler{
		Log:      log,
		Services: services,
		Cfg:      cfg,
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

		r.Route("/auth", func(r chi.Router) {
			r.Post("/sign-up", h.Signup())
			r.Post("/sign-in", h.Singin())
			r.Delete("/sign-out", h.Signout())
			r.Post("/refresh-token", h.RefreshToken())
			r.Post("/resend-verification", h.ResendVerification())
			r.Post("/verify-email", h.EmailVerification())
		})

		r.Get("/short-urls/{alias}", h.GetUrlByAlias())
		r.With(h.AuthMiddleware).Post("/short-urls", h.ShortUrl())
		r.With(h.AuthMiddleware).Delete("/short-urls/{id}", h.DeleteShortURL())
		r.With(h.AuthMiddleware).Put("/short-urls/{id}", h.UpdateShortUrl())

	})

	router.Get("/{alias}", h.Redirect())

	return router
}
