export type ShortUrlResponse = {
  id: string;
  original: string;
  alias: string;
  domain: string;
  userId?: string;
  expiresAt?: Date;
  createdAt: Date;
};
