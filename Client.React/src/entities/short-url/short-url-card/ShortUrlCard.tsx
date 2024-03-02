import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { CardContainer } from "@/shared/components/CardContainer";
import { ExternalLink } from "lucide-react";
import moment from "moment";
import { HTMLAttributes } from "react";

interface ShortUrlCardProps extends HTMLAttributes<HTMLDivElement> {
  shortUrl: ShortUrlResponse;
  actions?: React.ReactNode;
}

export function ShortUrlCard({
  shortUrl: { alias, domain, original, createdAt, expiresAt },
  actions,
  ...other
}: ShortUrlCardProps) {
  return (
    <CardContainer {...other}>
      <div className="grid grid-cols-8 gap-x-3 items-center">
        <div className="col-span-5">
          <div className="flex items-center mb-1">
            <ExternalLink className="w-5 h-5 mr-2" />
            <a
              target="_blank"
              className="font-semibold underline-offset-2 hover:underline"
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
          className="text-sm underline-offset-2 hover:underline"
          href={`/stats/${alias}`}
        >
          0 clicks
        </a>
        <div>
          <p className="text-sm">{moment(createdAt).format("DD/MM/YYYY")}</p>
          {expiresAt && (
            <p className="text-sm text-red-500">
              {moment(expiresAt).format("DD/MM/YYYY")}
            </p>
          )}
        </div>
        {actions}
      </div>
    </CardContainer>
  );
}
