import axios from "@/lib/axios";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { PagedResponse } from "@/lib/dto/common/paged.response";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { useQuery } from "@tanstack/react-query";
import { AxiosError } from "axios";

export const shortUrlsKeys = {
  shortUrls: {
    root: ["short-urls"],
    alias: (alias: string) => [...shortUrlsKeys.shortUrls.root, "alias", alias],
    query: (params?: ShortUrlsParams) => [
      ...shortUrlsKeys.shortUrls.root,
      { ...params },
    ],
  },
  mutations: {
    create: () => [...shortUrlsKeys.shortUrls.root, "create"],
    update: () => [...shortUrlsKeys.shortUrls.root, "update"],
    delete: () => [...shortUrlsKeys.shortUrls.root, "delete"],
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

type ShortUrlsParams = {
  page?: number | null;
  size?: number | null;
  query?: string | null;
  userId?: string | null;
};

async function fetchShortUrls(params?: ShortUrlsParams) {
  const response = await axios.get<PagedResponse<ShortUrlResponse>>(
    "/short-urls",
    {
      params: params,
    }
  );

  return response.data;
}

export const useShortUrls = (params?: ShortUrlsParams) => {
  return useQuery<
    PagedResponse<ShortUrlResponse>,
    AxiosError<ErrorResponse>,
    PagedResponse<ShortUrlResponse>
  >({
    queryKey: shortUrlsKeys.shortUrls.query(params),
    queryFn: async () => {
      return await fetchShortUrls(params);
    },
  });
};
