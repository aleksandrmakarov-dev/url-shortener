import { authKeys } from "@/entities/auth/api";
import { SignInDto } from "@/lib/dto/auth/sign-in.dto";
import { TokenDto } from "@/lib/dto/auth/token.dto";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";

async function signInLocal(dto: SignInDto) {
  const response = await axios.post<TokenDto>("/auth/sign-in", dto);
  return response.data;
}

export const useSignInLocal = () => {
  return useMutation<TokenDto, AxiosError<ErrorResponseDto>, SignInDto>({
    mutationKey: authKeys.mutations.signInLocal(),
    mutationFn: async (data) => {
      return await signInLocal(data);
    },
  });
};
