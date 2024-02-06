import { SignInForm } from "@/entities/auth";
import { SignInDto } from "@/lib/dto/auth/sign-in.dto";
import { Button } from "@/shared/ui/button";

export function SignInCard() {
  const onSubmit = (data: SignInDto) => {
    console.log(data);
  };

  return (
    <div className="w-full max-w-md">
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
      <SignInForm onSubmit={onSubmit} />
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
    </div>
  );
}
