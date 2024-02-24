import { authKeys } from "@/entities/auth/api";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { ErrorResponse } from "@/lib/dto/common/error.response";

async function signOut() {
  await axios.delete("/auth/sign-out");
}

export const useSignOut = () => {
  return useMutation<unknown, AxiosError<ErrorResponse>, unknown>({
    mutationKey: authKeys.mutations.signOut(),
    mutationFn: async () => {
      await signOut();
    },
  });
};
