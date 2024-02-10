import { SignUpDto, signUpDto } from "@/lib/dto/auth/sign-up.dto";
import {
  VerifyEmailDto,
  verifyEmailDto,
} from "@/lib/dto/auth/verify-email.dto";
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

interface VerifyEmailFormProps {
  data?: VerifyEmailDto;
  isLoading?: boolean;
  onSubmit: (data: VerifyEmailDto) => void;
}

export function VerifyEmailForm({
  data,
  isLoading,
  onSubmit,
}: VerifyEmailFormProps) {
  const form = useForm<VerifyEmailDto>({
    resolver: zodResolver(verifyEmailDto),
    defaultValues: {
      email: "",
      token: "",
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
        <FormField
          control={form.control}
          name="token"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Token</FormLabel>
              <FormControl>
                <Input type="text" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button loading={isLoading} type="submit" className="w-full">
          Verify my account
        </Button>
      </form>
    </Form>
  );
}
