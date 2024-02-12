import { UserDto } from "@/lib/dto/user/user.dto";
import { useQuery } from "@tanstack/react-query";
import { AxiosError } from "axios";
import axios from "@/lib/axios";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";

export const userKeys = {
  users: {
    root: ["users"],
    id: (id: string) => [...userKeys.users.root, "id", id],
  },
};

async function fetchUserById(params: UserByIdParams) {
  const response = await axios.get<UserDto>(`/users/${params.id}`);
  return response.data;
}

type UserByIdParams = {
  id?: string;
};

export const useUserById = (params: UserByIdParams) => {
  return useQuery<UserDto, AxiosError<ErrorResponseDto>, UserDto, unknown[]>({
    queryKey: userKeys.users.id(params.id!),
    queryFn: async () => {
      return await fetchUserById(params);
    },
    enabled: !!params.id,
  });
};
