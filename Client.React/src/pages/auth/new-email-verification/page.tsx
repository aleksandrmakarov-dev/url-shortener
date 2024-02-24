import { NewEmailVerificationCard } from "@/widgets/auth";
import { useSearchParams } from "react-router-dom";

export default function NewEmailVerificationPage() {
  const [searchParams] = useSearchParams();

  return (
    <NewEmailVerificationCard
      data={{
        email: searchParams.get("email") || "",
      }}
    />
  );
}
