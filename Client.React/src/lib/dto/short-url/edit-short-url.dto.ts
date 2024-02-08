import { z } from "zod";

export const editShortUrlDto = z.object({
  redirect: z.string().url().min(1),
  alias: z.string().optional(),
});

export type EditShortUrlDto = z.infer<typeof editShortUrlDto>;
