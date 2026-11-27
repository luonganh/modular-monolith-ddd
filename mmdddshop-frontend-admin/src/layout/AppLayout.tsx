import { Suspense, useEffect } from 'react';
import { Outlet, useLocation } from 'react-router-dom';

import { useAppDispatch, useAppSelector } from 'shared/store/hooks';
import { setMobileSidebarOpen } from 'shared/store/reducers/util';

import Sidebar from 'layout/Sidebar';
import TopBar from 'layout/TopBar';
import Spinner from 'components/Elements/Spinner';

const AppLayout: React.FC = () => {
    const dispatch = useAppDispatch();
    const { pathname } = useLocation();
    const isSidebarOpen = useAppSelector((s) => s.util.isSidebarOpen);
    const isMobileOpen = useAppSelector((s) => s.util.isMobileSidebarOpen);
    const theme = useAppSelector((s) => s.util.theme);

    // Mobile: chuyển trang thì đóng sidebar
    useEffect(() => {
        dispatch(setMobileSidebarOpen(false));
    }, [pathname, dispatch]);

    // Kéo cửa sổ lên kích thước desktop thì bỏ trạng thái mobile
    useEffect(() => {
        const mq = window.matchMedia('(min-width: 1024px)');
        const onChange = (e: MediaQueryListEvent) => {
            if (e.matches) dispatch(setMobileSidebarOpen(false));
        };
        mq.addEventListener('change', onChange);
        return () => mq.removeEventListener('change', onChange);
    }, [dispatch]);

    // Bật dark mode thật (darkMode: 'class')
    useEffect(() => {
        document.documentElement.classList.toggle('dark', theme === 'dark');
    }, [theme]);

    return (
        <div className="min-h-screen bg-light-primary100 dark:bg-dark-primary100">
            <Sidebar />

            {/* Backdrop chỉ dùng trên mobile */}
            {isMobileOpen && (
                <div
                    aria-hidden="true"
                    className="fixed inset-0 z-40 bg-black/50 lg:hidden"
                    onClick={() => dispatch(setMobileSidebarOpen(false))}
                />
            )}

            <div
                className={`flex min-h-screen flex-col transition-[margin] duration-300 ${
                    isSidebarOpen ? 'lg:ml-72' : 'lg:ml-20'
                }`}
            >
                <TopBar />
                <main className="flex-1 p-4 md:p-6">
                    <Suspense fallback={<Spinner message="Loading" />}>
                        <Outlet />
                    </Suspense>
                </main>
            </div>
        </div>
    );
};

export default AppLayout;