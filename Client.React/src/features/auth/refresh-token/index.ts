import { authKeys } from "@/entities/auth/api";
import axios from "@/lib/axios";
import { TokenDto } from "@/lib/dto/auth/token.dto";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";

async function refreshToken() {
  const response = await axios.post<TokenDto>("/auth/refresh-token");
  return response.data;
}

export const useRefreshToken = () => {
  return useMutation<TokenDto, AxiosError<ErrorResponseDto>, unknown>({
    mutationKey: authKeys.mutations.refreshToken(),
    mutationFn: async () => {
      return await refreshToken();
    },
  });
};
