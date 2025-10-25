import {
    createApi,
    fetchBaseQuery,
} from '@reduxjs/toolkit/query/react';
import type {
    BaseQueryFn,
    FetchArgs,
    FetchBaseQueryError,
} from '@reduxjs/toolkit/query';
import type { RootState } from '../index';

import { clearUser } from '../reducers/user';

const baseQuery = fetchBaseQuery({
    baseUrl: import.meta.env.DASHBOARD_API,
    prepareHeaders: (headers, { getState }) => {
        const token = (getState() as RootState).user.token;
        if (token) {
            headers.set('authorization', `Bearer ${token}`);
        }
        return headers;
    },
});

const baseQueryWithReauth: BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
> = async (args, api, extraOptions) => {
    const result = await baseQuery(args, api, extraOptions);
    if (result.error && result.error.status === 401) {
        api.dispatch(clearUser());
    }
    return result;
};

export const dashboardAPI = createApi({
    reducerPath: 'dashboardAPI',
    baseQuery: baseQueryWithReauth,
    endpoints: () => ({}),
});

export default dashboardAPI;
