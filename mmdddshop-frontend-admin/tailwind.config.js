const flattenPalette = (palette) =>
  Object.fromEntries(
    Object.entries(palette).flatMap(([name, shades]) =>
      Object.entries(shades).map(([shade, value]) => [`${name}${shade}`, value]),
    ),
  )

const lightPalette = {
  primary: {
    50: '#f8fafc',
    100: '#f1f5f9',
    200: '#e2e8f0',
    300: '#cbd5e1',
    400: '#94a3b8',
    500: '#64748b',
    600: '#475569',
    700: '#334155',
    800: '#1e293b',
    900: '#0f172a',
  },
  secondary: {
    50: '#f0fdfa',
    100: '#ccfbf1',
    200: '#99f6e4',
    300: '#5eead4',
    400: '#2dd4bf',
    500: '#14b8a6',
    600: '#0d9488',
    700: '#0f766e',
    800: '#115e59',
    900: '#134e4a',
  },
  accent: {
    50: '#f8fafc',
    100: '#f1f5f9',
    200: '#e2e8f0',
    300: '#cbd5e1',
    400: '#94a3b8',
    500: '#64748b',
    600: '#475569',
    700: '#334155',
    800: '#1e293b',
    900: '#0f172a',
  },
  gray: {
    50: '#f9fafb',
    100: '#f3f4f6',
    200: '#e5e7eb',
    300: '#d1d5db',
    400: '#9ca3af',
    500: '#6b7280',
    600: '#4b5563',
    700: '#374151',
    800: '#1f2937',
    900: '#111827',
  },
  success: {
    50: '#ecfdf5',
    500: '#10b981',
    900: '#064e3b',
  },
  warning: {
    50: '#fffbeb',
    500: '#f59e0b',
    900: '#78350f',
  },
  error: {
    50: '#fef2f2',
    300: '#fca5a5',
    500: '#ef4444',
    600: '#dc2626',
    900: '#7f1d1d',
  },
}

const darkPalette = {
  primary: {
    50: '#0f172a',
    100: '#1e293b',
    200: '#334155',
    300: '#475569',
    400: '#64748b',
    500: '#94a3b8',
    600: '#cbd5e1',
    700: '#e2e8f0',
    800: '#f1f5f9',
    900: '#f8fafc',
  },
  secondary: {
    50: '#134e4a',
    100: '#115e59',
    200: '#0f766e',
    300: '#0d9488',
    400: '#14b8a6',
    500: '#2dd4bf',
    600: '#5eead4',
    700: '#99f6e4',
    800: '#ccfbf1',
    900: '#f0fdfa',
  },
  accent: {
    50: '#0f172a',
    100: '#1e293b',
    200: '#334155',
    300: '#475569',
    400: '#64748b',
    500: '#94a3b8',
    600: '#cbd5e1',
    700: '#e2e8f0',
    800: '#f1f5f9',
    900: '#f8fafc',
  },
  gray: {
    50: '#111827',
    100: '#1f2937',
    200: '#374151',
    300: '#4b5563',
    400: '#6b7280',
    500: '#9ca3af',
    600: '#d1d5db',
    700: '#e5e7eb',
    800: '#f3f4f6',
    900: '#f9fafb',
  },
  success: {
    50: '#064e3b',
    500: '#34d399',
    900: '#ecfdf5',
  },
  warning: {
    50: '#78350f',
    500: '#fbbf24',
    900: '#fffbeb',
  },
  error: {
    50: '#7f1d1d',
    300: '#f87171',
    500: '#fca5a5',
    600: '#fecaca',
    900: '#fef2f2',
  },
}

/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  darkMode: 'class',
  theme: {
    extend: {
      fontFamily: {
        custom: ['IBM Plex Sans', 'sans-serif'],
      },
      colors: {
        light: flattenPalette(lightPalette),
        dark: flattenPalette(darkPalette),
      },
    },
  },
  plugins: [],
}