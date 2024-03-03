import { ErrorResponse } from "@/lib/dto/common/error.response";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { cn } from "@/lib/utils";
import { EmptyViewBase } from "@/shared/components/EmptyViewBase";
import { FormAlert } from "@/shared/components/FormAlert";
import { LoadingViewBase } from "@/shared/components/LoadingViewBase";
import { FolderSearch, Loader2 } from "lucide-react";
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
      <LoadingViewBase
        icon={<Loader2 className="w-12 h-12 animate-spin" />}
        title="Please wait"
        description="We are loading your Short URLs..."
      />
    );
  }

  if (isError) {
    return <FormAlert isError={isError} error={error} />;
  }

  if (shortUrls?.length == 0) {
    return (
      <EmptyViewBase
        icon={<FolderSearch className="w-20 h-20" />}
        title="Couldn't find any Short URLs"
        description="Shorten your long URL or clear filter to see them here."
      />
    );
  }

  return (
    <div className={cn("flex flex-col gap-y-3", className)} {...other}>
      {shortUrls?.map((u) => render(u))}
    </div>
  );
}
