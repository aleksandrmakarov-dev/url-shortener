import { useSession } from "@/context/session-provider/SessionProvider";
import { Navigate, Outlet } from "react-router-dom";

interface RouteRoleGuardProps {
  roles?: string[];
}

export function RouteRoleGuard(props: RouteRoleGuardProps) {
  const { roles } = props;

  const { session, isLoading } = useSession();

  if (isLoading) {
    return null;
  }

  if (session) {
    if (roles && roles.includes("")) {
      return <Outlet />;
    } else {
      return <Navigate to="/access-denied" replace />;
    }
  } else {
    return <Navigate to="/auth/sign-in" replace />;
  }
}
