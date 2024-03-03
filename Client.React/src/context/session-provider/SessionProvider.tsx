import { useRefreshToken } from "@/features/auth/refresh-token";
import { ErrorResponse } from "@/lib/dto/common/error.response";
import {
  Dispatch,
  SetStateAction,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import { setAuthorizationToken } from "@/lib/axios";
import { SessionResponse } from "@/lib/dto/auth/session.response";

type SessionContextData = {
  session?: SessionResponse;
  setSession: Dispatch<SetStateAction<SessionResponse | undefined>>;
  isLoading?: boolean;
  isError?: boolean;
  error?: ErrorResponse;
  isSuccess?: boolean;
};

const SessionContext = createContext<SessionContextData>({
  setSession: () => {},
});

export const useSession = () => {
  return useContext<SessionContextData>(SessionContext);
};

export default function SessionProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [session, setSession] = useState<SessionResponse | undefined>(
    undefined
  );

  const [isLoading, setIsLoading] = useState<boolean>(true);

  const {
    mutate: refreshTokenMutate,
    isPending: isRefreshTokenLoading,
    isError: isRefreshTokenError,
    error: refreshTokenError,
  } = useRefreshToken();

  useEffect(() => {
    if (!session) {
      setIsLoading(true);
      refreshTokenMutate(
        {},
        {
          onSuccess: (data) => {
            setAuthorizationToken(data.accessToken);
            setSession(data);
          },
          onError: (e) => {
            console.log(e);
          },
          onSettled: () => setIsLoading(false),
        }
      );
    } else {
      if (isLoading) setIsLoading(false);
    }
  }, [session]);

  return (
    <SessionContext.Provider
      value={{
        session: session,
        setSession: setSession,
        isLoading: isLoading || isRefreshTokenLoading,
        isError: isRefreshTokenError,
        error: refreshTokenError?.response?.data,
      }}
    >
      {children}
    </SessionContext.Provider>
  );
}
