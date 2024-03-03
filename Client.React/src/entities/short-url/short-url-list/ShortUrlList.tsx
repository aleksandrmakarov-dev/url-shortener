import { ErrorResponse } from "@/lib/dto/common/error.response";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { cn } from "@/lib/utils";
import { FormAlert } from "@/shared/components/FormAlert";
import { FolderSearch, Loader, Loader2, TextSearch } from "lucide-react";
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
    return (
      <div className="text-center flex flex-col items-center py-10">
        <Loader2 className="w-12 h-12 text-muted-foreground animate-spin" />
        <h5 className="text-lg font-medium">Please wait...</h5>
        <p className="text-muted-foreground">We are loading your Short URLs.</p>
      </div>
    );
  }

  if (isError) {
    return <FormAlert isError={isError} error={error} />;
  }

  if (shortUrls?.length == 0) {
    return (
      <div className="text-center flex flex-col items-center py-10">
        <FolderSearch className="w-24 h-24" />
        <h5 className="text-lg font-medium">Couldn't find any Short URLs</h5>
        <p className="text-muted-foreground">
          Shorten your long URL or clear filter to see them here.
        </p>
      </div>
    );
  }

  return (
    <div className={cn("flex flex-col gap-y-3", className)} {...other}>
      {shortUrls?.map((u) => render(u))}
    </div>
  );
}
