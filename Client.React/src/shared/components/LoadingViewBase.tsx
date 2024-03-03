import { Loader2 } from "lucide-react";

interface LoadingViewBaseProps {
  icon?: React.ReactNode;
  title?: string;
  description?: string;
}

export function LoadingViewBase({
  icon,
  title,
  description,
}: LoadingViewBaseProps) {
  return (
    <div className="w-full h-full flex items-center justify-center">
      <div className="text-center flex flex-col items-center py-10">
        {icon ?? <Loader2 className="h-8 w-8 animate-spin" />}
        <h5 className="text-lg font-medium">{title ?? "Please wait"}</h5>
        <p className="text-muted-foreground">
          {description ?? "The data is loading..."}
        </p>
      </div>
    </div>
  );
}
