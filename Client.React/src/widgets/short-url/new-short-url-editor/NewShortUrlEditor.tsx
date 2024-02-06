import { ShortUrlForm } from "@/entities/short-url";
import { EditShortUrlDto } from "@/lib/dto/short-url/edit-short-url.dto";

export function NewShortUrlEditor() {
  const onSubmit = (data: EditShortUrlDto) => {
    console.log(data);
  };

  return <ShortUrlForm onSubmit={onSubmit} />;
}
