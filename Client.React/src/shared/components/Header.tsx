import { cn } from "@/lib/utils";
import { HTMLAttributes } from "react";

interface HeaderProps extends HTMLAttributes<HTMLDivElement> {
  title: string;
  description?: string;
}

export function Header({
  title,
  description,
  className,
  ...other
}: HeaderProps) {
  return (
    <div className={cn(className)} {...other}>
      <h1 className="text-2xl font-semibold">{title}</h1>
      {description && <p className="text-lg">{description}</p>}
    </div>
  );
}
