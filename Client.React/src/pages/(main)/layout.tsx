import { useSession } from "@/context/session-provider/SessionProvider";
import FullPageWrapper from "@/shared/components/FullPageWrapper";
import { Button } from "@/shared/ui/button";
import { UserProfileMenu } from "@/widgets/user";
import { Outlet } from "react-router-dom";

export default function MainLayout() {
  const { session, isLoading } = useSession();

  return (
    <FullPageWrapper className="bg-gray-50">
      <nav className="sticky h-14 flex items-center bg-gray-50 top-0 px-5 z-10">
        <div className="max-w-screen-lg w-full mx-auto flex items-center justify-between">
          <div>
            <a className="text-lg font-bold" href="/">
              SHRT.COM
            </a>
          </div>
          {isLoading ? (
            <p>Loading...</p>
          ) : session ? (
            <UserProfileMenu />
          ) : (
            <div className="space-x-2">
              <Button size="sm" variant="ghost" asChild>
                <a href="/auth/sign-up">Sign up</a>
              </Button>
              <Button size="sm" asChild>
                <a href="/auth/sign-in">Sign in</a>
              </Button>
            </div>
          )}
        </div>
      </nav>
      <div className="max-w-screen-lg mx-auto py-10 px-5 xl:px-0">
        <Outlet />
      </div>
    </FullPageWrapper>
  );
}
