import { EditShortUrlRequest, editShortUrlRequest } from "@/lib/dto/short-url/edit-short-url.request";
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
  onSubmit: (data: EditShortUrlRequest) => void;
}

export function ShortUrlForm({ onSubmit }: ShortUrlFormProps) {
  const form = useForm<EditShortUrlRequest>({
    resolver: zodResolver(editShortUrlRequest),
    defaultValues: {
      redirect: "",
      customAlias: "",
    },
  });

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <FormField
          control={form.control}
          name="redirect"
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
        <div className="grid grid-cols-[1fr_auto_1fr_1fr] gap-x-3 my-5">
          <FormItem>
            <FormLabel>Domain</FormLabel>
            <Input value="shrt.com" disabled />
          </FormItem>
          <span className="py-2.5 self-end font-semibold">/</span>
          <div className="col-span-2">
            <FormField
              control={form.control}
              name="customAlias"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Alias (optional)</FormLabel>
                  <FormControl>
                    <Input placeholder="example: favorite-link" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
        </div>
        <div>
          <Button>Get my URL</Button>
        </div>
      </form>
    </Form>
  );
}
