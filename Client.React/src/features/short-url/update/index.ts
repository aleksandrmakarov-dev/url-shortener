import { shortUrlsKeys } from "@/entities/short-url/api";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";

type UpdateShortUrlParams = {
  id: string;
  value: EditShortUrlRequest;
};

async function updateShortUrl(request: UpdateShortUrlParams) {
  const response = await axios.put<unknown>(
    `/short-urls/${request.id}`,
    request.value
  );
  return response.data;
}

export const useUpdateShortUrlById = () => {
  return useMutation<unknown, AxiosError<ErrorResponse>, UpdateShortUrlParams>({
    mutationKey: shortUrlsKeys.mutations.update(),
    mutationFn: async (data) => {
      return await updateShortUrl(data);
    },
  });
};
