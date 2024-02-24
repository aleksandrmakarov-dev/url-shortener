import { z } from "zod";

export const newEmailVerificationRequest = z.object({
  email: z.string().email(),
});

export type NewEmailVerificationRequest = z.infer<typeof newEmailVerificationRequest>;
