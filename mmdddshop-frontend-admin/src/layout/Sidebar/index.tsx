import { useAppSelector, useAppDispatch } from 'shared/store/hooks';
import { setSidebarOpen } from 'shared/store/reducers/util';
import Logo from 'assets/images/logo.svg';
import SideBarItem from 'layout/Sidebar/SideBarItem';
import Image from 'components/Elements/Image';
import { menus } from './menus';

const Sidebar: React.FC = () => {
    const dispatch = useAppDispatch();
    const isSidebarOpen = useAppSelector((s) => s.util.isSidebarOpen);
    const isMobileOpen = useAppSelector((s) => s.util.isMobileSidebarOpen);

    // Trên mobile sidebar luôn hiện đầy đủ; trên desktop phụ thuộc thu gọn/mở rộng
    const expanded = isSidebarOpen || isMobileOpen;

    return (
        <aside
            className={[
                'fixed inset-y-0 left-0 z-50 flex w-72 flex-col overflow-x-hidden',
                'border-r border-light-gray300 bg-light-primary50 dark:border-dark-gray300 dark:bg-dark-primary50',
                'transition-all duration-300',
                isMobileOpen ? 'translate-x-0' : '-translate-x-full',
                'lg:translate-x-0',
                isSidebarOpen ? 'lg:w-72' : 'lg:w-20',
            ].join(' ')}
        >
            <div
                className="flex h-16 shrink-0 cursor-pointer items-center border-b border-light-gray300 bg-light-primary100 pl-4 dark:border-dark-gray300 dark:bg-dark-primary100"
                onClick={() => dispatch(setSidebarOpen(!isSidebarOpen))}
            >
                <Image className="mr-4 w-12" src={Logo} />
                {expanded && (
                    <div className="animate-fade-in">
                        <h3 className="min-w-max">Mmdddshop</h3>
                        <p className="caption min-w-max">Admin</p>
                    </div>
                )}
            </div>

            <nav className="flex-1 overflow-y-auto">
                {menus.map((item) => (
                    <SideBarItem key={item.id} menuItem={item} expanded={expanded} />
                ))}
            </nav>
        </aside>
    );
};

export default Sidebar;