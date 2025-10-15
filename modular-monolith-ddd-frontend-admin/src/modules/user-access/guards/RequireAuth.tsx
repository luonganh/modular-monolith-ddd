import { useEffect, useState, type ReactNode } from 'react';
import {   
  login as oidcLogin, 
  isAuthenticated as checkAuth
} from '../../../auth/AuthenticationService';

export default function RequireAuth({ children }: { children: ReactNode }) {   
  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    const path = window.location.pathname;
    console.log('RequireAuth useEffect - path:', path);
        
    // Only run when NOT on callback
    if (window.location.pathname.startsWith('/callback')) {
      console.log('On callback page, skipping auth check');
      return;
    }        
    
    checkAuthStatus();
  }, []); // Empty dependency array - only run once

  const checkAuthStatus = async () => {
    try {
      // Check if we're on callback page - don't check auth status
      const path = window.location.pathname;
      if (window.location.pathname.startsWith('/callback')) {
        return;
      }
      
      setLoading(true);
      console.log('Checking authentication status...');
      const authenticated = await checkAuth();
      console.log('Authentication result:', authenticated);
      
      if (authenticated) {
        console.log('User is authenticated, no need to login');
        setLoading(false);
        return;
      }
     
      console.log('User not authenticated, starting login flow');
      // Auto-redirect to OIDC authorize when unauthenticated on public routes
      const isCallback = path === '/callback';
      const isManualLogin = path === '/login';
      if (!isCallback && !isManualLogin) {
        try {
          await oidcLogin();
        } catch (error) {
          console.error('Login error:', error);
        }
      }
    } catch (error) {
      console.error('Error checking auth status:', error);      
    } finally {
      setLoading(false);
    }
  };
 
  if (loading) {
    return (
      <div style={{ padding: '20px', textAlign: 'center' }}>
        <div>Loading...</div>
      </div>
    );
  }

  return <>{children}</>;
}