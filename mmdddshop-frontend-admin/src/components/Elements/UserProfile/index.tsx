import { useEffect, useState } from 'react';
import { getUser, logout } from 'modules/user-access/services/AuthenticationService';
import DropDownButton from 'components/Elements/DropDownButton';
import Spinner from 'components/Elements/Spinner';
import { useTranslation } from 'react-i18next';

const UserProfile = () => {
    const [user, setUser] = useState<any>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const { t } = useTranslation();

    useEffect(() => {
      loadUserData();
    }, []);
  
    const loadUserData = async () => {
      try {
        setLoading(true);
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
      return null;
    }
  
    const userMenuItems = [
      {
        title: t('user.profile', { ns: 'translation' }),
        handleClick: () => {
          // Navigate to profile page
          console.log('Navigate to profile');
        },
        disabled: false,
      },
      {
        title: t('user.settings', { ns: 'translation' }),
        handleClick: () => {
          // Navigate to settings page
          console.log('Navigate to settings');
        },
        disabled: false,
      },
      {
        title: t('user.logout', { ns: 'translation' }),
        handleClick: handleLogout,
        disabled: false,
      },
    ];
  
    return (
      <div className="flex items-center gap-3">
        <div className="text-right">
          <div className="text-sm font-medium text-gray-900 dark:text-white">
            {user.profile?.name || user.profile?.preferred_username || 'User'}
          </div>
          <div className="text-xs text-gray-500 dark:text-gray-400">
            {user.profile?.email}
          </div>
        </div>
        <DropDownButton
          title={user.profile?.name || user.profile?.preferred_username || 'User'}
          variant="ghost"
          itemList={userMenuItems.map((item) => ({
            title: item.title,
            handleClick: item.handleClick,
            disabled: item.disabled,
          }))}
        />
      </div>
    );
  };
  
  export default UserProfile;