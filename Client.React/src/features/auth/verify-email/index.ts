import { authKeys } from "@/entities/auth/api";
import { VerifyEmailRequest } from "@/lib/dto/auth/verify-email.request";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";

async function verifyEmail(request: VerifyEmailRequest) {
  await axios.post("/auth/verify-email", request);
}

export const useVerifyEmail = () => {
  return useMutation<unknown, AxiosError<ErrorResponse>, VerifyEmailRequest>({
    mutationKey: authKeys.mutations.verifyEmail(),
    mutationFn: async (data) => {
      await verifyEmail(data);
    },
  });
};
