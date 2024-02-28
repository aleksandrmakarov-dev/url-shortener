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
        <div className="grid grid-cols-3 gap-x-3">
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
          <FormField
            control={form.control}
            name="expiresAt"
            render={({ field: { disabled, ...other } }) => (
              <FormItem>
                <FormLabel>Expires At (Optional)</FormLabel>
                <FormControl>
                  <Input
                    type="datetime-local"
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
        <div>
          <Button loading={isLoading}>Get my short URL</Button>
        </div>
      </form>
    </Form>
  );
}
