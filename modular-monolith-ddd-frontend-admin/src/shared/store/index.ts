/* Global state management */
import { configureStore } from '@reduxjs/toolkit';
import {
    FLUSH,
    REHYDRATE,
    PAUSE,
    PERSIST,
    PURGE,
    REGISTER,
} from 'redux-persist';
import { combineReducers } from '@reduxjs/toolkit';
// Global reducers
import utilReducer from './reducers/util';
import userReducer from './reducers/user';
import { dashboardAPI } from './api';

const rootReducer = combineReducers({
    util: utilReducer,
    user: userReducer,
    [dashboardAPI.reducerPath]: dashboardAPI.reducer,   
    // userAccess: useccessStore.reducer, // Lazy load
  });

const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({
            serializableCheck: {
                ignoredActions: [
                    FLUSH,
                    REHYDRATE,
                    PAUSE,
                    PERSIST,
                    PURGE,
                    REGISTER,
                ],
            },
        }).concat(dashboardAPI.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
