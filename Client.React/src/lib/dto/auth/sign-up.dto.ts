import { z } from "zod";

export const signUpDto = z.object({
  email: z.string().email(),
  password: z.string().min(6),
});

export type SignUpDto = z.infer<typeof signUpDto>;
