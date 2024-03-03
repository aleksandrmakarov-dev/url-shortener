import { ShortUrlCard } from "@/entities/short-url";
import { useShortUrlById } from "@/entities/short-url/api";
import { copyShortUrlToClipboard } from "@/features/short-url";
import { FormAlert } from "@/shared/components/FormAlert";
import { LoadingViewBase } from "@/shared/components/LoadingViewBase";
import { Button } from "@/shared/ui/button";
import { Loader2, Copy } from "lucide-react";

interface CurrentShortUrlCardProps {
  id?: string;
}

export function CurrentShortUrlCard({ id }: CurrentShortUrlCardProps) {
  const {
    data: shortUrlData,
    isLoading: isGetLoading,
    isError: isGetError,
    error: getError,
  } = useShortUrlById({
    id: id,
  });

  if (isGetLoading) {
    return (
      <LoadingViewBase
        icon={<Loader2 className="w-12 h-12 animate-spin" />}
        title="Please wait"
        description="We are loading your Short URL..."
      />
    );
  }

  if (isGetError || !shortUrlData) {
    return (
      <FormAlert
        className="mb-5"
        isError={isGetError}
        error={getError?.response?.data}
      />
    );
  }

  return (
    <ShortUrlCard
      className="mb-5"
      shortUrl={shortUrlData}
      actions={
        <div className="text-end">
          <Button
            size="icon"
            variant="default"
            onClick={() =>
              copyShortUrlToClipboard(shortUrlData.domain, shortUrlData.alias)
            }
          >
            <Copy className="w-5 h-5" />
          </Button>
        </div>
      }
    />
  );
}
