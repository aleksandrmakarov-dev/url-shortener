import { z } from "zod";

export const editShortUrlRequest = z.object({
  redirect: z.string().url().min(1),
  customAlias: z.string().optional(),
});

export type EditShortUrlRequest = z.infer<typeof editShortUrlRequest>;
