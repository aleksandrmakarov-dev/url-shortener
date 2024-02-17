package emailveriftoken

import (
	"crypto/rand"
	"encoding/hex"
	"fmt"
)

func GenerateToken() (string, error) {
	opr := "internal.lib.random.refreshToken.GenerateToken"
	bytes := make([]byte, 128)
	if _, err := rand.Read(bytes); err != nil {
		return "", fmt.Errorf("%s: %w", opr, err)
	}
	return hex.EncodeToString(bytes), nil
}
