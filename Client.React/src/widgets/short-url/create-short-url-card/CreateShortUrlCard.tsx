import { useSession } from "@/context/session-provider/SessionProvider";
import { ShortUrlForm, ShortenedUrl } from "@/entities/short-url";
import { useCreateShortUrl } from "@/features/short-url/create";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
import { LocalToUTC } from "@/lib/utils";
import { CardContainer } from "@/shared/components/CardContainer";
import { FormAlert } from "@/shared/components/FormAlert";
import { HTMLAttributes } from "react";

interface CreateShortUrlCardProps extends HTMLAttributes<HTMLDivElement> {}

export function CreateShortUrlCard(props: CreateShortUrlCardProps) {
  const { session } = useSession();

  const { mutate, isPending, isSuccess, data, isError, error, reset } =
    useCreateShortUrl();

  const onSubmit = (request: EditShortUrlRequest) => {
    mutate({ ...request, expiresAt: LocalToUTC(request.expiresAt) });
  };

  return (
    <CardContainer {...props}>
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
    </CardContainer>
  );
}
