import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function isNullOrEmpty(value?: string | null): boolean {
  return !(value !== undefined && value !== null && value.length > 0);
}

export function writeToClipboard(value: string) {
  navigator.clipboard.writeText(value);
}
