import { Routes, Route, Navigate } from 'react-router-dom';
import { RequireAuth, Callback } from './modules/user-access';
import { Dashboard } from './modules/admin';
import AppLayout from 'layout/AppLayout';

function App() {
  
  return (
    <Routes>
    <Route path="/callback" element={<Callback />} />
    <Route 
    element={
      <RequireAuth>
        <AppLayout />
      </RequireAuth>
    } 
    >

      <Route index element={<Dashboard />} />
                {/* các trang sau thêm ở đây: products, inventory, orders, users */}
            </Route>

    <Route path="*" element={<Navigate to="/" replace />} />
  </Routes>
  )
}

export default App
