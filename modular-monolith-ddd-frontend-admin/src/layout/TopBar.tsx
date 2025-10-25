// Redux
import { useAppSelector, useAppDispatch } from 'shared/store/hooks';
import { toggleTheme, setSidebarOpen } from 'shared/store/reducers/util';
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
    const { i18n, t } = useTranslation();    

    const getLanguageName = (code: string) => {
        const languageNames: { [key: string]: { [key: string]: string } } = {
            en: { en: 'English', de: 'German', es: 'Spanish', vi: 'Vietnamese' },
            vi: { en: 'Tiếng Anh', de: 'Tiếng Đức', es: 'Tiếng Tây Ban Nha', vi: 'Tiếng Việt' },
            de: { en: 'Englisch', de: 'Deutsch', es: 'Spanisch', vi: 'Vietnamesisch' },
            es: { en: 'Inglés', de: 'Alemán', es: 'Español', vi: 'Vietnamita' }
        };
        
        return languageNames[i18n.resolvedLanguage || 'en']?.[code] || Languages[code];
    };

    return (
        <>
            <div
                className={`fixed top-0 z-20 m-auto flex h-16 w-full select-none items-center justify-between border-b-[1px] border-light-gray300 bg-light-primary50 duration-300 dark:border-dark-gray300 dark:bg-dark-primary50`}
                style={{
                    marginLeft:
                        window.innerWidth > 1024
                            ? isSidebarOpen
                                ? '287px'
                                : '79px'
                            : '0',
                }}
            >
                <div>
                    <RiMenuFoldLine
                        size={'1.5em'}
                        className={`ml-4 cursor-pointer duration-300 ${
                            !isSidebarOpen && '-scale-x-100'
                        }`}
                        onClick={() => dispatch(setSidebarOpen(!isSidebarOpen))}
                    />
                </div>

                <div className="fixed right-6 top-4 flex cursor-pointer items-center gap-3">
                    <div>
                        <DropDownButton
                            title={getLanguageName(i18n.resolvedLanguage!)}
                            variant="ghost"
                            itemList={Object.keys(Languages).map(
                                (language: string) => {
                                    return {
                                        title: getLanguageName(language),
                                        handleClick: () => {
                                            i18n.changeLanguage(language);
                                        },
                                        disabled:
                                            i18n.resolvedLanguage === language,
                                    };
                                }
                            )}
                        />
                    </div>
                    <div onClick={() => dispatch(toggleTheme())}>
                        {theme === 'light' ? (
                            <FiMoon size="1.5rem" />
                        ) : (
                            <FiSun size="1.5rem" />
                        )}
                    </div>

                    <UserProfile />
                </div>
            </div>
        </>
    );
};

export default TopBar;
