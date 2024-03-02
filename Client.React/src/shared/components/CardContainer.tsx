import { cn } from "@/lib/utils";
import { HTMLAttributes } from "react";

interface CardContainerProps extends HTMLAttributes<HTMLDivElement> {
  children?: React.ReactNode;
}

export function CardContainer({
  children,
  className,
  ...other
}: CardContainerProps) {
  return (
    <div
      className={cn("border border-border bg-white rounded-md p-5", className)}
      {...other}
    >
      {children}
    </div>
  );
}
