import { CardContainer } from "@/shared/components/CardContainer";
import { Button } from "@/shared/ui/button";
import { CreateShortUrlCard } from "@/widgets/short-url";

export default function HomePage() {
  return (
    <div className="mx-auto max-w-screen-sm">
      <div className="text-center pb-10">
        <h1 className="text-5xl font-semibold mb-5">Free URL Shortener</h1>
        <p className="text-xl">
          Create short links, QR Codes, and Link-in-bio pages. Share them
          anywhere. Track what’s working, and what’s not.
        </p>
      </div>
      <CreateShortUrlCard className="mb-10 p-8" />
      <CardContainer>
        <div className="text-center">
          <h1 className="text-3xl font-semibold mb-5">
            Need more features? <br /> Create free account!
          </h1>
          <p className="mb-5 max-w-md mx-auto">
            Custom short links, powerful dashboard, detailed analytics, API, UTM
            builder, QR codes, browser extension, app integrations and support
          </p>
          <Button>Create Account</Button>
        </div>
      </CardContainer>
    </div>
  );
}
