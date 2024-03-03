import { useSession } from "@/context/session-provider/SessionProvider";
import { MenuBase } from "@/shared/components/MenuBase";
import { Button } from "@/shared/ui/button";
import { CircleUserRound } from "lucide-react";

export function UserProfileMenu() {
  const { session } = useSession();

  if (!session) {
    return null;
  }

  return (
    <MenuBase
      trigger={
        <Button className="rounded-full" size="icon" variant="ghost">
          <CircleUserRound />
        </Button>
      }
      label={session.email}
    >
      <a className="cursor-pointer" href={`/links/u/${session.userId}`}>
        My links
      </a>
      <a className="cursor-pointer" href="/auth/sign-out">
        Sign out
      </a>
    </MenuBase>
  );
}
