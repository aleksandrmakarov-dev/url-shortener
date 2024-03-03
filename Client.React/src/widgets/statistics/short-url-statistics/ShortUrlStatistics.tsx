import { StatisticsAgent, StatisticsLocation } from "@/entities/statistics";
import { useStatisticsById } from "@/entities/statistics/api";
import { FormAlert } from "@/shared/components/FormAlert";
import { LoadingViewBase } from "@/shared/components/LoadingViewBase";
import { Loader2 } from "lucide-react";

interface ShortUrlStatisticsProps {
  id?: string;
}

export function ShortUrlStatistics({ id }: ShortUrlStatisticsProps) {
  const {
    data: statisticsData,
    isLoading: isStatisticsLoading,
    isError: isStatisticsError,
    error: statisticsError,
  } = useStatisticsById({
    id: id,
  });

  if (isStatisticsLoading) {
    return (
      <LoadingViewBase
        icon={<Loader2 className="w-12 h-12 animate-spin" />}
        title="Please wait"
        description="We are loading your Short URL statistics..."
      />
    );
  }

  if (isStatisticsError) {
    return (
      <FormAlert
        isError={isStatisticsError}
        error={statisticsError.response?.data}
      />
    );
  }

  return (
    <>
      <StatisticsLocation
        className="mb-5"
        data={statisticsData?.countries}
        isLoading={isStatisticsLoading}
      />
      <StatisticsAgent
        data={[
          {
            name: "Platforms",
            values: statisticsData?.platforms,
          },
          {
            name: "Browsers",
            values: statisticsData?.countries,
          },
        ]}
        isLoading={isStatisticsLoading}
      />
    </>
  );
}
