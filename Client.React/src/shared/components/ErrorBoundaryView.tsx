import { isRouteErrorResponse, useRouteError } from "react-router-dom";
import { ErrorViewBase } from "./ErrorViewBase";

export function ErrorBoundaryView() {
  const error = useRouteError();

  if (isRouteErrorResponse(error)) {
    return (
      <ErrorViewBase
        status={error.status}
        statusText={error.statusText}
        message={error.data}
      />
    );
  } else if (error instanceof Error) {
    return <ErrorViewBase message={error.message} />;
  } else {
    return <ErrorViewBase />;
  }
}
