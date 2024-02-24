import { ShortUrlForm } from "@/entities/short-url";
import { EditShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";

export function NewShortUrlEditor() {
  const onSubmit = (data: EditShortUrlRequest) => {
    console.log(data);
  };

  return <ShortUrlForm onSubmit={onSubmit} />;
}
