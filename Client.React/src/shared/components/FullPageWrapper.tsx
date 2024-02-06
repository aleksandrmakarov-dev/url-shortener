import { cn } from "@/lib/utils";
import { HTMLAttributes } from "react";

interface FullPageWrapperProps extends HTMLAttributes<HTMLDivElement> {
  children?: React.ReactNode;
}

export default function FullPageWrapper({
  className,
  children,
  ...other
}: FullPageWrapperProps) {
  return (
    <div className={cn("min-h-screen", className)} {...other}>
      {children}
    </div>
  );
}
