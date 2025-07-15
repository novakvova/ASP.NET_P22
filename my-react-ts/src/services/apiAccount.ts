import {createApi} from '@reduxjs/toolkit/query/react';
import {createBaseQuery} from '../utilities/createBaseQuery.ts';
import {serialize} from "object-to-formdata";
import type {IAuthResponse, IRegister} from "./types.ts";
import {loginSuccess} from "../store/authSlice.ts";
import type {Dispatch} from "@reduxjs/toolkit";
import type {RootState} from "../store";
import {apiCart} from "./apiCart.ts";
import {clearCart} from "../store/localCartSlice.ts";

export interface ILoginRequest {
    email: string;
    password: string;
}

interface ILoginResponse {
    token: string;
}

export interface IForgotPasswordRequest {
    email: string;
}

export interface IValidateTokenRequest {
    token: string;
    email: string;
}

export interface IResetPasswordRequest {
    newPassword: string;
    token: string;
    email: string;
}

// 🔄 Уніфікована логіка обробки авторизації
const handleAuthSuccess = async (
    queryFulfilled: Promise<{ data: IAuthResponse }>,
    dispatch: Dispatch,
    getState: () => RootState
) => {
    try {
        const { data } = await queryFulfilled;
        if (data?.token) {
            dispatch(loginSuccess(data.token));

            const localCart = getState().localCart.items;
            if (localCart.length > 0) {
                await dispatch(apiCart.endpoints.addToCartsRange.initiate(localCart)).unwrap();
            }

            dispatch(clearCart());
        }
    } catch (error) {
        console.error('Auth error:', error);
    }
};

export const apiAccount = createApi({
    reducerPath: 'api/account',
    baseQuery: createBaseQuery('account'),
    tagTypes: ['Account'],
    endpoints: (builder) => ({
        login: builder.mutation<IAuthResponse, ILoginRequest>({
            query: (credentials) => ({
                url: 'login',
                method: 'POST',
                body: credentials,
            }),
            onQueryStarted: async (_arg, { dispatch, getState, queryFulfilled }) =>
                handleAuthSuccess(queryFulfilled, dispatch, getState)


        }),
        loginByGoogle: builder.mutation<IAuthResponse, string>({
            query: (token) => ({
                url: 'googleLogin',
                method: 'POST',
                body: {token}
            }),
            onQueryStarted: async (_arg, { dispatch, getState, queryFulfilled }) =>
                handleAuthSuccess(queryFulfilled, dispatch, getState)
        }),
        //запускаємо процедуру відновлення паролю по пошті
        forgotPassword: builder.mutation<void, IForgotPasswordRequest>({
            query: (data) => ({
                url: 'forgotPassword',
                method: 'POST',
                body: data
            })
        }),
        //перевіряємо чи токен дійсний

        validateResetToken: builder.query<{ isValid: boolean }, IValidateTokenRequest>({
            query: (params) => ({
                url: 'validateResetToken',
                params, // це додасть параметри як query string: ?token=abc&email=...
            }),
            providesTags: ['Account'],
        }),

        // validateResetToken: builder.query<{isValid: boolean}, IValidateTokenRequest>({
        //     query: (data) => ({
        //         url: 'validateResetToken',
        //         method: 'GET',
        //         body: {data}
        //     })
        // }),

        //встановлюємо новий пароль
        resetPassword: builder.mutation<void, IResetPasswordRequest>({
            query: (data) => ({
                url: 'resetPassword',
                method: 'POST',
                body: data
            })
        }),

        register: builder.mutation<ILoginResponse, IRegister>({
            query: (credentials) => {
                const formData = serialize(credentials);

                return {
                    url: 'register',
                    method: 'POST',
                    body: formData
                };
            },
        }),
    }),
});

export const {
    useLoginMutation,
    useLoginByGoogleMutation,
    useForgotPasswordMutation,
    useValidateResetTokenQuery,
    useResetPasswordMutation,
    useRegisterMutation
} = apiAccount;