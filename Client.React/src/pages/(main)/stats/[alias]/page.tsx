import { Header } from "@/shared/components/Header";
import { CurrentShortUrlCard } from "@/widgets/short-url";
import { ShortUrlStatistics } from "@/widgets/statistics";
import { useParams } from "react-router-dom";

export default function StatsPage() {
  const { id } = useParams();

  return (
    <div className="max-w-screen-md mx-auto">
      <Header
        className="mb-5"
        title="Your Short URL's Statistics"
        description="View location, platform and device of users who visited your short URL."
      />
      <CurrentShortUrlCard id={id} />
      <ShortUrlStatistics id={id} />
    </div>
  );
}
