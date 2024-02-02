package hashgen

import (
	"crypto/sha1"
	"fmt"
)

type SHA1Hasher struct {
	Salt string
}

func New(salt string) *SHA1Hasher {
	return &SHA1Hasher{Salt: salt}
}

func (h SHA1Hasher) GenHash(pass string) string {
	hash := sha1.New()
	hash.Write([]byte(pass))

	return fmt.Sprintf("%x", hash.Sum([]byte(h.Salt)))
}
