import { ShortUrlCard, ShortUrlList } from "@/entities/short-url";
import { useShortUrls } from "@/entities/short-url/api";
import { ListPagination } from "@/shared/components/ListPagination";
import { HTMLAttributes } from "react";
import { useParams, useSearchParams } from "react-router-dom";

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
    //userId: userId,
  });

  return (
    <div {...props}>
      <ShortUrlList
        className="mb-3"
        shortUrls={data?.items ?? []}
        render={(item) => <ShortUrlCard key={item.id} shortUrl={item} />}
        isLoading={isLoading}
      />
      {data && <ListPagination pagination={data.pagination} />}
    </div>
  );
}
