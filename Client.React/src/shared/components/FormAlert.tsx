import { cn } from "@/lib/utils";
import { Alert, AlertDescription, AlertTitle } from "@/shared/ui/alert";
import { HTMLAttributes } from "react";

type AlertData = {
  title?: string;
  message?: React.ReactNode;
};

interface FormAlertProps extends HTMLAttributes<HTMLDivElement> {
  isSuccess?: boolean;
  success?: AlertData;
  isError?: boolean;
  error?: AlertData;
}

export function FormAlert({
  isSuccess,
  success,
  isError,
  error,
  className,
  ...other
}: FormAlertProps) {
  if (isError) {
    return (
      <Alert
        className={cn(
          "bg-red-50 border border-red-300 text-red-600 overflow-auto max-h-24",
          className
        )}
        {...other}
      >
        <AlertTitle>{error?.title || "Unexpected error"}</AlertTitle>
        <AlertDescription>
          {error?.message ||
            "Something went wrong while processing your request."}
        </AlertDescription>
      </Alert>
    );
  }

  if (isSuccess) {
    return (
      <Alert
        className={cn(
          "bg-green-50 border border-green-300 text-green-600",
          className
        )}
        {...other}
      >
        <AlertTitle>{success?.title || "Success"}</AlertTitle>
        <AlertDescription>
          {success?.message || "Operation successfully completed."}
        </AlertDescription>
      </Alert>
    );
  }

  return null;
}
