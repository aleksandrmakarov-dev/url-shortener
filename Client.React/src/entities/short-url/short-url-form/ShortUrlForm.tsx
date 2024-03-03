import { useSession } from "@/context/session-provider/SessionProvider";
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
  btnLabel?: string;
}

export function ShortUrlForm({
  shortUrl,
  onSubmit,
  isLoading,
  btnLabel,
}: ShortUrlFormProps) {
  const { session } = useSession();

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
        <div className="flex flex-col sm:grid grid-cols-5 gap-x-3">
          <div className="col-span-3">
            <FormField
              control={form.control}
              name="customAlias"
              render={({ field: { disabled, ...other } }) => (
                <FormItem>
                  <FormLabel>Alias (optional)</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="example: favorite-link"
                      disabled={disabled || !session}
                      {...other}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
          <div className="col-span-2">
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
                      disabled={disabled || !session}
                      {...other}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
        </div>
        <div className="flex justify-end">
          <Button className="w-full sm:w-auto" loading={isLoading}>
            {btnLabel ?? "Shorten URL"}
          </Button>
        </div>
      </form>
    </Form>
  );
}
