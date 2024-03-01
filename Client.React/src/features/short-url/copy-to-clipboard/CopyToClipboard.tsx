import { writeToClipboard } from "@/lib/utils";
import { CheckCircle } from "lucide-react";
import { toast } from "sonner";

export function copyShortUrlToClipboard(domain: string, alias: string) {
  const url = `${domain}/${alias}`;
  writeToClipboard(url);
  toast("Short URL copied", {
    description: `${url} has been copied to your clipboard`,
    icon: <CheckCircle className="pr-1.5 text-green-500" />,
  });
}
