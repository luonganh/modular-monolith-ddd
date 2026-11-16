// src/modules/user-access/store/sagas/authSaga.ts
import { call, put, take, takeLatest } from 'redux-saga/effects';
import type { SagaIterator } from 'redux-saga';
import type { User as OidcUser } from 'oidc-client-ts';
import {
  setUser,
  setLoading,
  setError,
  setInitialized,
  clearAuth,
  toAuthUser
} from '../slices/authSlice';
import {
  login,
  logout,
  handleCallback,
  getUser,
  isAuthenticated
} from '../../services/AuthenticationService';
import { getErrorMessage } from 'shared/utils/errors';

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

type HandleCallbackRequestAction = ReturnType<typeof handleCallbackRequest>;
type HandleCallbackResult = Awaited<ReturnType<typeof handleCallback>>;

// Sagas với return type annotations
function* loginSaga(): SagaIterator<void> {
  try {
    yield put(setLoading(true));
    yield put(setError(null));

    yield call(login);
    // Login will redirect, so we don't need to set user here
  } catch (error: unknown) {
    yield put(setError(getErrorMessage(error, 'Login failed')));
  } finally {
    yield put(setLoading(false));
  }
}

function* logoutSaga(): SagaIterator<void> {
  try {
    yield put(setLoading(true));
    yield call(logout);
    yield put(clearAuth());
  } catch (error: unknown) {
    yield put(setError(getErrorMessage(error, 'Logout failed')));
  } finally {
    yield put(setLoading(false));
  }
}

function* checkAuthSaga(): SagaIterator<void> {
  try {
    yield put(setLoading(true));
    const isAuth: boolean = yield call(isAuthenticated);

    if (isAuth) {
      const user: OidcUser | null = yield call(getUser);
      yield put(setUser(user ? toAuthUser(user) : null));
    } else {
      yield put(setUser(null));
    }
  } catch (error: unknown) {
    yield put(setError(getErrorMessage(error, 'Auth check failed')));
    yield put(setUser(null));
  } finally {
    yield put(setLoading(false));
    yield put(setInitialized(true));
  }
}

function* handleCallbackSaga(): SagaIterator<void> {
// (action: PayloadAction<string>)
  try {
    yield put(setLoading(true));
    yield put(setError(null));

    const action: HandleCallbackRequestAction = yield take(AUTH_ACTIONS.HANDLE_CALLBACK_REQUEST);
    if (!action.payload) {
      throw new Error('Callback URL is required');
    }

    const result: HandleCallbackResult = yield call(handleCallback, action.payload);
    yield put(setUser(toAuthUser(result.user)));

    // Redirect to return URL
    window.location.href = result.returnUrl;
  } catch (error: unknown) {
    yield put(setError(getErrorMessage(error, 'Callback handling failed')));
  } finally {
    yield put(setLoading(false));
  }
}

function* initializeAuthSaga(): SagaIterator<void> {
  try {
    yield put(setLoading(true));
    const isAuth: boolean = yield call(isAuthenticated);

    if (isAuth) {
      const user: OidcUser | null = yield call(getUser);
      yield put(setUser(user ? toAuthUser(user) : null));
    }
  } catch (error: unknown) {
    yield put(setError(getErrorMessage(error, 'Auth initialization failed')));
  } finally {
    yield put(setLoading(false));
    yield put(setInitialized(true));
  }
}

// Root saga
export function* authSaga(): SagaIterator<void> {
  yield takeLatest(AUTH_ACTIONS.LOGIN_REQUEST, loginSaga);
  yield takeLatest(AUTH_ACTIONS.LOGOUT_REQUEST, logoutSaga);
  yield takeLatest(AUTH_ACTIONS.CHECK_AUTH_REQUEST, checkAuthSaga);
  yield takeLatest(AUTH_ACTIONS.HANDLE_CALLBACK_REQUEST, handleCallbackSaga);
  yield takeLatest(AUTH_ACTIONS.INITIALIZE_AUTH, initializeAuthSaga);
}
