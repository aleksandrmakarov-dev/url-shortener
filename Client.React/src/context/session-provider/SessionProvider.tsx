import { useRefreshToken } from "@/features/auth/refresh-token";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { UserDto } from "@/lib/dto/user/user.dto";
import {
  Dispatch,
  SetStateAction,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import axios from "@/lib/axios";
import { useUserById } from "@/entities/user/api";
import { TokenDto } from "@/lib/dto/auth/token.dto";

type SessionContextData = {
  user?: UserDto;
  setToken: Dispatch<SetStateAction<TokenDto | undefined>>;
  isLoading?: boolean;
  isError?: boolean;
  error?: ErrorResponseDto;
};

const SessionContext = createContext<SessionContextData>({
  setToken: () => {},
  isLoading: false,
});

export const useSession = () => {
  return useContext<SessionContextData>(SessionContext);
};

export default function SessionProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [isLoading, setIsLoading] = useState<boolean>();
  const [token, setToken] = useState<TokenDto>();

  const {
    mutate: refreshTokenMutate,
    isPending: isRefreshTokenLoading,
    isError: isRefreshTokenError,
    error: refreshTokenError,
  } = useRefreshToken();

  const {
    data: userData,
    isPending: isUserLoading,
    isError: isUserError,
    error: userError,
  } = useUserById({
    id: token?.userId,
  });

  useEffect(() => {
    if (!token) {
      refreshTokenMutate(
        {},
        {
          onSuccess: (data) => {
            setToken(data);
          },
          onSettled: (_) => setIsLoading(false),
        }
      );
    } else {
      axios.interceptors.request.use(
        (config) => {
          if (!config.headers.Authorization) {
            config.headers.Authorization = `Bearer ${token.accessToken}`;
          }

          return config;
        },
        (error) => Promise.reject(error)
      );
    }
  }, [token]);

  return (
    <SessionContext.Provider
      value={{
        user: userData,
        setToken: setToken,
        isLoading: isRefreshTokenLoading || isUserLoading || isLoading,
        isError: isRefreshTokenError || isUserError,
        error: refreshTokenError?.response?.data || userError?.response?.data,
      }}
    >
      {children}
    </SessionContext.Provider>
  );
}
