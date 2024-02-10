import { VerifyEmailCard } from "@/widgets/auth";
import { useSearchParams } from "react-router-dom";

export default function VerifyEmailPage() {
  const [searchParams] = useSearchParams();

  return (
    <VerifyEmailCard
      data={{
        email: searchParams.get("email") || "",
        token: searchParams.get("token") || "",
      }}
    />
  );
}
