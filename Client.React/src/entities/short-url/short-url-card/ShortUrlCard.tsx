import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { UTCToLocal } from "@/lib/utils";
import { CardContainer } from "@/shared/components/CardContainer";
import { ExternalLink, LineChart } from "lucide-react";
import { HTMLAttributes } from "react";

interface ShortUrlCardProps extends HTMLAttributes<HTMLDivElement> {
  shortUrl: ShortUrlResponse;
  actions?: React.ReactNode;
}

export function ShortUrlCard({
  shortUrl: { id, alias, domain, original, createdAt, expiresAt },
  actions,
  ...other
}: ShortUrlCardProps) {
  return (
    <CardContainer {...other}>
      <div className="flex flex-col md:grid grid-cols-8 gap-3 md:items-center">
        <div className="col-span-5">
          <div className="flex items-center mb-1">
            <ExternalLink className="w-5 h-5 mr-1.5 shrink-0" />
            <a
              target="_blank"
              className="font-semibold underline-offset-2 hover:underline truncate"
              href={`${domain}/${alias}`}
            >{`${domain}/${alias}`}</a>
          </div>
          <div className="truncate">
            <a
              target="_blank"
              className="text-sm text-muted-foreground underline-offset-2 hover:underline"
              href={original}
            >
              {original}
            </a>
          </div>
        </div>
        <a
          className="text-sm flex items-center underline-offset-2 hover:underline"
          href={`/stats/${id}`}
        >
          <LineChart className="w-5 h-5 mr-1.5" /> <span>Stats</span>
        </a>
        <div>
          <p className="text-sm">{UTCToLocal(createdAt, "DD/MM/YYYY")}</p>
          {expiresAt && (
            <p className="text-sm text-red-500">
              {UTCToLocal(expiresAt, "DD/MM/YYYY HH:mm")}
            </p>
          )}
        </div>
        {actions}
      </div>
    </CardContainer>
  );
}
