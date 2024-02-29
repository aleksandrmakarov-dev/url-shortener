import { useSession } from "@/context/session-provider/SessionProvider";
import { Button } from "@/shared/ui/button";
import {
  CreateShortUrlDialog,
  FilterShortUrlCard,
  UserShortUrlList,
} from "@/widgets/short-url";
import { Link } from "lucide-react";

export default function LinksPage() {
  const { session } = useSession();

  return (
    <div className="grid grid-cols-3 gap-x-3">
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
            shortUrl={{
              original: "",
              userId: session?.userId,
            }}
          />
        </div>
      </div>
    </div>
  );
}
