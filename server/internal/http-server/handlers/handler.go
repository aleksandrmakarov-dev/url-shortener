package handlers

import (
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/render"
)

type Handler struct {
}

func (h *Handler) InitRoutes() *chi.Mux {
	router := chi.NewRouter()

	//router.Route("/", func(r chi.Router) {
	//r.Post("{alias}", redirect.New(*log, storage))

	//r.Route("/email-verification", func(r chi.Router) {
	//r.Get("{veriftoken}", emailverif.New(*log, storage))
	//})
	//})

	router.Route("/auth", func(r chi.Router) {
		//r.Post("/sign-up")
		//r.Post("/sign-in")
	})

	router.Route("/api/v1", func(r chi.Router) {
		r.Post("/test", func(w http.ResponseWriter, r *http.Request) {

			render.JSON(w, r, "test works!")
		})
	})

	return router
}
