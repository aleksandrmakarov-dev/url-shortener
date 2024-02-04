package main

import (
	"log/slog"
	"net/http"
	"url-shortener/internal/config"
	"url-shortener/internal/http-server/handlers/emailverif"
	resendverification "url-shortener/internal/http-server/handlers/resendVerification"
	"url-shortener/internal/http-server/handlers/signup"
	"url-shortener/internal/lib/hashgen"
	"url-shortener/internal/lib/logger"
	"url-shortener/internal/storage/storages/sqlite"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
)

func main() {

	cfg := config.LoadConfig()

	log := logger.StartLogger(cfg.Env)
	log.Info("Logger is started")

	storage, err := sqlite.New(cfg.StoragePath)
	if err != nil {
		log.Error("filed to init storage", slog.String("error", err.Error()))

	}
	log.Info("Storage is initialized")

	hasher := hashgen.SHA1Hasher{
		Salt: cfg.Salt,
	}

	_ = storage

	router := chi.NewRouter()

	router.Use(middleware.RequestID)
	router.Use(middleware.RealIP)
	router.Use(middleware.Logger)
	router.Use(middleware.Recoverer)
	router.Use(middleware.URLFormat)

	router.Post("/signup", signup.New(*log, storage, hasher, true))
	router.Get("/email-verification/{veriftoken}", emailverif.New(*log, storage))
	router.Post("/email-verification/resend-verification", resendverification.New(*log, storage))

	log.Info("Starting server", slog.String("addr", cfg.Address))
	srv := &http.Server{
		Addr:         cfg.Address,
		Handler:      router,
		ReadTimeout:  cfg.Timeout,
		WriteTimeout: cfg.Timeout,
		IdleTimeout:  cfg.IdleTimeout,
	}

	if err := srv.ListenAndServe(); err != nil {
		log.Error("Failed to start server")
	}

	log.Info("server stopped")

}
