import { Pagination as PaginationData } from "@/lib/dto/common/paged.response";
import { HTMLAttributes, useCallback } from "react";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationPrevious,
  PaginationLink,
  PaginationNext,
} from "../ui/pagination";
import { useLocation, useSearchParams } from "react-router-dom";
import { cn } from "@/lib/utils";

interface ListPaginationProps extends HTMLAttributes<HTMLDivElement> {
  pagination: PaginationData;
}

export function ListPagination({
  pagination: { hasNextPage, hasPreviousPage, page },
  ...other
}: ListPaginationProps) {
  const location = useLocation();
  const [searchParams] = useSearchParams();

  const createURL = useCallback(
    (pageValue: number) => {
      const params = new URLSearchParams(searchParams);
      params.set("page", pageValue.toString());
      return `${location.pathname}?${params.toString()}`;
    },
    [location, searchParams]
  );

  return (
    <Pagination {...other}>
      <PaginationContent>
        <PaginationItem className={cn({ hidden: !hasPreviousPage })}>
          <PaginationPrevious
            href={hasPreviousPage ? createURL(page - 1) : undefined}
          />
        </PaginationItem>
        <PaginationItem className={cn({ hidden: !hasPreviousPage })}>
          <PaginationLink
            href={hasPreviousPage ? createURL(page - 1) : undefined}
          >
            {page - 1}
          </PaginationLink>
        </PaginationItem>
        <PaginationItem>
          <PaginationLink href={createURL(page)} isActive>
            {page}
          </PaginationLink>
        </PaginationItem>
        <PaginationItem className={cn({ hidden: !hasNextPage })}>
          <PaginationLink href={hasNextPage ? createURL(page + 1) : undefined}>
            {page + 1}
          </PaginationLink>
        </PaginationItem>
        <PaginationItem className={cn({ hidden: !hasNextPage })}>
          <PaginationNext
            href={hasNextPage ? createURL(page + 1) : undefined}
          />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  );
}
