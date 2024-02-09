import FullPageWrapper from "@/shared/components/FullPageWrapper";
import { Button } from "@/shared/ui/button";
import { Outlet } from "react-router-dom";

export default function MainLayout() {
  return (
    <FullPageWrapper className="bg-gray-50">
      <nav className="sticky h-14 flex items-center bg-gray-50 top-0">
        <div className="max-w-screen-lg w-full mx-auto flex items-center justify-between">
          <div>
            <a className="text-lg font-bold" href="/">
              SHRT.COM
            </a>
          </div>
          <div className="space-x-10">
            <a
              className="font-semibold underline-offset-2 hover:underline "
              href="/"
            >
              Home
            </a>
            <a
              className="font-semibold underline-offset-2 hover:underline "
              href="/"
            >
              QR Codes
            </a>
            <a
              className="font-semibold underline-offset-2 hover:underline "
              href="/"
            >
              Stats
            </a>
          </div>
          <div className="space-x-2">
            <Button size="sm" variant="ghost" asChild>
              <a href="/auth/sign-up">Sign up Free</a>
            </Button>
            <Button size="sm" asChild>
              <a href="/auth/sign-in">Sign in</a>
            </Button>
          </div>
        </div>
      </nav>
      <div className="max-w-screen-lg mx-auto py-10">
        <Outlet />
      </div>
    </FullPageWrapper>
  );
}