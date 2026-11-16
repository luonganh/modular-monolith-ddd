// src/modules/user-access/store/slices/authSlice.ts
import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import type { IdTokenClaims, User as OidcUser } from 'oidc-client-ts';

export interface User {
  id: string;
  name: string;
  email: string;
  profile: IdTokenClaims;
  access_token?: string;
  expired?: boolean;
}

// Maps the oidc-client-ts User class instance to the plain object kept in the store.
export const toAuthUser = (user: OidcUser): User => ({
  id: user.profile.sub,
  name: user.profile.name ?? user.profile.preferred_username ?? '',
  email: user.profile.email ?? '',
  profile: user.profile,
  access_token: user.access_token,
  expired: user.expired,
});

export interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
  isInitialized: boolean;
}

const initialState: AuthState = {
  user: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,
  isInitialized: false,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    // Synchronous actions
    setUser: (state, action: PayloadAction<User | null>) => {
      state.user = action.payload;
      state.isAuthenticated = !!action.payload;
    },
    setLoading: (state, action: PayloadAction<boolean>) => {
      state.isLoading = action.payload;
    },
    setError: (state, action: PayloadAction<string | null>) => {
      state.error = action.payload;
    },
    setInitialized: (state, action: PayloadAction<boolean>) => {
      state.isInitialized = action.payload;
    },
    clearAuth: (state) => {
      state.user = null;
      state.isAuthenticated = false;
      state.error = null;
    },
  },
});

export const { setUser, setLoading, setError, setInitialized, clearAuth } = authSlice.actions;
export default authSlice.reducer;