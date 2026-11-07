import { ErrorResponse, User, UserManager, WebStorageStateStore } from 'oidc-client-ts';
import { ApplicationSettings } from '../../../configuration/application-settings';

// OIDC Client configuration - Functional approach
const createUserManager = () => {
 
  return new UserManager({
    authority: ApplicationSettings.AUTHORITY,
    client_id: ApplicationSettings.CLIENT_ID,
    redirect_uri: ApplicationSettings.REDIRECT_URI,
    response_type: 'code',
    scope: ApplicationSettings.SCOPE,

    // Store tokens (identity token, access token, refresh token) in localStorage after authenticate successfully
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    
    //// Add state management configuration
    stateStore: new WebStorageStateStore({ store: window.localStorage }),
   
    // Add automatic silent renew token
    automaticSilentRenew: false,
    accessTokenExpiringNotificationTimeInSeconds: 60,
    revokeTokensOnSignout: true,
    // Add load user info
    loadUserInfo: true,

    //The time when OIDC state is considered expired
    staleStateAgeInSeconds: 600,  // 10 minutes (default: 300 seconds)

    requestTimeoutInSeconds: 60,  // Timeout for HTTPrequests
   
  });
};

// Functional exports - Modern approach
export const userManager = createUserManager();

// Pure functions for authentication
export const login = async (): Promise<void> => {
  try {    
        
    const returnUrl = window.location.href;    
    console.log('Return URL: ', returnUrl);
    console.log('Starting OIDC login flow...');
    console.log('Authority (AUTH):', ApplicationSettings.AUTHORITY);
    console.log('Client ID:', ApplicationSettings.CLIENT_ID);
    console.log('Redirect URI:', ApplicationSettings.REDIRECT_URI);   
    
    // oidc-client-ts automatically creates a new entry in localStorage for the state    
    await userManager.signinRedirect({
      state: { returnUrl }        
    });
   
  } catch (error) {
    console.error('Login error:', error);
    throw error;
  }
};

let refreshInFlight: Promise<User | null> | null = null;

export const refreshAccessToken = async (rejectedToken?: string): Promise<string | null> => {
  if (!refreshInFlight) {
    refreshInFlight = (async () => {
      const user = await userManager.getUser();
      if (rejectedToken && user && user.access_token !== rejectedToken && !user.expired) return user;
      if (!user?.refresh_token) return null;
      try {
        return await userManager.signinSilent();
      } catch (error) {
        if (error instanceof ErrorResponse && error.error === 'invalid_grant') {
          await userManager.removeUser();
          return null;
        }
        throw error;
      }
    })();
  }
  try {
    const user = await refreshInFlight;
    return user && !user.expired ? user.access_token : null;
  } finally {
    refreshInFlight = null;
  }
};

// All renewal paths share a promise to avoid concurrent refresh token rotation.
userManager.events.addAccessTokenExpiring(() => {
  void refreshAccessToken().catch(() => {
    // Preserve the session on transient failures; foreground requests retry.
  });
});

export const handleCallback = async (url: string): Promise<{ user: User; returnUrl: string }> => {
  const user = await userManager.signinRedirectCallback(url);
  window.history.replaceState({}, document.title, '/callback');
  const state = user.state as { returnUrl?: string } | undefined;
  return { user, returnUrl: state?.returnUrl ?? '/' };
};

export const logout = async (): Promise<void> => {
  if (refreshInFlight) await refreshInFlight.catch(() => null);
  await userManager.signoutRedirect();
};

export const getAccessToken = async (): Promise<string | null> => {
  const user = await userManager.getUser();
  if (!user) return null;
  if ((user.expires_in ?? 0) <= 60) {
    const token = await refreshAccessToken();
    const current = await userManager.getUser();
    return token ?? (current && !current.expired ? current.access_token : null);
  }
  return user.access_token;
};

export const isAuthenticated = async (): Promise<boolean> => {
  const user = await userManager.getUser();
  return user != null && !user.expired;
};

export const getUser = async (): Promise<any> => {
  return await userManager.getUser();
};

export const getUserId = async (): Promise<string | null> => {
  const user = await userManager.getUser();
  return user?.profile?.sub ?? null;
};

export const getUsername = async (): Promise<string | null> => {
  const user = await userManager.getUser();
  return user?.profile?.name ?? user?.profile?.preferred_username ?? null;
};

export const authService = {
  login,
  handleCallback,
  logout,
  getAccessToken, refreshAccessToken,
  isAuthenticated,
  getUser,
  getUserId,
  getUsername 
} as const;
