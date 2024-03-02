import { useSession } from "@/context/session-provider/SessionProvider";
import { SignInForm } from "@/entities/auth";
import { useSignInLocal } from "@/features/auth/sign-in";
import { SignInRequest } from "@/lib/dto/auth/sign-in.request";
import { CardContainer } from "@/shared/components/CardContainer";
import { FormAlert } from "@/shared/components/FormAlert";
import { Button } from "@/shared/ui/button";
import { useNavigate } from "react-router-dom";

export function SignInCard() {
  const { mutate, isError, error, isSuccess, isPending } = useSignInLocal();
  const { setSession } = useSession();

  const navigate = useNavigate();

  const onSubmit = (data: SignInRequest) => {
    mutate(data, {
      onSuccess: (response) => {
        setSession(response);
        navigate("/", { replace: true });
      },
    });
  };

  return (
    <CardContainer className="w-full max-w-md mx-auto p-8">
      <h1 className="text-center mb-10 text-3xl font-semibold">
        Sign in to Account
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
          title: "Signed in to your account",
          message: "You succesfully signed in to your account.",
        }}
        isError={isError}
        error={{
          title: error?.response?.data.error,
          message: error?.response?.data.message,
        }}
      />
      <SignInForm isLoading={isPending} onSubmit={onSubmit} />
      <div className="text-center mt-5">
        <p>
          Don't have an account?{" "}
          <a
            href="/auth/sign-up"
            className="font-semibold underline underline-offset-2"
          >
            Sign up
          </a>
        </p>
      </div>
    </CardContainer>
  );
}
