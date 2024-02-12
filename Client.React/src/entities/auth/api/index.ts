export const authKeys = {
  auth: {
    root: ["auth"],
  },
  mutations: {
    signUpLocal: () => [...authKeys.auth.root, "sign-up-local"],
    signInLocal: () => [...authKeys.auth.root, "sign-in-local"],
    verifyEmail: () => [...authKeys.auth.root, "verify-email"],
    refreshToken: () => [...authKeys.auth.root, "refresh-token"],
    signOut: () => [...authKeys.auth.root, "sign-out"],
  },
};
