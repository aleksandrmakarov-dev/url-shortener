import { copyShortUrlToClipboard } from "@/features/short-url";
import { ShortUrlResponse } from "@/lib/dto/short-url/short-url.response";
import { Button } from "@/shared/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/shared/ui/dropdown-menu";
import {
  Copy,
  Edit,
  ExternalLink,
  LineChart,
  MoreVertical,
  Trash,
} from "lucide-react";
import moment from "moment";

interface ShortUrlCardProps {
  shortUrl: ShortUrlResponse;
  onEditClick?: (id: string) => void;
  onDeleteClick?: (id: string) => void;
}

export function ShortUrlCard({
  shortUrl: { id, alias, domain, original, createdAt, expiresAt },
  onDeleteClick,
  onEditClick,
}: ShortUrlCardProps) {
  return (
    <div className="border border-border rounded-md bg-white p-5">
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
        <a className="text-sm underline-offset-2 hover:underline" href="/">
          0 clicks
        </a>
        <div>
          <p className="text-sm">{moment(createdAt).format("L")}</p>
          {expiresAt && (
            <p className="text-sm text-red-500">
              {moment(expiresAt).format("L")}
            </p>
          )}
        </div>
        <div className="text-end">
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button size="icon" variant="ghost">
                <MoreVertical />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-42 absolute top-0 -right-4">
              <DropdownMenuItem
                onClick={() => copyShortUrlToClipboard(domain, alias)}
                asChild
              >
                <span className="cursor-pointer">
                  <Copy className="w-4 h-4 mr-1.5" />
                  <span>Copy link</span>
                </span>
              </DropdownMenuItem>
              <DropdownMenuItem onClick={() => onEditClick?.(id)} asChild>
                <span className="cursor-pointer">
                  <Edit className="w-4 h-4 mr-1.5" />
                  <span>Edit link</span>
                </span>
              </DropdownMenuItem>
              <DropdownMenuItem asChild>
                <span className="cursor-pointer">
                  <LineChart className="w-4 h-4 mr-1.5" />
                  <span>Statistics</span>
                </span>
              </DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem onClick={() => onDeleteClick?.(id)} asChild>
                <span className="cursor-pointer">
                  <Trash className="w-4 h-4 mr-1.5" />
                  <span>Delete link</span>
                </span>
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </div>
      </div>
    </div>
  );
}
