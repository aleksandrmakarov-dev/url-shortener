import { Button } from "../ui/button";

interface ErrorViewBaseProps {
  status?: number;
  statusText?: string;
  message?: string;
}

export function ErrorViewBase({
  status,
  statusText,
  message,
}: ErrorViewBaseProps) {
  return (
    <div className="w-full h-screen flex items-center justify-center">
      <div className="text-center">
        <p className="text-xl font-semibold text-muted-foreground sm:text-3xl">
          {status ?? "500"}
        </p>
        <h1 className="mt-4 text-3xl font-bold tracking-tight text-gray-900 sm:text-5xl">
          {statusText ?? "Internal Server Error"}
        </h1>
        <p className="mt-6 text-base leading-7 text-gray-600">
          {message ?? "Something went wrong while processing your request."}
        </p>
        <div className="mt-5 flex items-center justify-center">
          <Button asChild>
            <a href="/">Go back home</a>
          </Button>
        </div>
      </div>
    </div>
  );
}
