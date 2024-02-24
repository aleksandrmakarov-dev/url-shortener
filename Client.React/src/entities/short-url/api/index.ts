export const shortUrlsKeys = {
  shortUrls: {
    root: ["short-urls"],
    alias: (alias: string) => [
      ...shortUrlsKeys.shortUrls.root,
      "by-alias",
      alias,
    ],
  },
  mutations: {},
};
