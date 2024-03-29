import RedirectPage from "@/pages/(main)/[alias]/page";
import AccessDeniedPage from "@/pages/access-denied/page";
import HomePage from "@/pages/(main)/home/page";
import MainLayout from "@/pages/(main)/layout";
import LinksPage from "@/pages/(main)/links/u/[userId]/page";
import StatsPage from "@/pages/(main)/stats/[alias]/page";
import AuthLayout from "@/pages/auth/layout";
import NewEmailVerificationPage from "@/pages/auth/new-email-verification/page";
import SignInPage from "@/pages/auth/sign-in/page";
import SignOutPage from "@/pages/auth/sign-out/page";
import SignUpPage from "@/pages/auth/sign-up/page";
import VerifyEmailPage from "@/pages/auth/verify-email/page";
import FullPageWrapper from "@/shared/components/FullPageWrapper";
import { RoutePublicGuard } from "@/shared/components/RoutePublicGuard";
import { RouteRoleGuard } from "@/shared/components/RouteRoleGuard";
import { Suspense } from "react";
import {
  createBrowserRouter,
  createRoutesFromElements,
  RouterProvider as ReactDOMRouterProvider,
  Route,
} from "react-router-dom";
import { ErrorBoundaryView } from "@/shared/components/ErrorBoundaryView";
import AdminPage from "@/pages/(main)/admin/test/page";
import { Role } from "@/lib/utils";

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route errorElement={<ErrorBoundaryView />}>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<HomePage />} />
        <Route path=":alias" element={<RedirectPage />} />
        <Route path="links">
          <Route element={<RouteRoleGuard />}>
            <Route path="u">
              <Route path=":userId" element={<LinksPage />} />
            </Route>
          </Route>
        </Route>
        <Route element={<RouteRoleGuard />}>
          <Route path="stats">
            <Route path=":id" element={<StatsPage />} />
          </Route>
        </Route>
        <Route path="admin" element={<RouteRoleGuard roles={[Role.Admin]} />}>
          <Route path="test" element={<AdminPage />} />
        </Route>
      </Route>
      <Route path="/auth" element={<AuthLayout />}>
        <Route element={<RoutePublicGuard />}>
          <Route path="sign-in" element={<SignInPage />} />
          <Route path="sign-up" element={<SignUpPage />} />
        </Route>
        <Route path="verify-email" element={<VerifyEmailPage />} />
        <Route
          path="new-email-verification"
          element={<NewEmailVerificationPage />}
        />
        <Route path="sign-out" element={<SignOutPage />} />
      </Route>
      <Route path="access-denied" element={<AccessDeniedPage />} />
    </Route>
  )
);

export default function RouterProvider() {
  return (
    <Suspense
      fallback={
        <FullPageWrapper className="flex justify-center items-center">
          <div>
            <h5 className="font-semibold text-lg">Please wait for a while</h5>
            <p className="text-gray-700">Loading page...</p>
          </div>
        </FullPageWrapper>
      }
    >
      <ReactDOMRouterProvider router={router} />
    </Suspense>
  );
}
