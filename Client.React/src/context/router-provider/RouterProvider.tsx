import RedirectPage from "@/pages/(main)/[alias]/page";
import AccessDeniedPage from "@/pages/(main)/access-denied/page";
import HomePage from "@/pages/(main)/home/page";
import MainLayout from "@/pages/(main)/layout";
import AuthLayout from "@/pages/auth/layout";
import NewEmailVerificationPage from "@/pages/auth/new-email-verification/page";
import SignInPage from "@/pages/auth/sign-in/page";
import SignOutPage from "@/pages/auth/sign-out/page";
import SignUpPage from "@/pages/auth/sign-up/page";
import VerifyEmailPage from "@/pages/auth/verify-email/page";
import FullPageWrapper from "@/shared/components/FullPageWrapper";
import { RoutePublicGuard } from "@/shared/components/RoutePublicGuard";
import { Suspense } from "react";
import {
  createBrowserRouter,
  createRoutesFromElements,
  RouterProvider as ReactDOMRouterProvider,
  Route,
} from "react-router-dom";

const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<HomePage />} />
        <Route path=":alias" element={<RedirectPage />} />
        <Route path="/access-denied" element={<AccessDeniedPage />} />
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
    </>
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
