import { SignUpRequest, signUpRequest } from "@/lib/dto/auth/sign-up.request";
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

interface SignUpFormProps {
  isLoading?: boolean;
  onSubmit: (data: SignUpRequest) => void;
}

export function SignUpForm({ isLoading, onSubmit }: SignUpFormProps) {
  const [show, setShow] = useState<boolean>(false);

  const form = useForm<SignUpRequest>({
    resolver: zodResolver(signUpRequest),
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
              <div className="flex justify-between">
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
        <Button loading={isLoading} type="submit" className="w-full">
          Create my account
        </Button>
      </form>
    </Form>
  );
}
