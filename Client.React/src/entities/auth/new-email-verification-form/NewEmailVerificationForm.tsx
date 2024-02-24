
import { NewEmailVerificationRequest, newEmailVerificationRequest } from "@/lib/dto/auth/new-email-verification.request";
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

interface NewEmailVerificationFormProps {
  data?: NewEmailVerificationRequest;
  isLoading?: boolean;
  onSubmit: (data: NewEmailVerificationRequest) => void;
}

export function NewEmailVerificationForm({
  data,
  isLoading,
  onSubmit,
}: NewEmailVerificationFormProps) {
  const form = useForm<NewEmailVerificationRequest>({
    resolver: zodResolver(newEmailVerificationRequest),
    defaultValues: {
      email: "",
    },
    values: data,
  });

  return (
    <Form {...form}>
      <form className="space-y-5" onSubmit={form.handleSubmit(onSubmit)}>
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input type="email" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button loading={isLoading} type="submit" className="w-full">
          Request new verification
        </Button>
      </form>
    </Form>
  );
}
