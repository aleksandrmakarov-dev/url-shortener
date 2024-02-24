import { z } from "zod";

export const editShortUrlRequest = z.object({
  original: z.string().url().min(1),
  customAlias: z
    .string()
    .regex(/^[A-Za-z0-9_-]*$/, "contains invalid characters")
    .max(16)
    .optional(),
  userId: z.string().optional(),
});

export type EditShortUrlRequest = z.infer<typeof editShortUrlRequest>;
