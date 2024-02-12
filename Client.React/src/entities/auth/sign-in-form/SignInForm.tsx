import { SignInDto, signInDto } from "@/lib/dto/auth/sign-in.dto";
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
import { useState } from "react";
import { useForm } from "react-hook-form";

interface SignInFormProps {
  isLoading?: boolean;
  onSubmit: (data: SignInDto) => void;
}

export function SignInForm({ isLoading, onSubmit }: SignInFormProps) {
  const [show, setShow] = useState<boolean>(false);

  const form = useForm<SignInDto>({
    resolver: zodResolver(signInDto),
    defaultValues: {
      email: "",
      password: "",
    },
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
          name="password"
          render={({ field }) => (
            <FormItem>
              <div className="flex justify-between items-center">
                <FormLabel>Password</FormLabel>
                <span
                  className="text-sm font-medium hover:underline hover:cursor-pointer underline-offset-2"
                  onClick={() => setShow((prev) => !prev)}
                >
                  {show ? "Hide" : "Show"}
                </span>
              </div>
              <FormControl>
                <Input type={show ? "text" : "password"} {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <div className="text-end">
          <a
            href="/auth/recovery"
            className="font-semibold underline underline-offset-2"
          >
            Forgot Password?
          </a>
        </div>
        <Button className="w-full" loading={isLoading}>
          Sign in
        </Button>
      </form>
    </Form>
  );
}
