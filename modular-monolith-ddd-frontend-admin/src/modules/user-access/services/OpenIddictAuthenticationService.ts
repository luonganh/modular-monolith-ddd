import { UserManager, WebStorageStateStore } from 'oidc-client-ts';
import { ApplicationSettings } from '../../../configuration/application-settings';

// OIDC Client configuration - Functional approach
const createUserManager = () => {
 
  return new UserManager({
    authority: ApplicationSettings.AUTHORITY,
    client_id: ApplicationSettings.CLIENT_ID,
    redirect_uri: ApplicationSettings.REDIRECT_URI,
    response_type: 'code',
    scope: ApplicationSettings.SCOPE,
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    // Add state management configuration
    stateStore: new WebStorageStateStore({ store: window.localStorage }),
   
    // Add automatic silent renew token
    automaticSilentRenew: false,
    // Add load user info
    loadUserInfo: true,

    //The time when OIDC state is considered expired
    staleStateAgeInSeconds: 600,  // 10 minutes (default: 300 seconds)

    requestTimeoutInSeconds: 60,  // Timeout for HTTPrequests

    // // Add revoke tokens on signout
    // revokeTokensOnSignout: true,
    // // Add include id token in silent renew
    // includeIdTokenInSilentRenew: true,

    // Let OIDC client auto-discover endpoints instead of hardcoding
    // metadata: {
    //   issuer: AUTH,
    //   authorization_endpoint: `${AUTH}/connect/authorize`,
    //   token_endpoint: `${AUTH}/connect/token`,
    //   userinfo_endpoint: `${AUTH}/connect/userinfo`,
    //   end_session_endpoint: `${AUTH}/connect/endsession`
    // }
  });
};

// Functional exports - Modern approach
export const userManager = createUserManager();

// Pure functions for authentication
export const login = async (): Promise<void> => {
  try {

    //const req = await userManager.createSigninRequest({ state: { returnUrl } });

    // // Phân tích URL authorize trước khi điều hướng
    // const authUrl = new URL(req.url);
    // console.group('Authorize URL (pre-redirect)');
    // console.log('href:', authUrl.href);
    // console.log('response_type:', authUrl.searchParams.get('response_type'));
    // console.log('client_id:', authUrl.searchParams.get('client_id'));
    // console.log('redirect_uri:', authUrl.searchParams.get('redirect_uri'));
    // console.log('scope:', authUrl.searchParams.get('scope'));
    // console.log('state (id):', (req as any).state?.id);
    // console.log('code_challenge:', authUrl.searchParams.get('code_challenge'));
    // console.log('code_challenge_method:', authUrl.searchParams.get('code_challenge_method')); // kỳ vọng 'S256'
    // console.groupEnd();

    // // (tuỳ chọn) kiểm tra entry trong storage theo state id để thấy code_verifier
    // try {
    //   const stateId = (req as any).state?.id;
    //   if (stateId) {
    //     const entryKey = `oidc.${stateId}`;
    //     const raw = localStorage.getItem(entryKey);
    //     console.log('oidc entry key:', entryKey, 'exists:', !!raw);
    //     if (raw) {
    //       const entry = JSON.parse(raw);
    //       console.log('entry.code_verifier exists:', !!entry.code_verifier);
    //       console.log('entry.created:', new Date(entry.created * 1000).toISOString());
    //     }
    //   }
    // } catch {}

    // // Điều hướng thủ công (tương đương signinRedirect)
    // window.location.assign(req.url);
    
    // const originalAssign = window.location.assign.bind(window.location);
    // const originalReplace = window.location.replace.bind(window.location);

    // (window.location as any).assign = (u: string | URL) => {
    //   const href = u.toString();
    //   const url = new URL(href);
    //   console.group('Redirect (assign) to authorize');
    //   console.log('href:', href);
    //   console.log('response_type:', url.searchParams.get('response_type'));
    //   console.log('client_id:', url.searchParams.get('client_id'));
    //   console.log('redirect_uri:', url.searchParams.get('redirect_uri'));
    //   console.log('scope:', url.searchParams.get('scope'));
    //   console.log('state:', url.searchParams.get('state'));
    //   console.log('code_challenge:', url.searchParams.get('code_challenge'));
    //   console.log('code_challenge_method:', url.searchParams.get('code_challenge_method'));
    //   console.groupEnd();
    //   return originalAssign(u);
    // };

    // (window.location as any).replace = (u: string | URL) => {
    //   const href = u.toString();
    //   const url = new URL(href);
    //   console.group('Redirect (replace) to authorize');
    //   console.log('href:', href);
    //   console.log('code_challenge:', url.searchParams.get('code_challenge'));
    //   console.log('code_challenge_method:', url.searchParams.get('code_challenge_method'));
    //   console.groupEnd();
    //   return originalReplace(u);
    // };
    
    const returnUrl = window.location.href;    
    console.log('Return URL: ', returnUrl);
    console.log('Starting OIDC login flow...');
    console.log('Authority (AUTH):', ApplicationSettings.AUTHORITY);
    console.log('Client ID:', ApplicationSettings.CLIENT_ID);
    console.log('Redirect URI:', ApplicationSettings.REDIRECT_URI);
    // const codeVerifier = generateCodeVerifier();
    // const codeChallenge = await generateCodeChallenge(codeVerifier);    
    // console.log('Generated code verifier:', codeVerifier);
    // console.log('Generated code challenge:', codeChallenge);
    // // Store code verifier into localStorage to use later
    // localStorage.setItem('oidc_code_verifier', codeVerifier);
    
    await userManager.signinRedirect({
      state: { returnUrl }     
    });
   
  } catch (error) {
    console.error('Login error:', error);
    throw error;
  }
};

function generateCodeVerifier(): string {
  const array = new Uint8Array(32);
  crypto.getRandomValues(array);
  return btoa(String.fromCharCode.apply(null, Array.from(array)))
    .replace(/\+/g, '-')
    .replace(/\//g, '_')
    .replace(/=/g, '');
}

async function generateCodeChallenge(verifier: string): Promise<string> {
  const encoder = new TextEncoder();
  const data = encoder.encode(verifier);
  const digest = await crypto.subtle.digest('SHA-256', data);
  const hashArray = Array.from(new Uint8Array(digest));
  const hashBase64 = btoa(String.fromCharCode.apply(null, hashArray));
  return hashBase64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
}

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
          // console.log('Code challenge method:', oidcData.code_challenge_method);
          // if (oidcData.code_verifier && oidcData.code_challenge) {
          //   const crypto = window.crypto || (window as any).msCrypto;
          //   const encoder = new TextEncoder();
          //   const data = encoder.encode(oidcData.code_verifier);
          //   const digest = await crypto.subtle.digest('SHA-256', data);
          //   const hashArray = Array.from(new Uint8Array(digest));
          //   const hashBase64 = btoa(String.fromCharCode.apply(null, hashArray));
          //   const codeChallenge = hashBase64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
            
          //   console.log('Calculated code challenge:', codeChallenge);
          //   console.log('Stored code challenge:', oidcData.code_challenge);
          //   console.log('PKCE Match:', codeChallenge === oidcData.code_challenge);
            
          //   // Nếu PKCE không match, đây là nguyên nhân lỗi
          //   if (codeChallenge !== oidcData.code_challenge) {
          //     console.error('PKCE MISMATCH! This is the root cause of the error.');
          //     throw new Error('PKCE validation failed: code challenge mismatch');
          //   }
          // }

          if (oidcData.created) {

            // oidcData.created is in seconds, so we need to convert it to milliseconds
            // oidcData.created * 1000 to get the milliseconds
            const createdLocalTime = new Date(oidcData.created * 1000);   
            //console.log('OIDC created local time:', createdLocalTime);
            const timeDiff = startTime.getTime() - createdLocalTime.getTime();
            console.log('OIDC entry created at UTC Time:', createdLocalTime.toISOString());
            // console.log('Time since OIDC entry created:', timeDiff, 'ms');
            // console.log('Time since OIDC entry created (seconds):', Math.round(timeDiff / 1000));
            
            // Cảnh báo nếu quá lâu
            if (timeDiff > 300000) { // 5 phút
              console.warn('OIDC entry is older than 5 minutes');
            }
            if (timeDiff > 600000) { // 10 phút
              console.error('OIDC entry is older than 10 minutes - likely expired');
            }
          }
        } catch (parseError) {
          console.error('Error parsing OIDC entry:', parseError);
        }


      }
      // storedState = oidcKeys[0].replace('oidc.', '');
      console.log('State from localStorage:', storedState);
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

    //       const st: any = userManager.settings.stateStore;
    // console.log(
    //   'StateStore uses:',
    //   st?.store === window.localStorage ? 'localStorage'
    //   : st?.store === window.sessionStorage ? 'sessionStorage'
    //   : st?.store
    // );
      

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
      debugCodeChallengePhases();
      // Go to /connect/token
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
      debugCodeChallengePhases();
      if (stateError instanceof Error) {
        console.error('Error details:', {
          message: stateError.message,
          name: stateError.name,
          stack: stateError.stack
        });

        if (stateError.message.includes('authorization code is no longer valid') 
          || stateError.message.includes('invalid_grant')) {
            console.error('AUTHORIZATION CODE EXPIRED!');

            //await checkServerTime();

            if (oidcEntry) {
              try {
                const oidcData = JSON.parse(oidcEntry);
                console.log('OIDC entry data:', oidcData);

                // console.log('Code verifier:', oidcData.code_verifier);
                // console.log('Code challenge:', oidcData.code_challenge);
                // console.log('Code challenge method:', oidcData.code_challenge_method);
                // Verify PKCE manually
                // if (oidcData.code_verifier && oidcData.code_challenge) {
                //   const crypto = window.crypto || (window as any).msCrypto;
                //   const encoder = new TextEncoder();
                //   const data = encoder.encode(oidcData.code_verifier);
                //   const digest = await crypto.subtle.digest('SHA-256', data);
                //   const hashArray = Array.from(new Uint8Array(digest));
                //   const hashBase64 = btoa(String.fromCharCode.apply(null, hashArray));
                //   const codeChallenge = hashBase64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
                  
                //   console.log('Calculated code challenge:', codeChallenge);
                //   console.log('Stored code challenge:', oidcData.code_challenge);
                //   console.log('PKCE Match:', codeChallenge === oidcData.code_challenge);
                // }
                
                // const storedCodeVerifier = localStorage.getItem('oidc_code_verifier');
                // console.log('Stored code verifier:', storedCodeVerifier);
                // if (storedCodeVerifier) {
                //   // Verify PKCE manually
                //   const crypto = window.crypto || (window as any).msCrypto;
                //   const encoder = new TextEncoder();
                //   const data = encoder.encode(storedCodeVerifier);
                //   const digest = await crypto.subtle.digest('SHA-256', data);
                //   const hashArray = Array.from(new Uint8Array(digest));
                //   const hashBase64 = btoa(String.fromCharCode.apply(null, hashArray));
                //   const codeChallenge = hashBase64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');                  
                //   console.log('Calculated code challenge:', codeChallenge);
                //   console.log('PKCE verification ready');                  
                //   // Clean up
                //   localStorage.removeItem('oidc_code_verifier');
                // }

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
    
    // Read back the returnUrl we set during login
    const returnUrl = (user?.state as any)?.returnUrl ?? '/'; 
    console.log('Return URL:', returnUrl);
    
    return { user, returnUrl };
  } catch (error) {
    console.error('Error in handleCallback:', error);
    throw error;
  }
};

// Debug server response
const debugServerResponse = async () => {
  try {
    const response = await fetch(`${ApplicationSettings.AUTHORITY}/.well-known/openid-configuration`,
      {
        method: 'GET'
      });
    const config = await response.json();
    console.log('Server config:', config);
    
    // Kiểm tra server time từ response headers
    console.log('Server time from headers:', response.headers.get('date'));
  } catch (error) {
    console.error('Could not fetch server config:', error);
  }
};

const checkServerTime = async () => {
  try {
    console.log('Checking server time...');
    const response = await fetch(`${ApplicationSettings.AUTHORITY}/.well-known/openid-configuration`,
      {
      method: 'GET'
    });
    
    console.log('Server response status:', response.status);
    console.log('Server response headers:', Object.fromEntries(response.headers.entries()));
    console.log('Server date header:', response.headers.get('date'));
    
    // So sánh với client time
    const clientTime = new Date();
    const serverTime = new Date(response.headers.get('date') || '');
    console.log('Client time:', clientTime.toISOString());
    console.log('Server time:', serverTime.toISOString());
    console.log('Time difference:', clientTime.getTime() - serverTime.getTime(), 'ms');
    
  } catch (error) {
    console.error('Could not check server time:', error);
  }
};

const debugBrowser = (): void => {
  console.log('Browser Debug Information:');
  console.log('User Agent:', navigator.userAgent);
  console.log('Platform:', navigator.platform);
  console.log('Language:', navigator.language);
  console.log('Languages:', navigator.languages);
  console.log('Timezone:', Intl.DateTimeFormat().resolvedOptions().timeZone);
  console.log('Timezone offset:', new Date().getTimezoneOffset());
  
  // Test Date object
  console.log('Date.now():', Date.now());
  console.log('new Date().getTime():', new Date().getTime());
  console.log('new Date().toISOString():', new Date().toISOString());
  console.log('new Date().toString():', new Date().toString());
  
  // Test timestamp conversion
  const testTimestamp = Math.floor(Date.now() / 1000);
  const testDate = new Date(testTimestamp * 1000);
  console.log('Test timestamp:', testTimestamp);
  console.log('Test date:', testDate.toISOString());
  console.log('Test year:', testDate.getFullYear());
  
  // Test different timestamp formats
  console.log('Current timestamp (seconds):', Math.floor(Date.now() / 1000));
  console.log('Current timestamp (milliseconds):', Date.now());
  
  // Test Date constructor
  console.log('new Date():', new Date());
  console.log('new Date(Date.now()):', new Date(Date.now()));
  console.log('new Date(Date.now() * 1000):', new Date(Date.now() * 1000));
};

const debugEnvironment = (): void => {
  console.log('Environment Debug Information:');
  console.log('Window location:', window.location.href);
  console.log('Window origin:', window.location.origin);
  console.log('Window protocol:', window.location.protocol);
  console.log('Window host:', window.location.host);
  
  // Test localStorage
  console.log('LocalStorage available:', typeof Storage !== 'undefined');
  console.log('LocalStorage length:', localStorage.length);
  
  // Test JSON
  console.log('JSON available:', typeof JSON !== 'undefined');
  
  // Test fetch
  console.log('Fetch available:', typeof fetch !== 'undefined');
  
  // Test Promise
  console.log('Promise available:', typeof Promise !== 'undefined');
  
  // Test async/await
  console.log('Async/await available:', (async () => {}).constructor.name === 'AsyncFunction');
};

const debugOidcClient = (): void => {
  console.log('OIDC Client Debug Information:');
  console.log('OIDC Client version:', '3.2.0');
  console.log('UserManager available:', typeof userManager !== 'undefined');
  
  // Test OIDC client methods
  if (userManager) {
    console.log('UserManager methods:', Object.getOwnPropertyNames(Object.getPrototypeOf(userManager)));
    console.log('UserManager settings:', userManager.settings);
  }
  
  // Test OIDC client events
  if (userManager && userManager.events) {
    console.log('UserManager events available:', typeof userManager.events !== 'undefined');
  }
};

const debugTimestampCreation = (): void => {
  console.log('Timestamp Creation Debug:');
  
  // Test different ways to create timestamp
  const now = new Date();
  const timestamp1 = Math.floor(now.getTime() / 1000);
  const timestamp2 = Math.floor(Date.now() / 1000);
  const timestamp3 = Math.floor(new Date().getTime() / 1000);
  
  console.log('Method 1 (new Date().getTime() / 1000):', timestamp1);
  console.log('Method 2 (Date.now() / 1000):', timestamp2);
  console.log('Method 3 (new Date().getTime() / 1000):', timestamp3);
  
  // Test conversion back to Date
  const date1 = new Date(timestamp1 * 1000);
  const date2 = new Date(timestamp2 * 1000);
  const date3 = new Date(timestamp3 * 1000);
  
  console.log('Date 1:', date1.toISOString());
  console.log('Date 2:', date2.toISOString());
  console.log('Date 3:', date3.toISOString());
  
  // Test if all methods give same result
  console.log('All methods same:', timestamp1 === timestamp2 && timestamp2 === timestamp3);
};

const debugOidcEntryCreation = (): void => {
  console.log('OIDC Entry Creation Debug:');
  
  // Test creating OIDC entry manually
  const testEntry = {
    id: 'test-id',
    data: { returnUrl: 'http://localhost:3000/' },
    created: Math.floor(Date.now() / 1000),
    request_type: 'si:r',
    authority: 'https://localhost:5000',
    client_id: 'admin-spa',
    redirect_uri: 'http://localhost:3000/callback',
    scope: 'openid profile offline_access modular-monolith-ddd-api'
  };
  
  console.log('Test entry:', testEntry);
  console.log('Test entry created:', new Date(testEntry.created * 1000).toISOString());
  
  // Test storing in localStorage
  try {
    localStorage.setItem('oidc.test-debug', JSON.stringify(testEntry));
    const retrieved = localStorage.getItem('oidc.test-debug');
    const parsed = JSON.parse(retrieved || '{}');
    console.log('Retrieved entry:', parsed);
    console.log('Retrieved created:', new Date(parsed.created * 1000).toISOString());
    localStorage.removeItem('oidc.test-debug');
  } catch (error) {
    console.error('Error testing localStorage:', error);
  }
};

const debugServerTimeout = async () => {
  try {
    console.log('Debugging server timeout...');
    
    const response = await fetch(`${ApplicationSettings.AUTHORITY}/.well-known/openid-configuration`);
    const config = await response.json();
    
    console.log('Server OIDC Configuration:');
    console.log('Server config:', config);
    
    // Kiểm tra xem server có expose authorization code lifetime không
    console.log('Looking for authorization code lifetime in config...');
    console.log('Config keys:', Object.keys(config));
    
    // Tìm các key liên quan đến timeout
    const timeoutKeys = Object.keys(config).filter(key => 
      key.toLowerCase().includes('lifetime') || 
      key.toLowerCase().includes('timeout') ||
      key.toLowerCase().includes('expire')
    );
    console.log('Timeout-related keys:', timeoutKeys);
    
    if (timeoutKeys.length > 0) {
      timeoutKeys.forEach(key => {
        console.log(`${key}:`, config[key]);
      });
    }
    
  } catch (error) {
    console.error('Could not debug server timeout:', error);
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

// Test PKCE validation function
export const testPKCEValidation = async (codeVerifier: string, codeChallenge: string): Promise<boolean> => {
  try {
    const encoder = new TextEncoder();
    const data = encoder.encode(codeVerifier);
    const digest = await crypto.subtle.digest('SHA-256', data);
    const hashArray = Array.from(new Uint8Array(digest));
    const hashBase64 = btoa(String.fromCharCode.apply(null, hashArray));
    const calculatedChallenge = hashBase64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
    
    console.log('Calculated challenge:', calculatedChallenge);
    console.log('Stored challenge:', codeChallenge);
    console.log('Match:', calculatedChallenge === codeChallenge);
    
    return calculatedChallenge === codeChallenge;
  } catch (error) {
    console.error('PKCE validation error:', error);
    return false;
  }
};

// Thêm vào AuthenticationService.ts
export const debugCodeChallengePhases = async (): Promise<void> => {
  console.group('🔍 PKCE Code Challenge Debug - All Phases');
  
  try {
    // === PHASE 1: CHECK CURRENT STATE ===
    console.group('📋 Phase 1: Current OIDC State');
    
    const oidcKeys = Object.keys(localStorage).filter(key => key.startsWith('oidc.'));
    console.log('OIDC keys found:', oidcKeys);
    
    if (oidcKeys.length === 0) {
      console.warn('No OIDC entries found in localStorage');
      console.groupEnd();
      return;
    }
    
    // Lấy entry mới nhất
    let latestEntry = null;
    let latestTimestamp = 0;
    
    for (const key of oidcKeys) {
      try {
        const entry = JSON.parse(localStorage.getItem(key) || '{}');
        if (entry.created && entry.created > latestTimestamp) {
          latestTimestamp = entry.created;
          latestEntry = { key, data: entry };
        }
      } catch (e) {
        console.warn(`Failed to parse entry ${key}:`, e);
      }
    }
    
    if (!latestEntry) {
      console.warn('No valid OIDC entries found');
      console.groupEnd();
      return;
    }
    
    console.log('Latest OIDC entry:', latestEntry.key);
    console.log('Created at:', new Date(latestEntry.data.created * 1000).toISOString());
    console.log('Request type:', latestEntry.data.request_type);
    console.log('Client ID:', latestEntry.data.client_id);
    console.log('Authority:', latestEntry.data.authority);
    
    // === PHASE 2: CHECK PKCE DATA ===
    console.group('Phase 2: PKCE Data Analysis');
    
    const { code_verifier, code_challenge, code_challenge_method } = latestEntry.data;
    
    console.log('Code Verifier exists:', !!code_verifier);
    console.log('Code Challenge exists:', !!code_challenge);
    console.log('Code Challenge Method:', code_challenge_method);
    
    if (code_verifier) {
      console.log('Code Verifier length:', code_verifier.length);
      console.log('Code Verifier (first 20 chars):', code_verifier.substring(0, 20) + '...');
    }
    
    if (code_challenge) {
      console.log('Code Challenge length:', code_challenge.length);
      console.log('Code Challenge (first 20 chars):', code_challenge.substring(0, 20) + '...');
    }
    
    // === PHASE 3: VALIDATE PKCE ===
    console.group('Phase 3: PKCE Validation');
    
    if (code_verifier && code_challenge) {
      console.log('Calculating code challenge from verifier...');
      
      try {
        const encoder = new TextEncoder();
        const data = encoder.encode(code_verifier);
        const digest = await crypto.subtle.digest('SHA-256', data);
        const hashArray = Array.from(new Uint8Array(digest));
        const hashBase64 = btoa(String.fromCharCode.apply(null, hashArray));
        const calculatedChallenge = hashBase64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
        
        console.log('Calculated challenge:', calculatedChallenge);
        console.log('Stored challenge:', code_challenge);
        console.log('Match result:', calculatedChallenge === code_challenge);
        
        if (calculatedChallenge === code_challenge) {
          console.log('PKCE VALIDATION PASSED');
        } else {
          console.error('PKCE VALIDATION FAILED');
          console.error('Expected:', calculatedChallenge);
          console.error('Actual:', code_challenge);
        }
        
      } catch (error) {
        console.error('Error during PKCE calculation:', error);
      }
    } else {
      console.warn('Missing PKCE data - cannot validate');
    }
    
    console.groupEnd(); // Phase 3
    
    // === PHASE 4: URL ANALYSIS ===
    console.group('Phase 4: URL Analysis');
    
    const currentUrl = window.location.href;
    console.log('Current URL:', currentUrl);
    
    const url = new URL(currentUrl);
    const code = url.searchParams.get('code');
    const state = url.searchParams.get('state');
    const sessionState = url.searchParams.get('session_state');
    
    console.log('Authorization Code exists:', !!code);
    console.log('State parameter:', state);
    console.log('Session State:', sessionState);
    
    if (code) {
      console.log('Code length:', code.length);
      console.log('Code (first 20 chars):', code.substring(0, 20) + '...');
    }
    
    // Kiểm tra xem state có khớp với OIDC entry không
    if (state && latestEntry.data.id) {
      console.log('State match check:');
      console.log('URL state:', state);
      console.log('OIDC entry ID:', latestEntry.data.id);
      console.log('State matches:', state === latestEntry.data.id);
    }
    
    console.groupEnd(); // Phase 4
    
    // === PHASE 5: TIMING ANALYSIS ===
    console.group('Phase 5: Timing Analysis');
    
    const createdTime = new Date(latestEntry.data.created * 1000);
    const currentTime = new Date();
    const timeDiff = currentTime.getTime() - createdTime.getTime();
    
    console.log('OIDC entry created:', createdTime.toISOString());
    console.log('Current time:', currentTime.toISOString());
    console.log('Time elapsed:', Math.round(timeDiff / 1000), 'seconds');
    
    if (timeDiff > 300000) { // 5 minutes
      console.warn('OIDC entry is older than 5 minutes');
    }
    if (timeDiff > 600000) { // 10 minutes
      console.error('OIDC entry is older than 10 minutes - likely expired');
    }
    
    console.groupEnd(); // Phase 5
    
  } catch (error) {
    console.error('Error during debug:', error);
  }
  
  console.groupEnd(); // Main group
};

// Thêm vào openIddictAuthenticationService
export const openIddictAuthenticationService = {
  login,
  handleCallback,
  logout,
  getAccessToken,
  isAuthenticated,
  getUser,
  getUserId,
  getUsername,
  testPKCEValidation,
  debugCodeChallengePhases
} as const;