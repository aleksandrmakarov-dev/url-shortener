import { authKeys } from "@/entities/auth/api";
import { VerifyEmailDto } from "@/lib/dto/auth/verify-email.dto";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";

async function verifyEmail(dto: VerifyEmailDto) {
  await axios.post("/auth/verify-email", dto);
}

export const useVerifyEmail = () => {
  return useMutation<unknown, AxiosError<ErrorResponseDto>, VerifyEmailDto>({
    mutationKey: authKeys.mutations.signInLocal(),
    mutationFn: async (data) => {
      await verifyEmail(data);
    },
  });
};
