import { ShortUrlCard } from "@/entities/short-url";
import { useShortUrlById } from "@/entities/short-url/api";
import { StatisticsAgent, StatisticsLocation } from "@/entities/statistics";
import { useStatisticsById } from "@/entities/statistics/api";
import { copyShortUrlToClipboard } from "@/features/short-url";
import { FormAlert } from "@/shared/components/FormAlert";
import { Header } from "@/shared/components/Header";
import { Button } from "@/shared/ui/button";
import { Copy } from "lucide-react";
import { useParams } from "react-router-dom";

export default function StatsPage() {
  const { id } = useParams();

  const {
    data: shortUrlData,
    isLoading: isGetLoading,
    isError: isGetError,
    error: getError,
  } = useShortUrlById({
    id: id,
  });

  const {
    data: statisticsData,
    isLoading: isStatisticsLoading,
    isError: isStatisticsError,
    error: statisticsError,
  } = useStatisticsById({
    id: id,
  });

  return (
    <div className="max-w-screen-md mx-auto">
      <Header
        className="mb-5"
        title="Your Short URL's Statistics"
        description="View location, platform and device of users who visited your short URL."
      />
      <FormAlert
        className="mb-5"
        isError={isGetError}
        error={getError?.response?.data}
      />
      {isGetLoading ? (
        <p>Loading...</p>
      ) : shortUrlData ? (
        <ShortUrlCard
          className="mb-5"
          shortUrl={shortUrlData}
          actions={
            <div className="text-end">
              <Button
                size="icon"
                variant="default"
                onClick={() =>
                  copyShortUrlToClipboard(
                    shortUrlData.domain,
                    shortUrlData.alias
                  )
                }
              >
                <Copy className="w-5 h-5" />
              </Button>
            </div>
          }
        />
      ) : null}
      {isStatisticsError ? (
        <FormAlert
          isError={!isGetError && isStatisticsError}
          error={statisticsError.response?.data}
        />
      ) : (
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
      )}
    </div>
  );
}
