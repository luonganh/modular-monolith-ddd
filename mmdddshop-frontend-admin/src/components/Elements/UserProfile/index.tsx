import { useEffect, useRef, useState } from 'react';
import type { User } from 'oidc-client-ts';
import { FiChevronDown, FiLogOut } from 'react-icons/fi';
import { useTranslation } from 'react-i18next';
import { getUser, logout } from 'modules/user-access/services/AuthenticationService';

const getDisplayName = (user: User): string =>
    user.profile.name || user.profile.preferred_username || user.profile.email || 'User';

const getInitials = (name: string): string =>
    name
        .split(/\s+/)
        .filter(Boolean)
        .slice(-2)
        .map((part) => part.charAt(0).toUpperCase())
        .join('');

const UserProfile = () => {
    const { t } = useTranslation('translation');
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);
    const [open, setOpen] = useState(false);
    const containerRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        let cancelled = false;

        getUser()
            .then((userData) => {
                if (!cancelled) setUser(userData && !userData.expired ? userData : null);
            })
            .catch((error: unknown) => {
                console.error('Error loading user:', error);
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, []);

    // Close the menu on outside click or Escape
    useEffect(() => {
        if (!open) return;

        const handleMouseDown = (event: MouseEvent) => {
            if (!containerRef.current?.contains(event.target as Node)) setOpen(false);
        };
        const handleKeyDown = (event: KeyboardEvent) => {
            if (event.key === 'Escape') setOpen(false);
        };

        document.addEventListener('mousedown', handleMouseDown);
        document.addEventListener('keydown', handleKeyDown);
        return () => {
            document.removeEventListener('mousedown', handleMouseDown);
            document.removeEventListener('keydown', handleKeyDown);
        };
    }, [open]);

    const handleLogout = async () => {
        try {
            // signoutRedirect removes the stored user itself; do not clear storage first
            await logout();
        } catch (error: unknown) {
            console.error('Logout error:', error);
            localStorage.clear();
            window.location.href = '/';
        }
    };

    if (loading) {
        return (
            <div
                aria-hidden="true"
                className="h-9 w-9 animate-pulse rounded-full bg-light-gray200 dark:bg-dark-gray200"
            />
        );
    }

    if (!user) {
        return null;
    }

    const displayName = getDisplayName(user);
    const email = user.profile.email;

    return (
        <div ref={containerRef} className="relative">
            <button
                type="button"
                aria-haspopup="menu"
                aria-expanded={open}
                onClick={() => setOpen((value) => !value)}
                className="flex items-center gap-2 rounded-full p-1 pr-2 hover:bg-light-primary100 hover:text-light-gray900 dark:hover:bg-dark-primary100 dark:hover:text-dark-gray900"
            >
                <span className="flex h-9 w-9 items-center justify-center rounded-full bg-light-secondary600 text-sm font-semibold text-white">
                    {getInitials(displayName)}
                </span>
                <span className="hidden text-sm font-medium sm:block">{displayName}</span>
                <FiChevronDown
                    className={`transition-transform duration-200 ${open ? 'rotate-180' : ''}`}
                />
            </button>

            {open && (
                <div
                    role="menu"
                    className="absolute right-0 mt-2 w-64 overflow-hidden rounded-lg border border-light-gray300 bg-light-primary50 shadow-lg dark:border-dark-gray300 dark:bg-dark-primary50"
                >
                    <div className="border-b border-light-gray300 px-4 py-3 dark:border-dark-gray300">
                        <span className="block truncate text-sm font-medium">{displayName}</span>
                        {email && (
                            <span className="block truncate text-xs text-light-gray500 dark:text-dark-gray500">
                                {email}
                            </span>
                        )}
                    </div>
                    <button
                        type="button"
                        role="menuitem"
                        onClick={() => void handleLogout()}
                        className="flex w-full items-center gap-2 px-4 py-3 text-left text-sm hover:bg-light-primary100 hover:text-light-gray900 dark:hover:bg-dark-primary100 dark:hover:text-dark-gray900"
                    >
                        <FiLogOut />
                        {t('user.logout', { defaultValue: 'Log out' })}
                    </button>
                </div>
            )}
        </div>
    );
};

export default UserProfile;
