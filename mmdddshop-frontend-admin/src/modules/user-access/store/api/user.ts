import userAPI from '../api';

export const userEndpoints = userAPI.injectEndpoints({
    endpoints: (builder) => ({
        userSetPassword: builder.mutation<
            any,
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
