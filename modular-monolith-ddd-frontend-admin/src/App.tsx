import './App.css';
import { Routes, Route, Navigate } from 'react-router-dom';
import Dashboard from './modules/admin/pages/Dashboard';
import Callback from './modules/user-access/pages/Callback';
import RequireAuth from './modules/user-access/guards/RequireAuth';

function App() {
  return (
    <Routes>
      <Route path="/callback" element={<Callback />} />
      <Route path="/" element={
        <RequireAuth>
          <Dashboard />
        </RequireAuth>
      } />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default App;