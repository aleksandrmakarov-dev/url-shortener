import { ShortUrlFilterForm } from "@/entities/short-url";
import { FilterShortUrlRequest } from "@/lib/dto/short-url/filter-short-url.request";

export function FilterShortUrlCard() {
  const onSubmit = (data: FilterShortUrlRequest) => {
    console.log(data);
  };

  return (
    <div className="border border-border rounded-md p-3 bg-white">
      <ShortUrlFilterForm filter={{ query: "" }} onSubmit={onSubmit} />
    </div>
  );
}
