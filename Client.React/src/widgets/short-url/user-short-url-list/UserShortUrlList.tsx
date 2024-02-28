import { ShortUrlCard, ShortUrlList } from "@/entities/short-url";
import { HTMLAttributes } from "react";

interface UserShortUrlListProps extends HTMLAttributes<HTMLDivElement> {}

export function UserShortUrlList(props: UserShortUrlListProps) {
  return (
    <ShortUrlList
      shortUrls={Array(10).fill({
        id: Math.round(Math.random() * 100000).toString(),
        domain: "http://localhost:5173",
        alias: Math.round(Math.random() * 100000).toString(),
        createdAt: new Date(Date.now()),
        original:
          "https://www.youtube.com/watch?v=0Ru9O_oWrzE&ab_channel=alongthisjourney",
      })}
      render={(item) => <ShortUrlCard key={item.id} shortUrl={item} />}
      {...props}
    />
  );
}
