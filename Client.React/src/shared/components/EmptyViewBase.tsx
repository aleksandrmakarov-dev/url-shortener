import { Search } from "lucide-react";

interface EmptyViewBaseProps {
  icon?: React.ReactNode;
  title?: string;
  description?: string;
}

export function EmptyViewBase({
  icon,
  title,
  description,
}: EmptyViewBaseProps) {
  return (
    <div className="w-full h-full flex items-center justify-center">
      <div className="text-center flex flex-col items-center py-10">
        {icon ?? <Search className="w-8 h-8" />}
        <h5 className="text-lg font-medium">
          {title ?? "Cound't find anything"}
        </h5>
        {description && <p className="text-muted-foreground">{description}</p>}
      </div>
    </div>
  );
}
