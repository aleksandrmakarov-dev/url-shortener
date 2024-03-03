import { copyShortUrlToClipboard } from "@/features/short-url";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { Button } from "@/shared/ui/button";
import { Input } from "@/shared/ui/input";
import { Label } from "@/shared/ui/label";
import { Copy } from "lucide-react";

interface ShortenedUrlProps {
  data: ShortUrlResponse;
  back?: () => void;
}

export function ShortenedUrl({
  data: { original, domain, alias },
  back,
}: ShortenedUrlProps) {
  return (
    <div className="space-y-5">
      <div className="space-y-2">
        <Label>Original</Label>
        <Input value={original} readOnly />
      </div>
      <div className="space-y-2">
        <Label>Short URL</Label>
        <Input value={`${domain}/${alias}`} readOnly />
      </div>
      <div className="flex flex-col gap-3 md:flex-row md:justify-end">
        <Button
          className="w-full  md:w-auto"
          variant="outline"
          onClick={() => copyShortUrlToClipboard(domain, alias)}
        >
          <Copy className="w-4 h-4 mr-1.5" />
          <span>Copy</span>
        </Button>
        <Button className="w-full md:w-auto" onClick={back}>
          Shorten another one
        </Button>
      </div>
    </div>
  );
}
