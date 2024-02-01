package main

import (
	"log/slog"
	"url-shortener/internal/config"
	"url-shortener/internal/lib/logger"
	"url-shortener/internal/storage/storages/sqlite"
	
)

func main() {

	cfg := config.LoadConfig()

	log := logger.StartLogger(cfg.Env)
	log.Info("logger started")

	storage, err := sqlite.New(cfg.StoragePath)
	if err != nil {
		log.Error("filed to init storage", slog.Any("error", err))
	}

	_ = storage

}
