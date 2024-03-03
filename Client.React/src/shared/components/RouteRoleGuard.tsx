import { useSession } from "@/context/session-provider/SessionProvider";
import { Navigate, Outlet } from "react-router-dom";

interface RouteRoleGuardProps {
  roles?: string[];
}

export function RouteRoleGuard(props: RouteRoleGuardProps) {
  const { roles } = props;

  const { session, isLoading } = useSession();

  if (isLoading) {
    // If session is still loading, return null or a loading indicator
    return null;
  }

  if (!session) {
    // If there's no session, redirect to sign-in
    console.log(session);
    return <Navigate to="/auth/sign-in" replace />;
  }

  if (roles && roles.length > 0) {
    // If roles are specified and the user has at least one of the roles
    if (roles.includes(session.role)) {
      return <Outlet />;
    } else {
      // If the user doesn't have any of the specified roles, redirect to access denied
      return <Navigate to="/access-denied" replace />;
    }
  }

  // If no roles are specified, allow access
  return <Outlet />;
}
