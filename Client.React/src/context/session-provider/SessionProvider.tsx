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
  const [session, setSession] = useState<SessionResponse>();

  const {
    mutate: refreshTokenMutate,
    isPending: isRefreshTokenLoading,
    isError: isRefreshTokenError,
    error: refreshTokenError,
  } = useRefreshToken();

  useEffect(() => {
    if (!session) {
      refreshTokenMutate(
        {},
        {
          onSuccess: (data) => {
            setSession(data);
          },
          onError: (e) => {
            console.log(e);
          },
        }
      );
    } else {
      setAuthorizationToken(session.accessToken);
    }
  }, [session]);

  return (
    <SessionContext.Provider
      value={{
        session: session,
        setSession: setSession,
        isLoading: isRefreshTokenLoading,
        isError: isRefreshTokenError,
        error: refreshTokenError?.response?.data,
      }}
    >
      {children}
    </SessionContext.Provider>
  );
}
