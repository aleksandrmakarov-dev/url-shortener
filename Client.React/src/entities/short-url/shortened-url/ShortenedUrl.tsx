import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { Button } from "@/shared/ui/button";
import { Input } from "@/shared/ui/input";
import { Label } from "@/shared/ui/label";

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
      <Button onClick={back}>Shorten another one</Button>
    </div>
  );
}
