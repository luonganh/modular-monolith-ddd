import './App.css'
import { Routes, Route, Navigate } from 'react-router-dom';
import { RequireAuth, Callback } from './modules/user-access';
import { Dashboard } from './modules/admin';

function App() {
  
  return (
    <Routes>
    <Route path="/callback" element={<Callback />} />
    <Route path="/" element={
      <RequireAuth>
        { <Dashboard /> }
      </RequireAuth>
    } />
    <Route path="*" element={<Navigate to="/" replace />} />
  </Routes>
  )
}

export default App
