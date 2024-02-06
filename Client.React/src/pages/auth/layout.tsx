import FullPageWrapper from "@/shared/components/FullPageWrapper";
import { Outlet } from "react-router-dom";

export default function AuthLayout() {
  return (
    <FullPageWrapper className="flex items-center justify-center py-14 px-5">
      <Outlet />
    </FullPageWrapper>
  );
}
