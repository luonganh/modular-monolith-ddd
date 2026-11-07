// src/modules/user-access/store/sagas/authSaga.ts
import { call, put, take, takeLatest } from 'redux-saga/effects';
import { 
  setUser, 
  setLoading, 
  setError, 
  setInitialized,
  clearAuth 
} from '../slices/authSlice';
import { 
  login, 
  logout, 
  handleCallback, 
  getUser, 
  isAuthenticated 
} from '../../services/AuthenticationService';

// Action types
export const AUTH_ACTIONS = {
  LOGIN_REQUEST: 'auth/loginRequest',
  LOGOUT_REQUEST: 'auth/logoutRequest',
  CHECK_AUTH_REQUEST: 'auth/checkAuthRequest',
  HANDLE_CALLBACK_REQUEST: 'auth/handleCallbackRequest',
  INITIALIZE_AUTH: 'auth/initializeAuth',
} as const;

// Action creators
export const loginRequest = () => ({ type: AUTH_ACTIONS.LOGIN_REQUEST });
export const logoutRequest = () => ({ type: AUTH_ACTIONS.LOGOUT_REQUEST });
export const checkAuthRequest = () => ({ type: AUTH_ACTIONS.CHECK_AUTH_REQUEST });
export const handleCallbackRequest = (callbackUrl: string) => ({ 
  type: AUTH_ACTIONS.HANDLE_CALLBACK_REQUEST, 
  payload: callbackUrl 
});
export const initializeAuth = () => ({ type: AUTH_ACTIONS.INITIALIZE_AUTH });

// Sagas với return type annotations
function* loginSaga(): Generator<any, void, any> {
  try {
    yield put(setLoading(true));
    yield put(setError(null));
    
    yield call(login);
    // Login will redirect, so we don't need to set user here
  } catch (error: any) {
    yield put(setError(error.message || 'Login failed'));
  } finally {
    yield put(setLoading(false));
  }
}

function* logoutSaga(): Generator<any, void, any> {
  try {
    yield put(setLoading(true));
    yield call(logout);
    yield put(clearAuth());
  } catch (error: any) {
    yield put(setError(error.message || 'Logout failed'));
  } finally {
    yield put(setLoading(false));
  }
}

function* checkAuthSaga(): Generator<any, void, any> {
  try {
    yield put(setLoading(true));
    const isAuth: boolean = yield call(isAuthenticated);
    
    if (isAuth) {
      const user: any = yield call(getUser);
      yield put(setUser(user));
    } else {
      yield put(setUser(null));
    }
  } catch (error: any) {
    yield put(setError(error.message || 'Auth check failed'));
    yield put(setUser(null));
  } finally {
    yield put(setLoading(false));
    yield put(setInitialized(true));
  }
}

function* handleCallbackSaga(): Generator<any, void, any> {
// (action: PayloadAction<string>) 
  try {
    yield put(setLoading(true));
    yield put(setError(null));
    
    const action = yield take(AUTH_ACTIONS.HANDLE_CALLBACK_REQUEST);
    if (!action.payload) {
      throw new Error('Callback URL is required');
    }
    
     const result: { user: any; returnUrl: string } = yield call(
        handleCallback as any,        
      action.payload
    );
    //const result: { user: any; returnUrl: string } = yield call(handleCallback, callbackUrl);
    yield put(setUser(result.user));
    
    // Redirect to return URL
    window.location.href = result.returnUrl;
  } catch (error: any) {
    yield put(setError(error.message || 'Callback handling failed'));
  } finally {
    yield put(setLoading(false));
  }
}

function* initializeAuthSaga(): Generator<any, void, any> {
  try {
    yield put(setLoading(true));
    const isAuth: boolean = yield call(isAuthenticated);
    
    if (isAuth) {
      const user: any = yield call(getUser);
      yield put(setUser(user));
    }
  } catch (error: any) {
    yield put(setError(error.message || 'Auth initialization failed'));
  } finally {
    yield put(setLoading(false));
    yield put(setInitialized(true));
  }
}

// Root saga
export function* authSaga(): Generator<any, void, any> {
  yield takeLatest(AUTH_ACTIONS.LOGIN_REQUEST, loginSaga);
  yield takeLatest(AUTH_ACTIONS.LOGOUT_REQUEST, logoutSaga);
  yield takeLatest(AUTH_ACTIONS.CHECK_AUTH_REQUEST, checkAuthSaga);
  yield takeLatest(AUTH_ACTIONS.HANDLE_CALLBACK_REQUEST, handleCallbackSaga);
  yield takeLatest(AUTH_ACTIONS.INITIALIZE_AUTH, initializeAuthSaga);
}