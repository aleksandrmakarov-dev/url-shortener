package main

import (
	"url-shortener/internal/config"
	"url-shortener/internal/lib/logger"
)

func main() {

	cfg := config.LoadConfig()

	log := logger.StartLogger(cfg.Env)
	log.Info("logger started")
}
