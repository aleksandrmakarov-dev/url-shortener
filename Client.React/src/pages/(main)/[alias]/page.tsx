import { useShortUrlByAlias } from "@/entities/short-url/api";
import { FormAlert } from "@/shared/components/FormAlert";
import { useEffect } from "react";
import { useParams } from "react-router-dom";

export default function RedirectPage() {
  const { alias } = useParams();

  const { data, isSuccess, isLoading, isError, error } = useShortUrlByAlias({
    alias: alias,
  });

  useEffect(() => {
    if (data && data.original) {
      window.location.replace(data.original);
    }
  }, [data]);

  if (isLoading) {
    return <p className="text-center">Redirecting...</p>;
  }

  return (
    <FormAlert
      className="max-w-md mx-auto"
      isSuccess={isSuccess}
      success={{
        title: "Redirect sucessfully",
        message: (
          <>
            If you were not redirected automatically click{" "}
            <a
              className="underline font-medium underline-offset-1"
              href={data?.original}
            >
              Here
            </a>
            .
          </>
        ),
      }}
      isError={isError}
      error={{
        title: error?.response?.data.error,
        message: error?.response?.data.message,
      }}
    />
  );
}
