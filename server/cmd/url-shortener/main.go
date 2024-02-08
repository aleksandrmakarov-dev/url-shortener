package main

import (
	"log/slog"
	"net/http"
	"url-shortener/internal/config"
	"url-shortener/internal/http-server/handlers"
	"url-shortener/internal/lib/logger"
	"url-shortener/internal/storage/storages/sqlite"
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

	//hasher := hashgen.SHA1Hasher{
	//	Salt: cfg.Salt,
	//}

	_ = storage

	handler := handlers.Handler{}
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
