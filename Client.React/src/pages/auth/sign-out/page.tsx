import { useSignOut } from "@/features/auth/sign-out";
import { Alert, AlertDescription, AlertTitle } from "@/shared/ui/alert";
import { useEffect } from "react";

export default function SignOutPage() {
  const { mutate, isPending, isError, error } = useSignOut();

  useEffect(() => {
    mutate({});
  }, []);

  if (isPending) {
    return <p>Signing out of your account...</p>;
  }

  if (isError) {
    return (
      <Alert>
        <AlertTitle>{error?.response?.data.error}</AlertTitle>
        <AlertDescription>{error?.response?.data.message}</AlertDescription>
      </Alert>
    );
  }

  return (
    <div className="text-center">
      <p className="mb-1.5">You sign out of your account.</p>
      <p className="space-x-5">
        <a
          className="font-medium underline underline-offset-2"
          href="/auth/sign-in"
        >
          Sign in
        </a>
        <a className="font-medium underline underline-offset-2" href="/">
          Home
        </a>
      </p>
    </div>
  );
}
