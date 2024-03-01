import { ShortUrlForm } from "@/entities/short-url";
import { shortUrlsKeys, useShortUrlById } from "@/entities/short-url/api";
import { useUpdateShortUrlById } from "@/features/short-url/update";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
import { isNullOrEmpty } from "@/lib/utils";
import { DialogBase } from "@/shared/components/DialogBase";
import { FormAlert } from "@/shared/components/FormAlert";
import { useQueryClient } from "@tanstack/react-query";
import { CheckCircle } from "lucide-react";
import { Dispatch, SetStateAction } from "react";
import { toast } from "sonner";

interface UpdateShortUrlDialogProps {
  id: string;
  open: boolean;
  setOpen: Dispatch<SetStateAction<boolean>>;
}

export function UpdateShortUrlDialog({
  id,
  open,
  setOpen,
}: UpdateShortUrlDialogProps) {
  const queryClient = useQueryClient();

  const {
    mutate,
    isPending: isUpdateLoading,
    isError: isUpdateError,
    error: updateError,
    reset: resetUpdate,
  } = useUpdateShortUrlById();

  const {
    data: shortUrl,
    isLoading: isGetLoading,
    isError: isGetError,
    error: getError,
  } = useShortUrlById({
    id: id,
  });

  const onSubmit = (data: EditShortUrlRequest) => {
    mutate(
      {
        id: id,
        value: {
          ...data,
          expiresAt: isNullOrEmpty(data.expiresAt) ? undefined : data.expiresAt,
        },
      },
      {
        onSuccess: () => {
          setOpen(false);

          resetUpdate();

          queryClient.invalidateQueries({
            queryKey: shortUrlsKeys.shortUrls.query(),
          });

          toast("Short URL updated", {
            description: "Short URL has been updated successfully",
            icon: <CheckCircle className="pr-1.5 text-green-500" />,
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
      title="Update Short URL"
      description="Fill the fields to update your Short URL"
    >
      <FormAlert
        isError={isGetError || isUpdateError}
        error={getError?.response?.data || updateError?.response?.data}
      />
      <ShortUrlForm
        onSubmit={onSubmit}
        isLoading={isGetLoading || isUpdateLoading}
        shortUrl={{
          original: shortUrl?.original ?? "",
          userId: shortUrl?.userId,
          customAlias: shortUrl?.alias,
          expiresAt: shortUrl?.expiresAt?.toString(),
        }}
        btnLabel="Update my URL"
      />
    </DialogBase>
  );
}
