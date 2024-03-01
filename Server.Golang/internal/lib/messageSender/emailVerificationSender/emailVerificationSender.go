package emailverificationsender

import (
	"fmt"

	"github.com/resend/resend-go/v2"
)

func SendMessage(email string, token string, apiKey string, domain string) error {
	const opr = "internal.lib.messageSender.emailVerificationSender.SendMessage"

	client := resend.NewClient(apiKey)
	params := &resend.SendEmailRequest{
		To:      []string{email},
		From:    "noreply@aleksandrmakarov.com",
		Subject: "Email Verification",
		Html: `<!DOCTYPE html>
		<html lang="en">
		<head>
			<meta charset="UTF-8">
			<meta http-equiv="X-UA-Compatible" content="IE=edge">
			<meta name="viewport" content="width=device-width, initial-scale=1.0">
			<title>Email Verification</title>
			<style>
				body {
					font-family: Arial, sans-serif;
					background-color: #625ca8;
					margin: 0;
					padding: 0;
					display: flex;
					align-items: center;
					justify-content: center;
					height: 100vh;
				}
				.container {
					background-color: #fff;
					padding: 20px;
					border-radius: 8px;
					box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
					max-width: 400px;
					width: 100%;
					text-align: center;
				}
				h1 {
					color: #333;
				}
				p {
					color: #666;
				}
				.verification-link {
					display: inline-block;
					padding: 10px 20px;
					background-color: #4caf50;
					color: #fff;
					text-decoration: none;
					border-radius: 4px;
					margin-top: 20px;
				}
			</style>
		</head>
		<body>
			<div class="container">
				<h1>Email Verification</h1>
				<p>To complete your registration, please confirm your email address.</p>
				<a href="` + domain + `/auth/verify-email?email=` + email + `&token=` + token + `" class="verification-link">Confirm Email</a>
			</div>
		</body>
		</html>
		`,
	}

	_, err := client.Emails.Send(params)
	if err != nil {
		return fmt.Errorf("%s: %w", opr, err)
	}

	return nil
}
