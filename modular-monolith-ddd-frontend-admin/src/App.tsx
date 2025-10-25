import './App.css';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
// import Dashboard from './modules/admin/pages/Dashboard';
import Callback from './modules/user-access/pages/Callback';
// import RequireAuth from './modules/user-access/guards/RequireAuth';
import { Suspense, lazy, useEffect } from 'react';
import { Toaster } from 'react-hot-toast';
import SplashPage from './pages/SplashPage';
import TopBar from './layout/TopBar';
import Sidebar from './layout/Sidebar';
import { useAppSelector } from 'shared/store/hooks';

import './configuration/i18n';

// Lazy loading components
const Dashboard = lazy(() => import('./modules/user-access/pages/Dashboard'));

// Component Pages
const AccordionPage = lazy(() => import('./pages/Components/AccordionPage'));
const AlertPage = lazy(() => import('./pages/Components/AlertPage'));
const BadgePage = lazy(() => import('./pages/Components/BadgePage'));
const ButtonPage = lazy(() => import('./pages/Components/ButtonPage'));
const DropDownPage = lazy(() => import('./pages/Components/DropDownPage'));
const IconButtonPage = lazy(() => import('./pages/Components/IconButtonPage'));
const PaginationPage = lazy(() => import('./pages/Components/PaginationPage'));
const SpinnerPage = lazy(() => import('./pages/Components/SpinnerPage'));
const TabPage = lazy(() => import('./pages/Components/TabPage'));

// Layout components
const PrivateOutlet = lazy(() => import('./modules/user-access/guards/PrivateOutlet'));


function App() {
    const theme = useAppSelector((state) => state.util.theme);

    useEffect(() => {
        if (theme === 'light') {
            document.documentElement.classList.remove('dark');
        } else {
            document.documentElement.classList.add('dark');
        }
    }, [theme]);

    return (
        <div className="max-w-[1920px] h-screen m-auto animate-fade-in-up">
            <Toaster />
            <BrowserRouter>
                {/* <TopBar />
                <Sidebar /> */}
                <Suspense fallback={<SplashPage />}>
                    <Routes>
                        <Route path="/callback" element={<Callback />} />
                        {/* <Route path="/" element={
                            <RequireAuth>
                                <Dashboard />
                            </RequireAuth>
                        } /> */}
                        <Route path="/" element={<PrivateOutlet />}>   
                                {/* <RequireAuth>
                                    <Dashboard />
                                </RequireAuth>      */}
                            <Route path="" element={<Dashboard />} />                       
                            <Route path="accordion" element={<AccordionPage />} />
                            <Route path="alert" element={<AlertPage />} />
                            <Route path="badge" element={<BadgePage />} />
                            <Route path="button" element={<ButtonPage />} />
                            <Route path="dropdown" element={<DropDownPage />} />
                            <Route path="icon-action-button" element={<IconButtonPage />} />
                            <Route path="pagination" element={<PaginationPage />} />
                            <Route path="spinner" element={<SpinnerPage />} />
                            <Route path="tab" element={<TabPage />} />
                        </Route>
                        <Route path="*" element={<Navigate to={'/'} />} />
                    </Routes>
                </Suspense>
            </BrowserRouter>
        </div>
    );
}

export default App;