import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { authKeys } from "@/entities/auth/api";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { SignUpRequest } from "@/lib/dto/auth/sign-up.request";
import { MessageResponse } from "@/lib/dto/common/message.response";

async function signUpLocal(request: SignUpRequest) {
  const response = await axios.post<MessageResponse>("/auth/sign-up", request);
  return response.data;
}

export const useSignUpLocal = () => {
  return useMutation<
    MessageResponse,
    AxiosError<ErrorResponse>,
    SignUpRequest
  >({
    mutationKey: authKeys.mutations.signUpLocal(),
    mutationFn: async (data) => {
      return await signUpLocal(data);
    },
  });
};
