import { z } from "zod";

export const filterShortUrlRequest = z.object({
  query: z.string().optional(),
});

export type FilterShortUrlRequest = z.infer<typeof filterShortUrlRequest>;
