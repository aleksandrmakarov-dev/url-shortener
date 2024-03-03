import { type ClassValue, clsx } from "clsx";
import moment from "moment";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function LocalToUTC(value?: Date | string | null) {
  return value ? moment(value).utc().format("YYYY-MM-DDTkk:mm") : undefined;
}

export function UTCToLocal(value?: Date | string | null, format?: string) {
  return value
    ? moment
        .utc(value)
        .local()
        .format(format ?? "YYYY-MM-DDTkk:mm")
    : "";
}

export function writeToClipboard(value: string) {
  navigator.clipboard.writeText(value);
}

export const Role = {
  Admin: "Admin",
  User: "User",
};
