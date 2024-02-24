import { NewEmailVerificationForm } from "@/entities/auth";
import { useNewEmailVerification } from "@/features/auth/new-email-verification";
import { NewEmailVerificationRequest } from "@/lib/dto/auth/new-email-verification.request";
import { FormAlert } from "@/shared/components/FormAlert";

interface NewEmailVerificationCardProps {
  data?: NewEmailVerificationRequest;
}

export function NewEmailVerificationCard({ data }: NewEmailVerificationCardProps) {
  const { mutate, isError, error, isSuccess, isPending } = useNewEmailVerification();

  const onSubmit = (data: NewEmailVerificationRequest) => {
    mutate(data);
  };

  return (
    <div className="w-full max-w-md">
      <h1 className="text-center mb-10 text-3xl font-semibold">
        Request new verification token
      </h1>
      <FormAlert
        className="mb-3"
        isSuccess={isSuccess}
        success={{
          title: "New email verification tokens",
          message:<>You have been send verification link to your email or enter token manually at 
          <a className="underline underline-offset-2" href="/auth/verify-email">Verify email</a>
          </>
        }}
        isError={isError}
        error={error?.response?.data}
      />
      <NewEmailVerificationForm data={data} isLoading={isPending} onSubmit={onSubmit} />
    </div>
  );
}
