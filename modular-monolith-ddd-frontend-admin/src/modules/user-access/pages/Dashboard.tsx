import { useEffect, useState } from 'react';
import { logout } from '../services/AuthenticationService';
import MainLayout from 'layout/MainLayout';
import { useTranslation } from 'react-i18next';
import Spinner from 'components/Elements/Spinner';

const Dashboard: React.FC = () => {
  const { t } = useTranslation();
  const [user, setUser] = useState<any>(null);
  const [loading, setLoading] = useState<boolean>(true);
  useEffect(() => {
    loadUserData();
  }, []); // Only run once

  const loadUserData = async () => {
    try {
      setLoading(true);
      
      // Get user from OIDC
      const { getUser } = await import('../services/AuthenticationService');
      const userData = await getUser();
      
      if (userData && !userData.expired) {                        
        setUser(userData);        
      } else {
        console.log('No valid user found');
        setUser(null);
      }
    } catch (error) {
      console.error('Error loading user:', error);
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = async () => {
    try {
      await logout();
    } catch (error) {
      console.error('Logout error:', error);
      // Fallback: clear local storage and reload
      localStorage.clear();
      window.location.href = '/';
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

  if (!user) {
    return (
      <div style={{ padding: '20px', textAlign: 'center' }}>
        <div>Not authenticated</div>
      </div>
    );
  }


  return (
      <MainLayout
          title={t('dashboard', { ns: 'sidebar' })}
          breadcrumbs={[{ title: t('dashboard', { ns: 'sidebar' }), link: '#', disabled: true }]}
      >
          <div className="grid w-full grid-cols-1 gap-5 lg:grid-cols-2">
              <h1 className="card col-span-1 p-10 text-center lg:col-span-2">
                {t('dashboard.welcome', { ns: 'translation' })} {user.profile?.name || user.profile?.preferred_username}! {t('dashboard.authenticated', { ns: 'translation' })}
              </h1>                                        
              <p className="card p-10">
                  This is a flexible and responsive dashboard template created
                  using some of the latest technologies, including React JS,
                  TypeScript, Redux Toolkit, Tailwind CSS, React Router DOM
                  v6, and Vite JS.
              </p>
              <ul className="card p-10">
                  <h4 className="mt-4">Features</h4>
                  <li>
                      Highly customizable and responsive dashboard template
                  </li>
                  <li>
                      Built using React JS and TypeScript for efficient
                      development and maintenance
                  </li>
                  <li>Redux Toolkit for efficient state management</li>
                  <li>Tailwind CSS for streamlined styling</li>
                  <li>
                      React Router DOM v6 for easy navigation throughout your
                      application
                  </li>
                  <li>Vite JS for speedy development</li>
              </ul>
          </div>
      </MainLayout>
  );
};

export default Dashboard;