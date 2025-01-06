import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type {UserAuthorization} from "../../components/AuthorizationForm/authorizationModel.ts";
import type {UserRegistration} from "../../components/RegistrationForm/registrationModel.ts";
import type { User } from '../../entities/user/userModel';
import {baseURL} from "../../shared/configs/apiConfig.ts";

const baseQueryWithAuth = fetchBaseQuery({
    baseUrl: baseURL,
    prepareHeaders: (headers) => {
        const token = localStorage.getItem('jwt_token');
        if (token) {
            headers.set('Authorization', `Bearer ${token}`);
        }
        return headers;
    },
});

export const apiSlice = createApi({
    reducerPath: 'api',
    baseQuery: baseQueryWithAuth,
    endpoints: (builder) => ({
        fetchUser: builder.query<User, void>({
            query: () => ({
                url: '/accounts/self-info'
            })
        }),
        loginUser: builder.mutation<User, UserAuthorization>({
            query: (credentials) => ({
                url: '/accounts/login',
                method: 'POST',
                body: credentials,
            }),
        }),
        registerUser: builder.mutation<User, UserRegistration>({
            query: (credentials) => ({
                url: '/accounts/register',
                method: 'POST',
                body: credentials,
            }),
        })
    }),
});

export const { useFetchUserQuery, useLoginUserMutation, useRegisterUserMutation } = apiSlice;