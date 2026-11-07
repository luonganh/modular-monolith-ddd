// src/modules/user-access/store/selectors/authSelectors.ts
import { createSelector } from '@reduxjs/toolkit';

// Root state type
interface RootState {
  auth: any; 
}

const selectAuthState = (state: RootState) => state.auth;

export const selectUser = createSelector(
  selectAuthState,
  (auth) => auth.user
);

export const selectIsAuthenticated = createSelector(
  selectAuthState,
  (auth) => auth.isAuthenticated
);

export const selectIsLoading = createSelector(
  selectAuthState,
  (auth) => auth.isLoading
);

export const selectAuthError = createSelector(
  selectAuthState,
  (auth) => auth.error
);

export const selectIsInitialized = createSelector(
  selectAuthState,
  (auth) => auth.isInitialized
);

export const selectUserProfile = createSelector(
  selectUser,
  (user) => user?.profile
);

export const selectAccessToken = createSelector(
  selectUser,
  (user) => user?.access_token
);