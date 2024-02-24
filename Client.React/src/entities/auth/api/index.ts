export const authKeys = {
  auth: {
    root: ["auth"],
  },
  mutations: {
    signUpLocal: () => [...authKeys.auth.root, "sign-up-local"],
    signInLocal: () => [...authKeys.auth.root, "sign-in-local"],
    verifyEmail: () => [...authKeys.auth.root, "verify-email"],
    newEmailVerification: () => [...authKeys.auth.root,"new-email-verification"],
    refreshToken: () => [...authKeys.auth.root, "refresh-token"],
    signOut: () => [...authKeys.auth.root, "sign-out"],
  },
};
