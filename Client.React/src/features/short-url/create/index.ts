import { shortUrlsKeys } from "@/entities/short-url/api";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";

async function createShortUrl(request: EditShortUrlRequest) {
  const response = await axios.post<ShortUrlResponse>("/short-urls", request);
  return response.data;
}

export const useCreateShortUrl = () => {
  return useMutation<
    ShortUrlResponse,
    AxiosError<ErrorResponse>,
    EditShortUrlRequest
  >({
    mutationKey: shortUrlsKeys.mutations.create(),
    mutationFn: async (data) => {
      return await createShortUrl(data);
    },
  });
};
