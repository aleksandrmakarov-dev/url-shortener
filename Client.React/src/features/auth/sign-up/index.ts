import { SignUpDto } from "@/lib/dto/auth/sign-up.dto";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { authKeys } from "@/entities/auth/api";
import { MessageResponseDto } from "@/lib/dto/common/message-response.dto";

async function signUpLocal(dto: SignUpDto) {
  const response = await axios.post<MessageResponseDto>("/auth/sign-up", dto);
  return response.data;
}

export const useSignUpLocal = () => {
  return useMutation<
    MessageResponseDto,
    AxiosError<ErrorResponseDto>,
    SignUpDto
  >({
    mutationKey: authKeys.mutations.signUpLocal(),
    mutationFn: async (data) => {
      return await signUpLocal(data);
    },
  });
};
