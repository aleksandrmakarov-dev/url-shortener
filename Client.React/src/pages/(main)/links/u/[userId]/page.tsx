import { Header } from "@/shared/components/Header";
import { Button } from "@/shared/ui/button";
import {
  CreateShortUrlDialog,
  FilterShortUrlCard,
  UserShortUrlList,
} from "@/widgets/short-url";
import { Link } from "lucide-react";

export default function LinksPage() {
  return (
    <>
      <Header
        className="mb-5"
        title="My Short URLs"
        description="Access, manage and view statistics of all your custom short URLs conveniently in one place."
      />
      <div className="flex flex-col-reverse md:grid grid-cols-3 gap-3">
        <UserShortUrlList className="col-span-2" />
        <div className="space-y-3">
          <FilterShortUrlCard />
          <div className="border border-border p-5 bg-white rounded-md">
            <p className="text-center mb-3 font-semibold text-foreground">
              Getting Started
            </p>
            <CreateShortUrlDialog
              trigger={
                <Button className="w-full">
                  <Link className="w-4 h-4 mr-1.5" />
                  <span>Shorten my URL</span>
                </Button>
              }
            />
          </div>
        </div>
      </div>
    </>
  );
}
