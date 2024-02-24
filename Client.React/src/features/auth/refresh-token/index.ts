import { authKeys } from "@/entities/auth/api";
import axios from "@/lib/axios";
import { SessionResponse } from "@/lib/dto/auth/session.response";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";

async function refreshToken() {
  const response = await axios.post<SessionResponse>("/auth/refresh-token");
  return response.data;
}

export const useRefreshToken = () => {
  return useMutation<SessionResponse, AxiosError<ErrorResponse>, unknown>({
    mutationKey: authKeys.mutations.refreshToken(),
    mutationFn: async () => {
      return await refreshToken();
    },
  });
};
