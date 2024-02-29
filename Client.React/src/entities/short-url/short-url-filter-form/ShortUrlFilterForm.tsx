import {
  FilterShortUrlRequest,
  filterShortUrlRequest,
} from "@/lib/dto/short-url/filter-short-url.request";
import { Button } from "@/shared/ui/button";
import { Form, FormControl, FormField, FormItem } from "@/shared/ui/form";
import { Input } from "@/shared/ui/input";
import { zodResolver } from "@hookform/resolvers/zod";
import { Search } from "lucide-react";
import { useForm } from "react-hook-form";

interface ShortUrlFilterFormProps {
  onSubmit: (data: FilterShortUrlRequest) => void;
  filter?: FilterShortUrlRequest;
  isLoading?: boolean;
}

export function ShortUrlFilterForm({
  filter,
  onSubmit,
  isLoading,
}: ShortUrlFilterFormProps) {
  const form = useForm<FilterShortUrlRequest>({
    resolver: zodResolver(filterShortUrlRequest),
    defaultValues: {
      query: "",
    },
    values: filter,
  });

  return (
    <Form {...form}>
      <form className="flex gap-x-3" onSubmit={form.handleSubmit(onSubmit)}>
        <FormField
          control={form.control}
          name="query"
          render={({ field }) => (
            <FormItem className="w-full">
              <FormControl>
                <Input type="search" placeholder="Search" {...field} />
              </FormControl>
            </FormItem>
          )}
        />
        <Button className="shrink-0" loading={isLoading} size="icon">
          <Search className="w-4 h-4" />
        </Button>
      </form>
    </Form>
  );
}
