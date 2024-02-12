import { useRefreshToken } from "@/features/auth/refresh-token";
import { ErrorResponseDto } from "@/lib/dto/common/error-response.dto";
import { UserSessionDto } from "@/lib/dto/user/user-session.dto";
import {
  Dispatch,
  SetStateAction,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";

type SessionContextData = {
  user?: UserSessionDto;
  setAccessToken: Dispatch<SetStateAction<string | undefined>>;
  isLoading?: boolean;
  isError?: boolean;
  error?: ErrorResponseDto;
};

const SessionContext = createContext<SessionContextData>({
  setAccessToken: (_) => {},
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
  const {
    mutate,
    isPending: isRefreshTokenLoading,
    isError: isRefreshTokenError,
    error: refreshTokenError,
  } = useRefreshToken();

  const [user, setUser] = useState<UserSessionDto>();
  const [accessToken, setAccessToken] = useState<string>();

  useEffect(() => {
    if (!accessToken) {
      mutate(
        {},
        {
          onSuccess: (data) => {
            setAccessToken(data.accessToken);
          },
        }
      );
    } else {
    }
  }, [accessToken]);

  return (
    <SessionContext.Provider
      value={{
        user: user,
        setAccessToken: setAccessToken,
        isLoading: isRefreshTokenLoading,
        isError: isRefreshTokenError,
        error: refreshTokenError?.response?.data,
      }}
    >
      {children}
    </SessionContext.Provider>
  );
}
