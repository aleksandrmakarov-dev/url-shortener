import { z } from "zod";

export const signInDto = z.object({
  email: z.string().email(),
  password: z.string().min(6),
});

export type SignInDto = z.infer<typeof signInDto>;