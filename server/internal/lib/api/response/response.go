package response

import (
	"fmt"
	"net/http"
	"strings"

	"github.com/go-playground/validator/v10"
)

const (
	ErrBadReq   = "Bad Request"
	ErrInternal = "Internal error"
	ErrConflict = "Conflict"
	ErrUnauth   = "Unauthorized"
	ErrNotFound = "Not Found"
)

type Response struct {
	Title   string `json:"title,omitempty"`
	Status  int    `json:"status,omitempty"`
	Error   string `json:"error,omitempty"`
	Message string `json:"message"`
}

func ErrorResp(status int, errTitle string, errMessage string) Response {
	return Response{
		Status:  status,
		Error:   errTitle,
		Message: errMessage,
	}
}

func Resp(title, message string) Response {
	return Response{
		Title:   title,
		Message: message,
	}
}

func ValidationError(errs validator.ValidationErrors) Response {
	var errMsgs []string

	for _, err := range errs {
		switch err.ActualTag() {
		case "required":
			errMsgs = append(errMsgs, fmt.Sprintf("field %s is a required field", err.Field()))
		case "url":
			errMsgs = append(errMsgs, fmt.Sprintf("field %s is not a valid URL", err.Field()))
		case "email":
			errMsgs = append(errMsgs, fmt.Sprintf("field %s is not a valid Email", err.Field()))
		default:
			errMsgs = append(errMsgs, fmt.Sprintf("field %s is not valid", err.Field()))
		}
	}

	return Response{
		Status:  http.StatusBadRequest,
		Error:   ErrBadReq,
		Message: strings.Join(errMsgs, ", "),
	}
}
