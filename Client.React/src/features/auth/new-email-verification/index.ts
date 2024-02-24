import { authKeys } from "@/entities/auth/api";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { NewEmailVerificationRequest } from "@/lib/dto/auth/new-email-verification.request";

async function newEmailVerification(request: NewEmailVerificationRequest) {
  await axios.post("/auth/new-email-verification", request);
}

export const useNewEmailVerification = () => {
  return useMutation<unknown, AxiosError<ErrorResponse>, NewEmailVerificationRequest>({
    mutationKey: authKeys.mutations.signInLocal(),
    mutationFn: async (data) => {
      await newEmailVerification(data);
    },
  });
};
