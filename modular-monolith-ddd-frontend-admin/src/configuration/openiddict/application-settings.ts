// Hybrid ApplicationSettings: Grzybek pattern + Vite environment variables
export class ApplicationSettings {
    // Fallback values (Grzybek pattern)
    private static readonly DEFAULT_API_URL = 'https://localhost:5000/';
    private static readonly DEFAULT_AUTHORITY = 'https://localhost:5000';
    private static readonly DEFAULT_CLIENT_ID = 'admin-spa';
    // private static readonly DEFAULT_REDIRECT_URI = 'http://localhost:3001/callback';
    private static readonly DEFAULT_SCOPE = 'openid profile api offline_access';

    // Public static readonly properties (Grzybek pattern)
    public static readonly API_URL = ApplicationSettings.getEnvValue('VITE_API_URL', ApplicationSettings.DEFAULT_API_URL);
    public static readonly AUTHORITY = ApplicationSettings.getEnvValue('VITE_AUTHORITY', ApplicationSettings.DEFAULT_AUTHORITY);
    public static readonly CLIENT_ID = ApplicationSettings.getEnvValue('VITE_CLIENT_ID', ApplicationSettings.DEFAULT_CLIENT_ID);
    // Redirect URI used by the OIDC client.
    // Source: Vite env variable `VITE_REDIRECT_URI`; fallback to `${window.location.origin}/callback`.
    // Make sure this URI is whitelisted in the Authorization Server (both 3000 and 3001 if used).
    public static readonly REDIRECT_URI =
      ApplicationSettings.getEnvValue('VITE_REDIRECT_URI', `${window.location.origin}/callback`);     
    public static readonly SCOPE = ApplicationSettings.getEnvValue('VITE_SCOPE', ApplicationSettings.DEFAULT_SCOPE);

    // Helper method to get environment variable with fallback
    private static getEnvValue(envKey: string, fallback: string): string {
        // Try Vite environment variable first
        if (typeof import.meta !== 'undefined' && import.meta.env && import.meta.env[envKey]) {
            return import.meta.env[envKey] as string;
        }
        
        // Fallback to hardcoded value (Grzybek pattern)
        return fallback;
    }

    // Debug method to show current configuration
    public static getDebugInfo(): Record<string, string> {
        return {
            API_URL: ApplicationSettings.API_URL,
            AUTHORITY: ApplicationSettings.AUTHORITY,
            CLIENT_ID: ApplicationSettings.CLIENT_ID,
            REDIRECT_URI: ApplicationSettings.REDIRECT_URI,
            SCOPE: ApplicationSettings.SCOPE,
            'Environment': typeof import.meta !== 'undefined' ? 'Vite' : 'Node'
        };
    }    
}
