import { VerifyEmailForm } from "@/entities/auth";
import { useVerifyEmail } from "@/features/auth/verify-email";
import { VerifyEmailDto } from "@/lib/dto/auth/verify-email.dto";
import { FormAlert } from "@/shared/components/FormAlert";

interface VerifyEmailCardProps {
  data?: VerifyEmailDto;
}

export function VerifyEmailCard({ data }: VerifyEmailCardProps) {
  const { mutate, isError, error, isSuccess, isPending } = useVerifyEmail();

  const onSubmit = (data: VerifyEmailDto) => {
    mutate(data);
  };

  return (
    <div className="w-full max-w-md">
      <h1 className="text-center mb-10 text-3xl font-semibold">
        Verify you account
      </h1>
      <FormAlert
        className="mb-3"
        isSuccess={isSuccess}
        success={{
          title: "Verification sucessful",
          message:
            "Your account has been verified. You can sing in to your account.",
        }}
        isError={isError}
        error={error?.response?.data}
      />
      <VerifyEmailForm data={data} isLoading={isPending} onSubmit={onSubmit} />
    </div>
  );
}
