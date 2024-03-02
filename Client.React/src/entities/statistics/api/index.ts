import axios from "@/lib/axios";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { StatisticsResponse } from "@/lib/dto/statistics/statistics.response";
import { useQuery } from "@tanstack/react-query";
import { AxiosError } from "axios";

export const statisticsKeys = {
  shortUrls: {
    root: ["statistics"],
    id: (id: string) => [...statisticsKeys.shortUrls.root, "id", id],
  },
};

type StatisticsByIdParams = {
  id?: string;
};

async function fetchStatisticsById(params: StatisticsByIdParams) {
  const response = await axios.get<StatisticsResponse>(
    `/statistics/${params.id}`
  );
  return response.data;
}

export const useStatisticsById = (params: StatisticsByIdParams) => {
  return useQuery<
    StatisticsResponse,
    AxiosError<ErrorResponse>,
    StatisticsResponse,
    unknown[]
  >({
    queryKey: statisticsKeys.shortUrls.id(params.id!),
    queryFn: async () => {
      return await fetchStatisticsById(params);
    },
    enabled: !!params.id,
  });
};
