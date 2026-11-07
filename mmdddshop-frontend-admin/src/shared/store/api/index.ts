import {
    createApi,
    fetchBaseQuery,
} from '@reduxjs/toolkit/query/react';
import type {
    BaseQueryFn,
    FetchArgs,
    FetchBaseQueryError,
} from '@reduxjs/toolkit/query';
import { getAccessToken, refreshAccessToken } from '../../../modules/user-access/services/AuthenticationService';

import { clearUser } from '../reducers/user';

const baseQuery = fetchBaseQuery({
    baseUrl: import.meta.env.DASHBOARD_API,
    prepareHeaders: async (headers) => {
        const token = await getAccessToken();
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
    let result = await baseQuery(args, api, extraOptions);
    if (result.error && result.error.status === 401) {
        const rejectedToken = result.meta?.request.headers.get('authorization')?.replace(/^Bearer /, '');
        try {
            const token = await refreshAccessToken(rejectedToken);
            if (token) result = await baseQuery(args, api, extraOptions);
            else api.dispatch(clearUser());
        } catch {
            // Return the original 401 on transient refresh failures.
        }
    }
    return result;
};

export const dashboardAPI = createApi({
    reducerPath: 'dashboardAPI',
    baseQuery: baseQueryWithReauth,
    endpoints: () => ({}),
});

export default dashboardAPI;

