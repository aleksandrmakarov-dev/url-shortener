import { CardContainer } from "@/shared/components/CardContainer";
import { Button } from "@/shared/ui/button";
import { CreateShortUrlCard } from "@/widgets/short-url";

export default function HomePage() {
  return (
    <div className="mx-auto max-w-screen-sm">
      <div className="text-center pb-10">
        <h1 className="text-5xl font-semibold mb-5">Free URL Shortener</h1>
        <p className="text-xl">
          Create short links, set custom alias and expiration time, monitor
          navigation statistics. Share them anywhere.
        </p>
      </div>
      <CreateShortUrlCard className="mb-10 p-8" />
      <CardContainer>
        <div className="text-center">
          <h1 className="text-3xl font-semibold mb-5">
            Need more features? <br /> Create free account!
          </h1>
          <p className="mb-5 max-w-md mx-auto">
            After creating account you unlock features like custom aliases,
            expiration time and navigation statistics
          </p>
          <Button asChild>
            <a href="/auth/sign-up">Create Account</a>
          </Button>
        </div>
      </CardContainer>
    </div>
  );
}
