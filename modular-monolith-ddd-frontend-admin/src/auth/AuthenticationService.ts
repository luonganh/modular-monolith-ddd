import { UserManager, WebStorageStateStore } from 'oidc-client-ts';
import { ApplicationSettings } from '../configuration/application-settings';

// OIDC Client configuration - Functional approach
const createUserManager = () => {
 
  return new UserManager({
    authority: ApplicationSettings.AUTHORITY,
    client_id: ApplicationSettings.CLIENT_ID,
    redirect_uri: ApplicationSettings.REDIRECT_URI,
    response_type: 'code',
    scope: ApplicationSettings.SCOPE,

    // For get tokens (identity token, access token, refresh token) in localStorage after authenticate successfully
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    
    //// Add state management configuration
    stateStore: new WebStorageStateStore({ store: window.localStorage }),
   
    // Add automatic silent renew token
    automaticSilentRenew: false,
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

export const refreshUserWithFullScope = async (): Promise<void> => {
  try {
    // Get current user
    const user = await userManager.getUser();
    if (!user) return;
    
    // Create new UserManager with full scope
    const fullScopeUserManager = new UserManager({
      authority: ApplicationSettings.AUTHORITY,
      client_id: ApplicationSettings.CLIENT_ID,
      redirect_uri: ApplicationSettings.REDIRECT_URI,
      response_type: 'code',
      scope: ApplicationSettings.getScope(true), // Full scope
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      stateStore: new WebStorageStateStore({ store: window.localStorage }),
    });
    
    // Silent refresh with full scope
    await fullScopeUserManager.signinSilent();
    
    console.log('User refreshed with full scope');
  } catch (error) {
    console.error('Failed to refresh with full scope:', error);
  }
};

export const handleCallback = async (fullCallbackUrl: string): Promise<{ user: any, returnUrl: string}> => {
  try {   
    const startTime = new Date();
    
     // Parse URL để lấy state
    const url = new URL(fullCallbackUrl);
    const state = url.searchParams.get('state');
    console.log('URL:', url);
    console.log('State từ URL:', state);
                 
    const oidcKeys = Object.keys(localStorage).filter(key => key.startsWith('oidc.'));
    console.log('All OIDC keys:', oidcKeys);
    
    oidcKeys.forEach(key => {
      const entry = localStorage.getItem(key);
      if (entry) {
        try {
          const data = JSON.parse(entry);
          console.log(`Entry ${key}:`, {
            created: data.created,
            createdTime: new Date(data.created * 1000).toISOString(),
            year: new Date(data.created * 1000).getFullYear()
          });
        } catch (error) {
          console.error('Error parsing entry:', error);
        }
      }
    });

    let storedState = null;
    let oidcEntry = null;
    if (oidcKeys.length > 0) {
      const matchingKey = oidcKeys.find(key => key.includes(state || ''));
      if (matchingKey) {
        storedState = matchingKey.replace('oidc.', '');
        oidcEntry = localStorage.getItem(matchingKey);
        console.log('Found matching OIDC key:', matchingKey);
        console.log('OIDC entry data:', oidcEntry);        
      
         // Parse OIDC entry to take timestamp
         try {
          const oidcData = JSON.parse(oidcEntry || '{}');

          console.log('Code verifier:', oidcData.code_verifier);
          console.log('Code challenge:', oidcData.code_challenge);          

          if (oidcData.created) {

            // oidcData.created is in seconds, so we need to convert it to milliseconds
            // oidcData.created * 1000 to get the milliseconds
            const createdLocalTime = new Date(oidcData.created * 1000);              
            const timeDiff = startTime.getTime() - createdLocalTime.getTime();
            console.log('OIDC entry created at UTC Time:', createdLocalTime.toISOString());
                        
            if (timeDiff > 300000) { 
              console.warn('OIDC entry is older than 5 minutes');
            }
            if (timeDiff > 600000) {
              console.error('OIDC entry is older than 10 minutes - likely expired');
            }
          }
        } catch (parseError) {
          console.error('Error parsing OIDC entry:', parseError);
        }
      }         
    }    
    
    if (state !== storedState) {
      console.error('STATE MISMATCH!');
      console.error('URL state:', state);
      console.error('Stored state:', storedState);
      throw new Error('State mismatch detected');
    }
  
    // Try to get user from callback
    let user;
    try {
      console.log('Attempting signinRedirectCallback...');
      const callbackStartTime = new Date();
      console.log('Url before exchange the authorizationcode to receive tokens: ', window.location.href);
    
      // ==== OIDC DEBUG before signinRedirectCallback ====
      console.group('OIDC before token exchange');

      const href = window.location.href;
      console.log('href:', href);

      const url = new URL(href);
      const code = url.searchParams.get('code');
      const state = url.searchParams.get('state');
      const session_state = url.searchParams.get('session_state');

      console.log('code exists:', !!code, 'length:', code?.length);
      console.log('state:', state);
      console.log('session_state:', session_state);

      // OIDC client settings snapshot
      console.log('authority:', userManager.settings.authority);
      console.log('client_id:', userManager.settings.client_id);
      console.log('redirect_uri (client):', userManager.settings.redirect_uri);
      console.log('response_type:', userManager.settings.response_type);
      console.log('scope:', userManager.settings.scope);

      // State in storage (find the entry that matches this state)
      const oidcKeys = Object.keys(localStorage).filter(k => k.startsWith('oidc.'));
      console.log('oidc.* keys:', oidcKeys);

      const matchingKey = oidcKeys.find(k => k.includes(state || ''));
      console.log('matchingKey:', matchingKey);

      if (matchingKey) {
        try {
          const raw = localStorage.getItem(matchingKey)!;
          const entry = JSON.parse(raw);
          console.log('entry.request_type:', entry.request_type);       // should be 'si:r'
          console.log('entry.redirect_uri (stored):', entry.redirect_uri);
          console.log('entry.authority:', entry.authority);
          console.log('entry.client_id:', entry.client_id);
          console.log('entry.id (state id):', entry.id);
          if (entry.created) {
            const created = new Date(entry.created * 1000);
            const ageSec = Math.round((Date.now() - created.getTime()) / 1000);
            console.log('entry.created:', created.toISOString(), 'ageSec:', ageSec);
          }
          // PKCE fields (tùy bản lib/flow, có thể có hoặc không)
          console.log('has code_verifier:', !!entry.code_verifier);
          console.log('code_challenge_method:', entry.code_challenge_method);
        } catch (e) {
          console.warn('parse matching entry failed:', e);
        }
      } else {
        console.warn('No matching oidc.* entry for state');
      }

      console.groupEnd();
      // ==== END DEBUG ====

      // Whereas exchange authorization code to receive tokens
      //user = await userManager.signinRedirectCallback(window.location.href);
      user = await userManager.signinRedirectCallback(fullCallbackUrl);
      console.log('Error by clean up URL before sign in redirect callback! /connect/token');
      // Clean up URL after parse parameters as code, state, issuer...
      window.history.replaceState({}, document.title, '/callback');

      const callbackEndTime = new Date();
      const callbackDuration = callbackEndTime.getTime() - callbackStartTime.getTime();
      console.log('signinRedirectCallback completed in:', callbackDuration, 'ms');
      console.log('User from callback:', user);
      
    } catch (stateError: unknown) {
      console.error('State error, trying alternative approach:', stateError);
     
      if (stateError instanceof Error) {
        console.error('Error details:', {
          message: stateError.message,
          name: stateError.name,
          stack: stateError.stack
        });

        if (stateError.message.includes('authorization code is no longer valid') 
          || stateError.message.includes('invalid_grant')) {
            console.error('AUTHORIZATION CODE EXPIRED!');
            
            if (oidcEntry) {
              try {
                const oidcData = JSON.parse(oidcEntry);
                console.log('OIDC entry data:', oidcData);                

                if (oidcData.created) {
                  const createdTime = new Date(oidcData.created * 1000);
                  const currentTime = new Date();
                  console.error('OIDC entry created at:', createdTime.toISOString());
                  console.error('Current time:', currentTime.toISOString());
                  console.error('Total time elapsed:', currentTime.getTime() - createdTime.getTime(), 'ms');
                }
              } catch (parseError) {
                console.error('Could not parse OIDC entry for timing analysis');
              }
            }
          }
        } else {
          console.error('Unknown error type:', typeof stateError, stateError);
        }

      // If state error, try to get user directly
      user = await userManager.getUser();
      console.log('User from getUser:', user);
      
      if (!user) {
        throw new Error('No user found after callback');
      }
    }
    
    // After successful login, refresh with full scope
    await refreshUserWithFullScope();

    // Read back the returnUrl we set during login
    const returnUrl = (user?.state as any)?.returnUrl ?? '/'; 
    console.log('Return URL:', returnUrl);
        
    return { user, returnUrl };
  } catch (error) {
    console.error('Error in handleCallback:', error);
    throw error;
  }
};

export const logout = async (): Promise<void> => {
  await userManager.signoutRedirect();
};

export const getAccessToken = async (): Promise<string | null> => {
  const user = await userManager.getUser();
  return user?.access_token ?? null;
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
  getAccessToken,
  isAuthenticated,
  getUser,
  getUserId,
  getUsername 
} as const;