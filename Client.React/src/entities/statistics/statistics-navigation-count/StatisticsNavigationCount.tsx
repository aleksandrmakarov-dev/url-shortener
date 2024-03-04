import { cn } from "@/lib/utils";
import { CardContainer } from "@/shared/components/CardContainer";
import { HTMLAttributes } from "react";

interface StatisticsNavigationCountProps
  extends HTMLAttributes<HTMLDivElement> {
  navigationCount?: number;
  isLoading?: boolean;
}

export function StatisticsNavigationCount({
  navigationCount,
  isLoading,
  className,
  ...other
}: StatisticsNavigationCountProps) {
  if (isLoading) {
    <p>Loading...</p>;
  }

  return (
    <CardContainer className={cn(className)} {...other}>
      <p>
        Your Short URL was navigated{" "}
        <span className="font-semibold">{navigationCount}</span> time(s)
      </p>
    </CardContainer>
  );
}
