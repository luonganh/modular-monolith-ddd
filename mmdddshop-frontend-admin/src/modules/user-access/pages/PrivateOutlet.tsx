import { useEffect, useState, Suspense } from 'react';
import { Outlet } from 'react-router-dom';
import {   
  login as oidcLogin, 
  isAuthenticated as checkAuth
} from 'modules/user-access/services/AuthenticationService';
// Redux
import { useAppSelector } from 'shared/store/hooks';
import Sidebar from 'layout/Sidebar';
import TopBar from 'layout/TopBar';
import Spinner from 'components/Elements/Spinner';

const PrivateOutlet = () => {
    const isOpen = useAppSelector((state) => state.util.isSidebarOpen);    
    const [loading, setLoading] = useState<boolean>(false);    

    useEffect(() => {               
        checkAuthStatus();
      }, []); // Empty dependency array - only run once

    const checkAuthStatus = async () => {
        try {          
          const path = window.location.pathname;          
          setLoading(true);         
          console.log('Checking authentication status...');
          const authenticated = await checkAuth();
          console.log('Authentication result:', authenticated);          
          if (authenticated) {                     
            setLoading(false);                      
          }
          else {           
            // Auto-redirect to OIDC authorize when unauthenticated on public routes
            const isCallback = path === '/callback';
            const isManualLogin = path === '/login';
            if (!isCallback && !isManualLogin) {
              try {
                await oidcLogin();
              } catch (error) {
                console.error('Login error:', error);
              }
              finally {
                setLoading(false);
              }
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
            <div className="w-full min-h-[calc(100vh-64px)] pt-2 pl-2 pr-2 relative">
                <div className="abs-center">
                    <Spinner message="Loading" />
                </div>
            </div>
        );
    }

      // Don't redirect on callback page
      if (window.location.pathname.startsWith('/callback')) {
        return <Outlet />;
      }
  
    // if (!authenticated) {
    //     return <Navigate to={'/callback'} />;
    // }

    return (
        <>
        <TopBar />
        <Sidebar />                
            <div
                style={{
                    paddingLeft:
                        window.innerWidth < 1024
                            ? '0rem'
                            : isOpen
                            ? '18.5rem'
                            : '5.5rem',
                }}
                className={`pt-16 min-h-screen overflow-auto duration-300`}
            >
                <Suspense
                    fallback={
                        <div className="w-full min-h-[calc(100vh-64px)] pt-2 pl-2 pr-2 relative">
                            <div className="abs-center">
                                <Spinner message="Loading" />
                            </div>
                        </div>
                    }
                >
                    <Outlet />
                </Suspense>
            </div>
        </>
    );
};

export default PrivateOutlet;
