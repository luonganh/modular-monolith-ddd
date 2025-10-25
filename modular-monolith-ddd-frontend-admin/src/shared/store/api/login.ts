import dashboardAPI from './index';

interface LoginUserInterface {
    email: string;
    password: string;
}

export const loginAPI = dashboardAPI.injectEndpoints({
    endpoints: (builder) => ({
        login: builder.mutation<any, LoginUserInterface>({
            query: ({ email, password }) => ({
                url: `auth/login`,
                method: 'POST',
                body: { email, password },
            }),
            transformResponse(response: any) {
                if (response?.data) return response.data;
                return response;
            },
        }),
    }),
});

export const { useLoginMutation } = loginAPI;
