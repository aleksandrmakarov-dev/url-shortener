import { SignUpForm } from "@/entities/auth";
import { useSignUpLocal } from "@/features/auth/sign-up";
import { SignUpRequest } from "@/lib/dto/auth/sign-up.request";
import { FormAlert } from "@/shared/components/FormAlert";
import { Button } from "@/shared/ui/button";

export function SignUpCard() {
  const { mutate, isError, error, isSuccess, isPending } = useSignUpLocal();

  const onSubmit = (data: SignUpRequest) => {
    mutate(data);
  };

  return (
    <div className="w-full max-w-md">
      <h1 className="text-center mb-10 text-3xl font-semibold">
        Create your account
      </h1>
      <div>
        <Button className="w-full" variant="outline">
          Continue with Google
        </Button>
        <div className="relative text-center my-5">
          <div className="absolute h-[1px] w-full bg-border top-1/2" />
          <span className="relative bg-white px-4">OR</span>
        </div>
      </div>
      <FormAlert
        className="mb-3"
        isSuccess={isSuccess}
        success={{
          title: "User created",
          message: (
            <>
              You have been send verification link to your email. You can enter
              token manually at{" "}
              <a
                className="underline font-semibold underline-offset-2"
                href="/auth/verify-email"
              >
                Verify email
              </a>
              .
            </>
          ),
        }}
        isError={isError}
        error={{
          title: error?.response?.data.error,
          message: error?.response?.data.message,
        }}
      />
      <SignUpForm isLoading={isPending} onSubmit={onSubmit} />
      <div className="text-center mt-5">
        <p>
          Already have an account?{" "}
          <a
            href="/auth/sign-in"
            className="font-semibold underline underline-offset-2"
          >
            Sign in
          </a>
        </p>
      </div>
    </div>
  );
}
