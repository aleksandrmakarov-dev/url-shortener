import { ShortUrlCard, ShortUrlList } from "@/entities/short-url";
import { useShortUrls } from "@/entities/short-url/api";
import { ListPagination } from "@/shared/components/ListPagination";
import { HTMLAttributes, useState } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { DeleteShortUrlDialog } from "../delete-short-url-dialog/DeleteShortUrlDialog";
import { UpdateShortUrlDialog } from "../update-short-url-dialog/UpdateShortUrlDialog";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/shared/ui/dropdown-menu";
import { Button } from "@/shared/ui/button";
import { copyShortUrlToClipboard } from "@/features/short-url";
import { MoreVertical, Copy, Edit, LineChart, Trash } from "lucide-react";

interface UserShortUrlListProps extends HTMLAttributes<HTMLDivElement> {}

export function UserShortUrlList(props: UserShortUrlListProps) {
  const [searchParams] = useSearchParams();
  const { userId } = useParams();

  const { data, isLoading, isError, error } = useShortUrls({
    page: searchParams.has("page")
      ? Number(searchParams.get("page"))
      : undefined,
    size: searchParams.has("size")
      ? Number(searchParams.get("size"))
      : undefined,
    query: searchParams.get("query"),
    userId: userId,
  });

  const navigate = useNavigate();

  const [deleteId, setDeleteId] = useState<string | undefined>(undefined);
  const [updateId, setUpdateId] = useState<string | undefined>(undefined);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState<boolean>(false);
  const [updateDialogOpen, setUpdateDialogOpen] = useState<boolean>(false);

  const onDeleteClick = (id: string) => {
    setDeleteId(id);
    setDeleteDialogOpen(true);
  };

  const onEditClick = (id: string) => {
    setUpdateId(id);
    setUpdateDialogOpen(true);
  };

  const onStatisticsClick = (id: string) => {
    navigate(`/stats/${id}`);
  };

  return (
    <div {...props}>
      <ShortUrlList
        className="mb-3"
        shortUrls={data?.items ?? []}
        render={(item) => (
          <ShortUrlCard
            key={item.id}
            shortUrl={item}
            actions={
              <div className="text-end">
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button size="icon" variant="ghost">
                      <MoreVertical />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent className="w-42 absolute top-0 -right-4">
                    <DropdownMenuItem
                      onClick={() =>
                        copyShortUrlToClipboard(item.domain, item.alias)
                      }
                      asChild
                    >
                      <span className="cursor-pointer">
                        <Copy className="w-4 h-4 mr-1.5" />
                        <span>Copy link</span>
                      </span>
                    </DropdownMenuItem>
                    <DropdownMenuItem
                      onClick={() => onEditClick(item.id)}
                      asChild
                    >
                      <span className="cursor-pointer">
                        <Edit className="w-4 h-4 mr-1.5" />
                        <span>Edit link</span>
                      </span>
                    </DropdownMenuItem>
                    <DropdownMenuItem
                      onClick={() => onStatisticsClick(item.id)}
                      asChild
                    >
                      <span className="cursor-pointer">
                        <LineChart className="w-4 h-4 mr-1.5" />
                        <span>Statistics</span>
                      </span>
                    </DropdownMenuItem>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem
                      onClick={() => onDeleteClick(item.id)}
                      asChild
                    >
                      <span className="cursor-pointer">
                        <Trash className="w-4 h-4 mr-1.5" />
                        <span>Delete link</span>
                      </span>
                    </DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>
              </div>
            }
          />
        )}
        isLoading={isLoading}
        isError={isError}
        error={error?.response?.data}
      />
      {deleteId && (
        <DeleteShortUrlDialog
          id={deleteId}
          open={deleteDialogOpen}
          setOpen={setDeleteDialogOpen}
        />
      )}
      {updateId && (
        <UpdateShortUrlDialog
          id={updateId}
          open={updateDialogOpen}
          setOpen={setUpdateDialogOpen}
        />
      )}
      {data &&
        (data.pagination.hasNextPage || data.pagination.hasPreviousPage) && (
          <ListPagination pagination={data.pagination} />
        )}
    </div>
  );
}
