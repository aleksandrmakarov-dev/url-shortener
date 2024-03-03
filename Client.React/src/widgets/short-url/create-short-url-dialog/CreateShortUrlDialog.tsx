import { ShortUrlForm } from "@/entities/short-url";
import { shortUrlsKeys } from "@/entities/short-url/api";
import { copyShortUrlToClipboard } from "@/features/short-url";
import { useCreateShortUrl } from "@/features/short-url";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
import { LocalToUTC } from "@/lib/utils";
import { DialogBase } from "@/shared/components/DialogBase";
import { FormAlert } from "@/shared/components/FormAlert";
import { useQueryClient } from "@tanstack/react-query";
import { CheckCircle } from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";

interface CreateShortUrlDialogProps {
  trigger: JSX.Element;
  shortUrl?: EditShortUrlRequest;
}

export function CreateShortUrlDialog({
  trigger,
  shortUrl,
}: CreateShortUrlDialogProps) {
  const queryClient = useQueryClient();

  const [open, setOpen] = useState<boolean>(false);
  const { mutate, isPending, isError, error } = useCreateShortUrl();

  const onSubmit = (request: EditShortUrlRequest) => {
    mutate(
      { ...request, expiresAt: LocalToUTC(request.expiresAt) },
      {
        onSuccess: (data) => {
          setOpen(false);
          queryClient.invalidateQueries({
            queryKey: shortUrlsKeys.shortUrls.query(),
          });
          toast("Short URL created", {
            description: "A new Short URL has been created successfully",
            icon: <CheckCircle className="pr-1.5 text-green-500" />,
            action: {
              label: "Copy",
              onClick: () => copyShortUrlToClipboard(data.domain, data.alias),
            },
          });
        },
        onError: (e) => console.log(e),
      }
    );
  };

  return (
    <DialogBase
      open={open}
      setOpen={setOpen}
      title="Create Short URL"
      description="Change the fields to update your Short URL"
      trigger={trigger}
    >
      <FormAlert
        className="mb-3"
        isError={isError}
        error={error?.response?.data}
      />
      <ShortUrlForm
        onSubmit={onSubmit}
        isLoading={isPending}
        shortUrl={shortUrl}
        btnLabel="Create URL"
      />
    </DialogBase>
  );
}
