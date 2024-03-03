import { shortUrlsKeys } from "@/entities/short-url/api";
import { useDeleteShortUrlById } from "@/features/short-url/delete";
import { DialogBase } from "@/shared/components/DialogBase";
import { FormAlert } from "@/shared/components/FormAlert";
import { Button } from "@/shared/ui/button";
import { useQueryClient } from "@tanstack/react-query";
import { CheckCircle } from "lucide-react";
import { Dispatch, SetStateAction } from "react";
import { toast } from "sonner";

interface DeleteShortUrlDialogProps {
  id: string;
  open: boolean;
  setOpen: Dispatch<SetStateAction<boolean>>;
}

export function DeleteShortUrlDialog({
  id,
  open,
  setOpen,
}: DeleteShortUrlDialogProps) {
  const queryClient = useQueryClient();

  const { mutate, isPending, isError, error, reset } = useDeleteShortUrlById();

  const onSubmit = () => {
    mutate(
      { id: id },
      {
        onSuccess: () => {
          reset();

          setOpen(false);

          queryClient.invalidateQueries({
            queryKey: shortUrlsKeys.shortUrls.query(),
          });

          toast("Short URL deleted", {
            description: "Short URL has been deleted successfully",
            icon: <CheckCircle className="pr-1.5 text-green-500" />,
          });
        },
      }
    );
  };

  const onCancel = () => {
    reset();
    setOpen(false);
  };

  return (
    <DialogBase open={open} setOpen={setOpen} title="Delete Short URL">
      <FormAlert
        className="mb-3"
        isError={isError}
        error={error?.response?.data}
      />
      <p className="mb-3">
        Are you sure you want to delete this Short URL? This action cannot be
        undone.
      </p>
      <div className="flex gap-x-3 justify-end">
        <Button
          className="hidden md:block"
          disabled={isPending}
          type="button"
          variant="outline"
          onClick={onCancel}
        >
          No, keep it
        </Button>
        <Button
          className="w-full"
          loading={isPending}
          type="button"
          variant="destructive"
          onClick={onSubmit}
        >
          Yes, delete it
        </Button>
      </div>
    </DialogBase>
  );
}
