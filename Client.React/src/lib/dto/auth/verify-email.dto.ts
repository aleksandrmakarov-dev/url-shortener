import { z } from "zod";

export const verifyEmailDto = z.object({
  email: z.string().email(),
  token: z.string().min(0),
});

export type VerifyEmailDto = z.infer<typeof verifyEmailDto>;
