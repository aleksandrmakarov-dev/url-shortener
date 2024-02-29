import { ShortUrlCard, ShortUrlList } from "@/entities/short-url";
import { useShortUrls } from "@/entities/short-url/api";
import { ListPagination } from "@/shared/components/ListPagination";
import { HTMLAttributes, useState } from "react";
import { useParams, useSearchParams } from "react-router-dom";
import { DeleteShortUrlDialog } from "../delete-short-url-dialog/DeleteShortUrlDialog";
import { UpdateShortUrlDialog } from "../update-short-url-dialog/UpdateShortUrlDialog";

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

  return (
    <div {...props}>
      <ShortUrlList
        className="mb-3"
        shortUrls={data?.items ?? []}
        render={(item) => (
          <ShortUrlCard
            key={item.id}
            shortUrl={item}
            onDeleteClick={onDeleteClick}
            onEditClick={onEditClick}
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
      {data && <ListPagination pagination={data.pagination} />}
    </div>
  );
}
