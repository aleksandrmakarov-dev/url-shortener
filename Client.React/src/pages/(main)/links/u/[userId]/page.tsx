import { FilterShortUrlCard, UserShortUrlList } from "@/widgets/short-url";

export default function LinksPage() {
  return (
    <div className="grid grid-cols-3 gap-x-3">
      <UserShortUrlList className="col-span-2" />
      <div className="space-y-3">
        <FilterShortUrlCard />
        <div className="border border-border p-5 bg-white rounded-md"></div>
      </div>
    </div>
  );
}
