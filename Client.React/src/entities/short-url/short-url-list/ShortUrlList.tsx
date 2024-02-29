import { ErrorResponse } from "@/lib/dto/common/error.response";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { cn } from "@/lib/utils";
import { FormAlert } from "@/shared/components/FormAlert";
import React, { HTMLAttributes } from "react";

interface ShortUrlListProps extends HTMLAttributes<HTMLDivElement> {
  shortUrls?: ShortUrlResponse[];
  render: (item: ShortUrlResponse) => React.ReactNode;
  isLoading?: boolean;
  isError?: boolean;
  error?: ErrorResponse;
}

export function ShortUrlList({
  shortUrls,
  render,
  isLoading,
  isError,
  error,
  className,
  ...other
}: ShortUrlListProps) {
  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (isError) {
    return <FormAlert isError={isError} error={error} />;
  }

  return (
    <div className={cn("flex flex-col gap-y-3", className)} {...other}>
      {shortUrls?.map((u) => render(u))}
    </div>
  );
}
