import { useSession } from "@/context/session-provider/SessionProvider";
import { ShortUrlForm, ShortenedUrl } from "@/entities/short-url";
import { useCreateShortUrl } from "@/features/short-url/create";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
import { cn, isNullOrEmpty } from "@/lib/utils";
import { FormAlert } from "@/shared/components/FormAlert";
import { HTMLAttributes } from "react";

interface CreateShortUrlCardProps extends HTMLAttributes<HTMLDivElement> {}

export function CreateShortUrlCard({
  className,
  ...other
}: CreateShortUrlCardProps) {
  const { session } = useSession();

  const { mutate, isPending, isSuccess, data, isError, error, reset } =
    useCreateShortUrl();

  const onSubmit = (request: EditShortUrlRequest) => {
    mutate({
      ...request,
      expiresAt: isNullOrEmpty(request.expiresAt)
        ? undefined
        : request.expiresAt,
    });
  };

  return (
    <div
      className={cn(
        "bg-white border border-border rounded-md p-8 max-w-screen-sm mx-auto",
        className
      )}
      {...other}
    >
      {isSuccess ? (
        <>
          <h1 className="mb-5 text-3xl font-semibold">
            Here is your short URL
          </h1>
          <ShortenedUrl data={data} back={() => reset()} />
        </>
      ) : (
        <>
          <h1 className="mb-5 text-3xl font-semibold">Shorten your long URL</h1>
          <FormAlert
            className="mb-3"
            isError={isError}
            error={{
              title: error?.response?.data.error,
              message: error?.response?.data.message,
            }}
          />
          <ShortUrlForm
            onSubmit={onSubmit}
            isLoading={isPending}
            shortUrl={{
              original: "",
              customAlias: "",
              userId: session?.userId,
            }}
          />
        </>
      )}
    </div>
  );
}
