package emailveriftoken

import (
	"crypto/rand"
	"encoding/hex"
	"fmt"
)

func GenerateRefreshToken() (string, error) {
	opr := "internal.lib.random.refreshToken.GenerateRefreshToken"

	bytes := make([]byte, 32)
	if _, err := rand.Read(bytes); err != nil {
		return "", fmt.Errorf("%s: %w", opr, err)
	}
	return hex.EncodeToString(bytes), nil
}
