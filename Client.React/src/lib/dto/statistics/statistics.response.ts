export type KeyValuePair = {
  key: string;
  value: number;
};

export type StatisticsResponse = {
  navigationCount: number;
  countries: KeyValuePair[];
  platforms: KeyValuePair[];
  browsers: KeyValuePair[];
};
