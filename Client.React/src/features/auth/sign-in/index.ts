import { authKeys } from "@/entities/auth/api";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { SessionResponse } from "@/lib/dto/auth/session.response";
import { SignInRequest } from "@/lib/dto/auth/sign-in.request";
import { ErrorResponse } from "@/lib/dto/common/error.response";

async function signInLocal(request: SignInRequest) {
  const response = await axios.post<SessionResponse>("/auth/sign-in", request);
  return response.data;
}

export const useSignInLocal = () => {
  return useMutation<SessionResponse, AxiosError<ErrorResponse>, SignInRequest>({
    mutationKey: authKeys.mutations.signInLocal(),
    mutationFn: async (data) => {
      return await signInLocal(data);
    },
  });
};
