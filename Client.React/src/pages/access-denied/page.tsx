import { ErrorViewBase } from "@/shared/components/ErrorViewBase";

export default function AccessDeniedPage() {
  return (
    <ErrorViewBase
      status={401}
      statusText="Access Denied"
      message="Sorry, you can't have access to the page you’re looking for."
    />
  );
}
