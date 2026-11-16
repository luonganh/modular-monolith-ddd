import userAPI from '../api';

export const userEndpoints = userAPI.injectEndpoints({
    endpoints: (builder) => ({
        // The response body is not used, hence unknown.
        userSetPassword: builder.mutation<
            unknown,
            { email: string; password: string; token: string }
        >({
            query: ({ email, password, token }) => ({
                url: `user/set-password/${token}`,
                method: 'POST',
                body: { email, password },
            }),
        }),
    }),
});

export const { useUserSetPasswordMutation } = userEndpoints;
