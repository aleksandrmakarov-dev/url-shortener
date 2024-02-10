import { SignUpDto } from "@/lib/dto/auth/sign-up.dto";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { authKeys } from "@/entities/auth/api";

async function signUpLocal(dto: SignUpDto) {
  await axios.post("/auth/sign-up", dto);
}

export const useSignUpLocal = () => {
  return useMutation<unknown, AxiosError<ErrorResponseDto>, SignUpDto>({
    mutationKey: authKeys.mutations.signUpLocal(),
    mutationFn: async (data) => {
      await signUpLocal(data);
    },
  });
};
