import { useEffect, useState } from 'react';
import { logout } from '../../../auth/AuthenticationService';

export default function Dashboard() {
  const [user, setUser] = useState<any>(null);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    loadUserData();
  }, []); // Only run once

  const loadUserData = async () => {
    try {
      setLoading(true);
      
      // Get user from OIDC
      const { getUser } = await import('../../../auth/AuthenticationService');
      const userData = await getUser();
      
      if (userData && !userData.expired) {                        
        setUser(userData);
        console.log('User loaded:', userData);
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
      <div style={{ padding: '20px', textAlign: 'center' }}>
        <div>Loading...</div>
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
    <div style={{ padding: '20px' }}>
      <h1>Dashboard</h1>
      <p>Welcome! You are authenticated.</p>
      <div>
        <p><strong>User ID:</strong> {user.profile?.sub}</p>
        <p><strong>Name:</strong> {user.profile?.name || user.profile?.preferred_username}</p>
        <p><strong>Email:</strong> {user.profile?.email}</p>
      </div>
      <button onClick={handleLogout}>
        Logout
      </button>
    </div>
  );
}