import { useState, useEffect, useRef } from 'react';
import {  
  handleCallback as processOidcCallback, 
} from '../services/AuthenticationService';
import Spinner from 'components/Elements/Spinner';
export default function OpenIddictCallback() {
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const isProcessingCallback = useRef(false);
       
  useEffect(() => {
    if (isProcessingCallback.current){
      return;
    }
    
    const urlParams = new URLSearchParams(window.location.search);
    const code = urlParams.get('code');   
    const state = urlParams.get('state');
    const errorParam = urlParams.get('error');
    
    const fullCallbackUrl = window.location.href;

    // Check for OAuth error first
    if (errorParam) {
      console.error('OAuth error:', errorParam);
      setError(`Authentication failed: ${errorParam}`);
      setLoading(false);
      return;
    }
    
    if (!code) {
      console.log('No authorization code found');
      setError('No authorization code found');
      setLoading(false);
      return;
    }

    console.log('Processing OIDC callback...');
    console.log('Code:', code);
    console.log('State:', state);
    
    isProcessingCallback.current = true;
    
    const processCallback = async () => {
      try {
        setLoading(true);
                
        const result = await processOidcCallback(fullCallbackUrl);
        console.log('Callback processed successfully:', result);
               
        // Redirect to the return URL or home
        const returnUrl = result.returnUrl || '/';
        console.log('Redirecting to:', returnUrl);
        
        // Use replace to avoid back button issues
        window.location.replace(returnUrl);
        
      } catch (error) {
        console.error('Callback processing error:', error);
        const errorMessage = error instanceof Error ? error.message : 'Unknown error';
        setError(`Failed to process authentication callback: ${errorMessage}`);
        setLoading(false);
      }
      finally {
        isProcessingCallback.current = false;
      }
    };
    
    processCallback();
  }, []); // Empty dependency array - only run once

  if (loading) {
    return (
      <div className="w-full min-h-[calc(100vh-64px)] pt-2 pl-2 pr-2 relative">
                <div className="abs-center">
                    <Spinner message="Loading" />
                </div>
            </div>
    );
  }

  if (error) {
    return (
      <div style={{ padding: '20px', textAlign: 'center' }}>
        <div style={{ color: 'red' }}>Error: {error}</div>
        <button onClick={() => window.location.href = '/'}>
          Go to Home
        </button>
      </div>
    );
  }

  return null;
}