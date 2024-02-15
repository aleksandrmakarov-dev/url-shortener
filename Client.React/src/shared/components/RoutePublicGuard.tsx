import { useSession } from "@/context/session-provider/SessionProvider";
import { Navigate, Outlet } from "react-router-dom";

export function RoutePublicGuard() {
  const { user, isLoading } = useSession();

  if (isLoading) {
    return null;
  }

  return user ? <Navigate to="/" /> : <Outlet />;
}
