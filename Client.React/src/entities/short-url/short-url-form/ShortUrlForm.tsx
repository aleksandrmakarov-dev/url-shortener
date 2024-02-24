import {
  EditShortUrlRequest,
  editShortUrlRequest,
} from "@/lib/dto/short-url/edit-short-url.request";
import { Button } from "@/shared/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/shared/ui/form";
import { Input } from "@/shared/ui/input";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

interface ShortUrlFormProps {
  shortUrl?: EditShortUrlRequest;
  onSubmit: (data: EditShortUrlRequest) => void;
  isLoading?: boolean;
}

export function ShortUrlForm({
  shortUrl,
  onSubmit,
  isLoading,
}: ShortUrlFormProps) {
  const form = useForm<EditShortUrlRequest>({
    resolver: zodResolver(editShortUrlRequest),
    defaultValues: {
      original: "",
      customAlias: "",
    },
    values: shortUrl,
  });

  return (
    <Form {...form}>
      <form className="space-y-5" onSubmit={form.handleSubmit(onSubmit)}>
        <FormField
          control={form.control}
          name="original"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Original URL</FormLabel>
              <FormControl>
                <Input
                  placeholder="Example: http://super-long-link.com/shorten-it"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <div className="grid grid-cols-[1fr_auto_1fr_1fr] gap-x-3">
          <FormItem>
            <FormLabel>Domain</FormLabel>
            <Input value="shrt.com" disabled />
          </FormItem>
          <span className="py-2.5 self-end font-semibold">/</span>
          <div className="col-span-2">
            <FormField
              control={form.control}
              name="customAlias"
              render={({ field: { disabled, ...other } }) => (
                <FormItem>
                  <FormLabel>Alias (optional)</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="example: favorite-link"
                      disabled={disabled || !shortUrl?.userId}
                      {...other}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
        </div>
        <div>
          <Button loading={isLoading}>Get my short URL</Button>
        </div>
      </form>
    </Form>
  );
}
