import dashboardAPI from './index';

interface LoginUserInterface {
    email: string;
    password: string;
}

export const loginAPI = dashboardAPI.injectEndpoints({
    endpoints: (builder) => ({
        // The response shape is not defined by the backend yet, hence unknown.
        login: builder.mutation<unknown, LoginUserInterface>({
            query: ({ email, password }) => ({
                url: `auth/login`,
                method: 'POST',
                body: { email, password },
            }),
            transformResponse(response: unknown) {
                // Unwrap { data: ... } envelopes when present.
                if (
                    typeof response === 'object' &&
                    response !== null &&
                    'data' in response &&
                    response.data
                ) {
                    return response.data;
                }
                return response;
            },
        }),
    }),
});

export const { useLoginMutation } = loginAPI;
