package main

import (
	"log/slog"
	"net/http"
	"url-shortener/internal/config"
	"url-shortener/internal/http-server/handlers"
	"url-shortener/internal/lib/logger"
	"url-shortener/internal/repository"
	"url-shortener/internal/repository/sqlite"
	"url-shortener/internal/service"
)

func main() {

	cfg := config.LoadConfig()

	log := logger.StartLogger(cfg.Env)
	log.Info("Logger is started")

	db, err := sqlite.NewSqliteDB(cfg.StoragePath)
	if err != nil {
		log.Error("filed to init storage", slog.String("error", err.Error()))
	}
	log.Info("Storage is initialized")

	repos := repository.NewRepository(db)
	repos.Url.GetAllUrls(0, 1, 10, "")
	services := service.NewService(repos)
	handler := handlers.NewHandler(*log, services, *cfg)
	router := handler.InitRoutes()

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
