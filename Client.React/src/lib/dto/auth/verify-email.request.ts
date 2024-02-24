import { z } from "zod";

export const verifyEmailRequest = z.object({
  email: z.string().email(),
  token: z.string().min(0),
});

export type VerifyEmailRequest = z.infer<typeof verifyEmailRequest>;
