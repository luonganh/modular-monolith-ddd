import { configureStore } from '@reduxjs/toolkit';
import { combineReducers } from '@reduxjs/toolkit';

// Module-specific reducers
import userReducer from '../store/reducers/user';
import { userAPI } from '../store/api';

const moduleReducer = combineReducers({
  user: userReducer,
  [userAPI.reducerPath]: userAPI.reducer,
});

export const userAccessStore = configureStore({
  reducer: moduleReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(userAPI.middleware),
});

export type UserAccessState = ReturnType<typeof userAccessStore.getState>;
export type UserAccessDispatch = typeof userAccessStore.dispatch;