import { authKeys } from "@/entities/auth/api";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";

async function signOut() {
  await axios.delete("/auth/sign-out");
}

export const useSignOut = () => {
  return useMutation<unknown, AxiosError<ErrorResponseDto>, unknown>({
    mutationKey: authKeys.mutations.signOut(),
    mutationFn: async () => {
      await signOut();
    },
  });
};
