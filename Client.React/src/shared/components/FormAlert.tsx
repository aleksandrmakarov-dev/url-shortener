import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { MessageResponseDto } from "@/lib/dto/common/message-response.dto";
import { cn } from "@/lib/utils";
import { Alert, AlertDescription, AlertTitle } from "@/shared/ui/alert";
import { HTMLAttributes } from "react";

interface FormAlertProps extends HTMLAttributes<HTMLDivElement> {
  isSuccess?: boolean;
  success?: MessageResponseDto;
  isError?: boolean;
  error?: ErrorResponseDto;
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
          "bg-red-50 border border-red-300 text-red-600",
          className
        )}
        {...other}
      >
        <AlertTitle>{error?.error || "Unexpected error"}</AlertTitle>
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
