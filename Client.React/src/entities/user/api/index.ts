import { useQuery } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { UserResponse } from "@/lib/dto/user/user.response";
import { ErrorResponse } from "@/lib/dto/common/error.response";

export const userKeys = {
  users: {
    root: ["users"],
    id: (id: string) => [...userKeys.users.root, "id", id],
  },
};

async function fetchUserById(params: UserByIdParams) {
  const response = await axios.get<UserResponse>(`/users/${params.id}`);
  return response.data;
}

type UserByIdParams = {
  id?: string;
};

export const useUserById = (params: UserByIdParams) => {
  return useQuery<UserResponse, AxiosError<ErrorResponse>, UserResponse, unknown[]>({
    queryKey: userKeys.users.id(params.id!),
    queryFn: async () => {
      return await fetchUserById(params);
    },
    enabled: !!params.id,
  });
};
