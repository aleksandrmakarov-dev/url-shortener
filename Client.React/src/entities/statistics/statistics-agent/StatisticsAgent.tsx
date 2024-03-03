import { KeyValuePair } from "@/lib/dto/statistics/statistics.response";
import { cn } from "@/lib/utils";
import { CardContainer } from "@/shared/components/CardContainer";
import { HTMLAttributes } from "react";
import { Pie, Cell, PieChart, Legend } from "recharts";
import { StatisticsEmptyView } from "../statistics-empty-view/StatisticsEmptyView";

const COLORS = [
  "#0088FE", // Blue
  "#00C49F", // Green
  "#FFBB28", // Yellow
  "#FF8042", // Orange
  "#4CAF50", // Emerald Green
  "#9C27B0", // Purple
  "#FF5252", // Red
  "#FFD740", // Amber
  "#03A9F4", // Light Blue
  "#4DB6AC", // Teal
  "#FF5722", // Deep Orange
  "#673AB7", // Deep Purple
  "#2196F3", // Blue
  "#F44336", // Red
  "#9E9E9E", // Grey
  "#795548", // Brown
];

interface StatisticsAgentProps extends HTMLAttributes<HTMLDivElement> {
  data?: { name: string; values?: KeyValuePair[] }[];
  isLoading?: boolean;
}

export function StatisticsAgent({
  data,
  isLoading,
  className,
  ...other
}: StatisticsAgentProps) {
  if (isLoading) {
    return (
      <p className={cn(className)}>
        Loading platform and browser statistics...
      </p>
    );
  }

  return (
    <CardContainer {...other}>
      <div className="flex flex-col items-center md:grid grid-cols-2 overflow-auto">
        {data && data.length > 0 ? (
          data.map(({ name, values }) => (
            <div key={`item-${name}`}>
              <h5 className="text-xl font-medium mb-3 text-center">{name}</h5>
              {values && values.length > 0 ? (
                <PieChart height={256} width={356}>
                  <Pie
                    dataKey="value"
                    isAnimationActive={false}
                    data={values}
                    cx="50%"
                    cy="50%"
                    innerRadius={40}
                    outerRadius={80}
                    fill="#8884d8"
                    paddingAngle={5}
                    label
                  >
                    {values?.map((_entry, index) => (
                      <Cell
                        key={`cell-${index}`}
                        fill={COLORS[index % COLORS.length]}
                      />
                    ))}
                  </Pie>
                  <Legend
                    layout="vertical"
                    verticalAlign="middle"
                    align="right"
                    height={36}
                    payload={values?.map((v, index) => ({
                      id: v.key,
                      type: "square",
                      value: v.key,
                      color: COLORS[index % COLORS.length],
                    }))}
                  />
                </PieChart>
              ) : (
                <StatisticsEmptyView />
              )}
            </div>
          ))
        ) : (
          <StatisticsEmptyView />
        )}
      </div>
    </CardContainer>
  );
}
