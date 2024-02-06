package randomalias

import (
	"crypto/rand"
	"encoding/hex"
)

func GenRandomAlias() (string, error) {
	b := make([]byte, 4)

	if _, err := rand.Read(b); err != nil {
		return "", err
	}

	return hex.EncodeToString(b), nil
}
