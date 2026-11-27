import i18next from 'i18next';
import { initReactI18next } from 'react-i18next';
import Languagedetector from 'i18next-browser-languagedetector';
import Backend from 'i18next-http-backend';

export const Languages: { [key: string]: string } = {
    vi: 'Tiếng Việt', 
    en: 'English',
    de: 'Deutsch',
    es: 'Español',
};

i18next
    .use(initReactI18next)
    .use(Languagedetector)
    .use(Backend)
    .init({
        fallbackLng: 'vi',
        // Map browser codes such as 'en-US' to the supported base language ('en')
        supportedLngs: Object.keys(Languages),
        load: 'languageOnly',
        debug: import.meta.env.DEV,
        ns: ['sidebar', 'translation'],
        defaultNS: 'sidebar',
    });
