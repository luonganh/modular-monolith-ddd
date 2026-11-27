// Redux
import { useAppSelector, useAppDispatch } from 'shared/store/hooks';
import {
    toggleTheme,
    setSidebarOpen,
    setMobileSidebarOpen,
} from 'shared/store/reducers/util';
import { RiMenuFoldLine } from 'react-icons/ri';
import { FiMoon, FiSun } from 'react-icons/fi';
import { Languages } from 'configuration/i18n';
import DropDownButton from 'components/Elements/DropDownButton';
import { useTranslation } from 'react-i18next';
import UserProfile from 'components/Elements/UserProfile';

const TopBar = () => {
    const dispatch = useAppDispatch();
    const theme = useAppSelector((state) => state.util.theme);
    const isSidebarOpen = useAppSelector((state) => state.util.isSidebarOpen);
    const isMobileOpen = useAppSelector((state) => state.util.isMobileSidebarOpen);
    const { i18n } = useTranslation();
    // resolvedLanguage stays undefined until a translation file has loaded, so fall back to the detected language
    const currentLanguage = (i18n.resolvedLanguage ?? i18n.language ?? 'vi').split('-')[0];

    const getLanguageName = (code: string) => {
        const languageNames: { [key: string]: { [key: string]: string } } = {
            en: { en: 'English', de: 'German', es: 'Spanish', vi: 'Vietnamese' },
            vi: { en: 'Tiếng Anh', de: 'Tiếng Đức', es: 'Tiếng Tây Ban Nha', vi: 'Tiếng Việt' },
            de: { en: 'Englisch', de: 'Deutsch', es: 'Spanisch', vi: 'Vietnamesisch' },
            es: { en: 'Inglés', de: 'Alemán', es: 'Español', vi: 'Vietnamita' },
        };

        return languageNames[currentLanguage]?.[code] || Languages[code];
    };

    const handleToggleSidebar = () => {
        // Đọc kích thước lúc click, không đọc lúc render
        if (window.matchMedia('(min-width: 1024px)').matches) {
            dispatch(setSidebarOpen(!isSidebarOpen));
        } else {
            dispatch(setMobileSidebarOpen(!isMobileOpen));
        }
    };

    return (
        <header className="sticky top-0 z-30 flex h-16 w-full select-none items-center justify-between border-b border-light-gray300 bg-light-primary50 px-4 dark:border-dark-gray300 dark:bg-dark-primary50">
            <button
                type="button"
                aria-label="Toggle sidebar"
                onClick={handleToggleSidebar}
            >
                <RiMenuFoldLine
                    size="1.5em"
                    className={`duration-300 ${!isSidebarOpen ? '-scale-x-100' : ''}`}
                />
            </button>

            <div className="flex cursor-pointer items-center gap-3">
                <DropDownButton
                    title={getLanguageName(currentLanguage)}
                    variant="ghost"
                    itemList={Object.keys(Languages).map((language: string) => ({
                        title: getLanguageName(language),
                        handleClick: () => {
                            i18n.changeLanguage(language);
                        },
                        disabled: currentLanguage === language,
                    }))}
                />

                <button
                    type="button"
                    aria-label="Toggle theme"
                    onClick={() => dispatch(toggleTheme())}
                >
                    {theme === 'light' ? <FiMoon size="1.5rem" /> : <FiSun size="1.5rem" />}
                </button>

                <UserProfile />
            </div>
        </header>
    );
};

export default TopBar;