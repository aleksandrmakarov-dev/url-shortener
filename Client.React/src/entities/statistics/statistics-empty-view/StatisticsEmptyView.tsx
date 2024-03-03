import { EmptyViewBase } from "@/shared/components/EmptyViewBase";
import { FileSearch2Icon } from "lucide-react";

export function StatisticsEmptyView() {
  return (
    <EmptyViewBase
      icon={<FileSearch2Icon className="w-24 h-24" />}
      title="Couldn't find any statistics"
      description="Your Short URL has not been navigated yet."
    />
  );
}
