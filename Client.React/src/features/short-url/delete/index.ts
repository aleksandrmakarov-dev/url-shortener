import { shortUrlsKeys } from "@/entities/short-url/api";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";

type DeleteShortUrlParams = {
  id: string;
};

async function deleteShortUrl(params: DeleteShortUrlParams) {
  const response = await axios.delete<unknown>(`/short-urls/${params.id}`);
  return response.data;
}

export const useDeleteShortUrlById = () => {
  return useMutation<unknown, AxiosError<ErrorResponse>, DeleteShortUrlParams>({
    mutationKey: shortUrlsKeys.mutations.delete(),
    mutationFn: async (data) => {
      return await deleteShortUrl(data);
    },
  });
};
