import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { cn } from "@/lib/utils";
import React, { HTMLAttributes } from "react";

interface ShortUrlListProps extends HTMLAttributes<HTMLDivElement> {
  shortUrls?: ShortUrlResponse[];
  render: (item: ShortUrlResponse) => React.ReactNode;
  isLoading?: boolean;
}

export function ShortUrlList({
  shortUrls,
  render,
  isLoading,
  className,
  ...other
}: ShortUrlListProps) {
  if (isLoading) {
    return <p>Loading...</p>;
  }

  return (
    <div className={cn("flex flex-col gap-y-3", className)} {...other}>
      {shortUrls?.map((u) => render(u))}
    </div>
  );
}
