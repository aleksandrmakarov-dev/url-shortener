import { useSession } from "@/context/session-provider/SessionProvider";
import { ShortUrlForm, ShortenedUrl } from "@/entities/short-url";
import { copyShortUrlToClipboard } from "@/features/short-url";
import { useCreateShortUrl } from "@/features/short-url/create";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
import { LocalToUTC } from "@/lib/utils";
import { CardContainer } from "@/shared/components/CardContainer";
import { FormAlert } from "@/shared/components/FormAlert";
import { CheckCircle } from "lucide-react";
import { HTMLAttributes } from "react";
import { toast } from "sonner";

interface CreateShortUrlCardProps extends HTMLAttributes<HTMLDivElement> {}

export function CreateShortUrlCard(props: CreateShortUrlCardProps) {
  const { session } = useSession();

  const { mutate, isPending, isSuccess, data, isError, error, reset } =
    useCreateShortUrl();

  const onSubmit = (request: EditShortUrlRequest) => {
    mutate(
      { ...request, expiresAt: LocalToUTC(request.expiresAt) },
      {
        onSuccess: (data) => {
          toast("Short URL created", {
            description: "A new Short URL has been created successfully",
            icon: <CheckCircle className="pr-1.5 text-green-500" />,
            action: {
              label: "Copy",
              onClick: () => copyShortUrlToClipboard(data.domain, data.alias),
            },
          });
        },
      }
    );
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
              expiresAt: "",
              userId: session?.userId,
            }}
          />
        </>
      )}
    </CardContainer>
  );
}
