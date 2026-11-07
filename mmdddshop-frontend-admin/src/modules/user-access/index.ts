// src/modules/user-access/index.ts

// Services exports
export {
  login,
  logout,
  handleCallback,
  getAccessToken,
  isAuthenticated,
  getUser,
  getUserId,
  getUsername,
  userManager,
  authService
} from './services/AuthenticationService';

// Components exports (from pages/)
export { default as RequireAuth } from './pages/RequireAuth';
export { default as Callback } from './pages/Callback';

// Store exports
export * from './store';

// Types exports
// export * from './types';