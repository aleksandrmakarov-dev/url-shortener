import { KeyValuePair } from "@/lib/dto/statistics/statistics.response";
import { cn } from "@/lib/utils";
import { CardContainer } from "@/shared/components/CardContainer";
import { HTMLAttributes } from "react";
import {
  BarChart,
  Bar,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { StatisticsEmptyView } from "../statistics-empty-view/StatisticsEmptyView";

interface StatisticsLocationProps extends HTMLAttributes<HTMLDivElement> {
  data?: KeyValuePair[];
  isLoading?: boolean;
}

export function StatisticsLocation({
  data,
  isLoading,
  className,
  ...other
}: StatisticsLocationProps) {
  if (isLoading) {
    return <p className={cn(className)}>Loading location statistics...</p>;
  }

  return (
    <CardContainer className={cn("h-96 flex flex-col", className)} {...other}>
      <h5 className="text-xl font-medium pb-3">Locations</h5>
      {data?.length === 0 ? (
        <StatisticsEmptyView />
      ) : (
        <div className="w-full h-full">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={data} layout="vertical">
              <XAxis type="number" />
              <YAxis dataKey="key" type="category" width={75} />
              <Bar
                dataKey="value"
                fill="#4b7bec"
                background={{ fill: "#eee" }}
              />
              <Tooltip />
            </BarChart>
          </ResponsiveContainer>
        </div>
      )}
    </CardContainer>
  );
}
