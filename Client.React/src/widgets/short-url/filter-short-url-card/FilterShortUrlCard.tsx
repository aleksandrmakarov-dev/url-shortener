import { ShortUrlFilterForm } from "@/entities/short-url";
import { FilterShortUrlRequest } from "@/lib/dto/short-url/filter-short-url.request";
import { useSearchParams } from "react-router-dom";

export function FilterShortUrlCard() {
  const [searchParams, setSearchParams] = useSearchParams();

  const onSubmit = (data: FilterShortUrlRequest) => {
    const key = "query";
    const params = new URLSearchParams(searchParams);

    if (data.query) {
      params.set(key, data.query);
    } else {
      params.delete(key);
    }

    params.set("page", "1");

    setSearchParams(params);
  };

  return (
    <div className="border border-border rounded-md p-3 bg-white">
      <ShortUrlFilterForm
        filter={{ query: searchParams.get("query") ?? "" }}
        onSubmit={onSubmit}
      />
    </div>
  );
}
