import axios from "@/lib/axios";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { useQuery } from "@tanstack/react-query";
import { AxiosError } from "axios";

export const shortUrlsKeys = {
  shortUrls: {
    root: ["short-urls"],
    alias: (alias: string) => [...shortUrlsKeys.shortUrls.root, "alias", alias],
  },
  mutations: {
    create: () => [...shortUrlsKeys.shortUrls.root, "create"],
  },
};

type ShortUrlByAliasParams = {
  alias?: string;
};

async function fetchShortUrlByAlias(params: ShortUrlByAliasParams) {
  const response = await axios.get<ShortUrlResponse>(
    `/short-urls/${params.alias}`
  );
  return response.data;
}

export const useShortUrlByAlias = (params: ShortUrlByAliasParams) => {
  return useQuery<
    ShortUrlResponse,
    AxiosError<ErrorResponse>,
    ShortUrlResponse,
    unknown[]
  >({
    queryKey: shortUrlsKeys.shortUrls.alias(params.alias!),
    queryFn: async () => {
      return await fetchShortUrlByAlias(params);
    },
    enabled: !!params.alias,
  });
};
