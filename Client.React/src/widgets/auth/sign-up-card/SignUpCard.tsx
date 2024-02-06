import { SignUpForm } from "@/entities/auth";
import { SignUpDto } from "@/lib/dto/auth/sign-up.dto";
import { Button } from "@/shared/ui/button";

export function SignUpCard() {
  const onSubmit = (data: SignUpDto) => {
    console.log(data);
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
      <SignUpForm onSubmit={onSubmit} />
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
