import { useSession } from "@/context/session-provider/SessionProvider";
import { Button } from "@/shared/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/shared/ui/dropdown-menu";
import { CircleUserRound } from "lucide-react";

export function UserProfileMenu() {
  const { session } = useSession();

  if (!session) {
    return null;
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button className="rounded-full" size="icon" variant="ghost">
          <CircleUserRound />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-42 absolute top-0 -right-4">
        <DropdownMenuLabel className="truncate p-2">
          {session.email}
        </DropdownMenuLabel>
        <DropdownMenuItem asChild>
          <a className="cursor-pointer" href={`/links/u/${session.userId}`}>
            My links
          </a>
        </DropdownMenuItem>
        <DropdownMenuSeparator />
        <DropdownMenuItem asChild>
          <a className="cursor-pointer" href="/auth/sign-out">
            Sign out
          </a>
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
