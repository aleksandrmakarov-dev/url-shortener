import { VerifyEmailForm } from "@/entities/auth";
import { useVerifyEmail } from "@/features/auth/verify-email";
import { VerifyEmailRequest } from "@/lib/dto/auth/verify-email.request";
import { CardContainer } from "@/shared/components/CardContainer";
import { FormAlert } from "@/shared/components/FormAlert";

interface VerifyEmailCardProps {
  data?: VerifyEmailRequest;
}

export function VerifyEmailCard({ data }: VerifyEmailCardProps) {
  const { mutate, isError, error, isSuccess, isPending } = useVerifyEmail();

  const onSubmit = (data: VerifyEmailRequest) => {
    mutate(data);
  };

  return (
    <CardContainer className="w-full max-w-md p-8">
      <h1 className="text-center mb-10 text-3xl font-semibold">
        Verify your account
      </h1>
      <FormAlert
        className="mb-3"
        isSuccess={isSuccess}
        success={{
          title: "Verification sucessful",
          message: (
            <>
              Your account has been verified. You can sing in to your account{" "}
              <a
                className="underline font-semibold underline-offset-2"
                href="/auth/sign-in"
              >
                Sign in
              </a>
              .
            </>
          ),
        }}
        isError={isError}
        error={error?.response?.data}
      />
      <VerifyEmailForm data={data} isLoading={isPending} onSubmit={onSubmit} />
    </CardContainer>
  );
}
