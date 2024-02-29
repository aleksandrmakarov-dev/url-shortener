import axios from "@/lib/axios";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { PagedResponse } from "@/lib/dto/common/paged.response";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { useInfiniteQuery, useQuery } from "@tanstack/react-query";
import { AxiosError } from "axios";

export const shortUrlsKeys = {
  shortUrls: {
    root: ["short-urls"],
    alias: (alias: string) => [...shortUrlsKeys.shortUrls.root, "alias", alias],
    infinite: () => [...shortUrlsKeys.shortUrls.root, "infinity"],
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
    AxiosError,
    PagedResponse<ShortUrlResponse>
  >({
    queryKey: shortUrlsKeys.shortUrls.root,
    queryFn: async () => {
      return await fetchShortUrls(params);
    },
  });
};

export const useInfiniteShortUrls = (params?: ShortUrlsParams) => {
  return useInfiniteQuery<
    PagedResponse<ShortUrlResponse>,
    AxiosError,
    PagedResponse<ShortUrlResponse>
  >({
    queryKey: shortUrlsKeys.shortUrls.infinite(),
    queryFn: async () => {
      return await fetchShortUrls(params);
    },
    initialPageParam: { page: 1, size: 10 },
    getNextPageParam: (lastPage) =>
      lastPage.pagination.hasNextPage
        ? { page: lastPage.pagination.page + 1, size: lastPage.pagination.size }
        : undefined,
  });
};
