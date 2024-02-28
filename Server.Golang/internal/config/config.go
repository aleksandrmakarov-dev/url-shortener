package config

import (
	"log"
	"os"
	"time"

	"github.com/ilyakaznacheev/cleanenv"
	"github.com/joho/godotenv"
)

type Config struct {
	Env                  string `yaml:"env" env-default:"local"`
	StoragePath          string `yaml:"storage_path" env-required:"true"`
	ServerDomain         string `yaml:"serverDomain" env-required:"true"`
	ReactDomain          string `yaml:"reactDomain" env-required:"true"`
	Salt                 string `yaml:"salt" env-default:"salt"`
	SigningKey           string `yaml:"signingKey" env-default:"signingKey"`
	ResendApiKey         string
	AccsesTokenTokenTTL  time.Duration `yaml:"accsesTokenTokenTTL" env-default:"3600s"`
	RefreshTokenTokenTTL time.Duration `yaml:"refreshTokenTokenTTL" env-default:"720h"`
	EmailVerifTokenTTL   time.Duration `yaml:"emailVerifTokenTTL" env-default:"1h"`
	DefaultUrlLifatimeUA time.Duration `yaml:"defaultUrlLifatimeUA" env-default:"24h"`
	DefaultUrlLifatimeA  time.Duration `yaml:"defaultUrlLifatimeA" env-default:"72h"`
	EmailVerifRequired   bool          `yaml:"emailVerifRequired" env-default:"true"`
	HTTPServer           `yaml:"http_server"`
}

type HTTPServer struct {
	Address     string        `yaml:"address" env-default:"localhost:8080"`
	Timeout     time.Duration `yaml:"timeout" env-default:"5s"`
	IdleTimeout time.Duration `yaml:"idle_timeout" env-default:"60s"`
}

func LoadConfig() *Config {
	opr := "internal.config.LoadConfig"
	godotenv.Load(".env")

	cfgPath := os.Getenv("CONFIG_PATH")
	if cfgPath == "" {
		log.Fatalf("%s: config path is not set", opr)
	}

	if _, err := os.Stat(cfgPath); os.IsNotExist(err) {
		log.Fatalf("%s: config file does not exist", opr)
	}

	var cfg Config

	err := cleanenv.ReadConfig(cfgPath, &cfg)
	if err != nil {
		log.Fatalf("%s: %s", opr, err)
	}

	cfg.ResendApiKey = os.Getenv("API_KEY_RESEND")

	return &cfg
}
