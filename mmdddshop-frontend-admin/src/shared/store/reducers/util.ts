import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

export interface UtilState {
    theme: 'light' | 'dark';
    isLoading: boolean;
    isSidebarOpen: boolean;        // desktop: mở rộng (true) / thu gọn (false)
    isMobileSidebarOpen: boolean;  // mobile: hiện (true) / ẩn (false)
    isNotificationBarOpen: boolean;
}

const initialState: UtilState = {
    theme: 'light',
    isLoading: false,
    isSidebarOpen: true,
    isMobileSidebarOpen: false,
    isNotificationBarOpen: false,
};

const utilSliece = createSlice({
    name: 'util',
    initialState,
    reducers: {
        setLoading: (state, action) => {
            state.isLoading = action.payload;
        },
        setSidebarOpen: (state, action) => {
            state.isSidebarOpen = action.payload;
        },
        setMobileSidebarOpen: (state, action: PayloadAction<boolean>) => {
            state.isMobileSidebarOpen = action.payload;
        },
        setNotificationBarOpen: (state, action) => {
            state.isNotificationBarOpen = action.payload;
        },
        toggleTheme: (state) => {
            state.theme = state.theme === 'light' ? 'dark' : 'light';
        },
    },
});

export const {
    setLoading,
    setSidebarOpen,
    setNotificationBarOpen,
    setMobileSidebarOpen,
    toggleTheme,
} = utilSliece.actions;

export default utilSliece.reducer;
