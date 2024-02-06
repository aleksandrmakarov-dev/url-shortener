import HomePage from "@/pages/(main)/home/page";
import MainLayout from "@/pages/(main)/layout";
import AuthLayout from "@/pages/auth/layout";
import SignInPage from "@/pages/auth/sign-in/page";
import SignUpPage from "@/pages/auth/sign-up/page";
import FullPageWrapper from "@/shared/components/FullPageWrapper";
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
      </Route>
      <Route path="/auth" element={<AuthLayout />}>
        <Route path="sign-in" element={<SignInPage />} />
        <Route path="sign-up" element={<SignUpPage />} />
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
