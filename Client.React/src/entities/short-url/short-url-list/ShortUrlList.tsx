import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { cn } from "@/lib/utils";
import React, { HTMLAttributes } from "react";

interface ShortUrlListProps extends HTMLAttributes<HTMLDivElement> {
  shortUrls?: ShortUrlResponse[];
  render: (item: ShortUrlResponse) => React.ReactNode;
}

export function ShortUrlList({
  shortUrls,
  render,
  className,
  ...other
}: ShortUrlListProps) {
  return (
    <div className={cn("flex flex-col gap-y-3", className)} {...other}>
      {shortUrls?.map((u) => render(u))}
    </div>
  );
}
