package router

import (
	"github.com/go-chi/chi/v5"
)

type Router struct {
	Router *chi.Mux
}

func New() *Router {
	return &Router{
		Router: chi.NewRouter(),
	}
}

func (r Router) InitMiddlewares() {
	r.Router.Use()
}

func (r Router) InitRoutePatterns() {

}
