import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { UserAuthorization } from "../../components/AuthorizationForm/authorizationModel.ts";
import type { UserRegistration } from "../../components/RegistrationForm/registrationModel.ts";
import type { UserEditType } from "../../components/UserEditForm/userEditModel.ts";
import type {
	GostAddModel,
	GostChanges,
	GostFetchModel,
	GostViewInfo,
	GostViews,
	documentStatus,
} from "../../entities/gost/gostModel.ts";
import type { User } from "../../entities/user/userModel";
import { baseURL } from "../../shared/configs/apiConfig.ts";

type FlattenedParams = Record<string, string>;

const flattenParams = <T extends object>(obj: T | undefined, prefix = ""): FlattenedParams => {
	const flattened: FlattenedParams = {};
	if (!obj) {
		return flattened;
	}
	for (const [key, value] of Object.entries(obj)) {
		const fullKey = prefix ? `${prefix}.${key}` : key;

		if (value === null || value === undefined) {
			continue;
		}

		if (typeof value === "object" && !Array.isArray(value)) {
			Object.assign(flattened, flattenParams(value, fullKey));
		} else {
			flattened[fullKey] = String(value);
		}
	}
	return flattened;
};

const toFormData = (obj: Record<string, object | string | number | Blob>) => {
	console.log(obj);
	const formData = new FormData();
	for (const [key, value] of Object.entries(obj)) {
		if (value === null || value === undefined || Number.isNaN(value)) {
			continue;
		}
		if (isFile(value)) {
			formData.append(key, value, (value as File).name);
			continue;
		}
		if (isBlob(value)) {
			formData.append(key, value);
			continue;
		}
		if (Array.isArray(value)) {
			for (const [index, arrayValue] of value.entries()) {
				formData.append(`${key}[${index}]`, arrayValue);
			}
			continue;
		}
		if (typeof value === "object" && !Array.isArray(value)) {
			for (const [nestedKey, nestedValue] of Object.entries(value)) {
				formData.append(`${key}.${nestedKey}`, nestedValue);
			}
			continue;
		}
		if (typeof value === "number") {
			formData.append(key, value.toString());
			continue;
		}
		if (typeof value === "string") {
			formData.append(key, value);
			//continue;
		}
		//formData.append(key, value);
	}
	return formData;
};

function isBlob(value: unknown): value is Blob {
	return value instanceof Blob;
}

function isFile(value: unknown): value is File {
	return value instanceof File;
}

const baseQueryWithAuth = fetchBaseQuery({
	baseUrl: baseURL,
	prepareHeaders: (headers) => {
		const token = localStorage.getItem("jwt_token");
		if (token) {
			headers.set("Authorization", `Bearer ${token}`);
		}
		return headers;
	},
});

export const apiSlice = createApi({
	reducerPath: "api",
	baseQuery: baseQueryWithAuth,
	tagTypes: ["User", "Gost"],
	keepUnusedDataFor: 600,
	endpoints: (builder) => ({
		fetchUser: builder.query<User, void>({
			query: () => ({
				url: "/accounts/self-info",
			}),
			providesTags: ["User"],
		}),
		loginUser: builder.mutation<User, UserAuthorization>({
			query: (credentials) => ({
				url: "/accounts/login",
				method: "POST",
				body: credentials,
				keepUnusedDataFor: 0,
			}),
			invalidatesTags: ["User"],
		}),
		registerUser: builder.mutation<User, UserRegistration>({
			query: (credentials) => ({
				url: "/accounts/register",
				method: "POST",
				body: credentials,
				keepUnusedDataFor: 0,
			}),
			invalidatesTags: ["User"],
		}),
		fetchUsers: builder.query<User[], void>({
			query: () => "/admin/users",
			providesTags: ["User"],
		}),
		fetchUserInfo: builder.query<User, number>({
			query: (id) => ({
				url: `/admin/users/${id}`,
			}),
			providesTags: ["User"],
		}),
		editUser: builder.mutation<void, UserEditType & { id: number }>({
			query: (userData) => ({
				url: "/admin/edit-user",
				method: "POST",
				body: userData,
			}),
			invalidatesTags: ["User"],
		}),
		toggleAdmin: builder.mutation<void, { userId: number; isAdmin: boolean }>({
			query: (data) => ({
				url: "/admin/make-admin",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["User"],
		}),
		editSelf: builder.mutation<void, UserEditType>({
			query: (userData) => ({
				url: "/accounts/edit",
				method: "POST",
				body: userData,
			}),
			invalidatesTags: ["User"],
		}),
		fetchGost: builder.query<GostFetchModel, string>({
			query: (id) => ({
				url: `/docs/${id}`,
			}),
			providesTags: ["Gost"],
		}),
		addGost: builder.mutation<void, GostAddModel>({
			query: (gost) => {
				return {
					url: "/docs/add",
					method: "POST",
					body: toFormData(gost),
				};
			},
			invalidatesTags: ["Gost"],
		}),
		changeGostStatus: builder.mutation<void, { id: string | number; status: documentStatus }>({
			query: ({ id, status }) => ({
				url: "/docs/change-status",
				method: "PUT",
				body: { id, status },
			}),
			invalidatesTags: ["Gost"],
		}),
		updateGost: builder.mutation<void, { id: string; gost: GostAddModel; actualize: boolean }>({
			query: ({ id, gost, actualize }) => ({
				url: `/docs/${actualize ? "actualize" : "update"}/${id}`,
				method: "PUT",
				body: toFormData(gost),
			}),
			invalidatesTags: ["Gost"],
		}),
		resetPassword: builder.mutation<void, { login: string; old_password: string; new_password: string }>({
			query: (credentials) => ({
				url: "/accounts/change-password",
				method: "POST",
				body: credentials,
			}),
			invalidatesTags: ["User"],
		}),
		getViewsStats: builder.query<
			GostViews[],
			{ startDate: string; endDate: string; designation?: string; codeOks?: string; activityField?: string }
		>({
			query: (params) => ({
				url: "/actions/views",
				params: {
					...params,
					StartDate: params.startDate,
					EndDate: params.endDate,
				},
			}),
			keepUnusedDataFor: 60,
			providesTags: ["Gost"],
		}),
		getChangesStats: builder.query<GostChanges[], { status: string; StartDate: string; EndDate: string }>({
			query: (params) => ({
				url: "/actions/list",
				params,
			}),
			keepUnusedDataFor: 60,
			providesTags: ["Gost"],
		}),
		deleteGost: builder.mutation<void, string>({
			query: (id) => ({
				url: `/docs/delete/${id}`,
				method: "DELETE",
			}),
			invalidatesTags: ["Gost"],
		}),
		fetchGostsPage: builder.query<GostViewInfo[], { url: string; offset: number; limit: number; params?: object }>({
			query: ({ url, offset, limit, params }) => ({
				url,
				params: { ...flattenParams(params), offset, limit },
			}),
			providesTags: ["Gost"],
		}),
		fetchGostsCount: builder.query<number, { url: string; params?: object }>({
			query: ({ url, params }) => ({
				url: `${url}-count`,
				params: flattenParams(params),
			}),
			providesTags: ["Gost"],
		}),
	}),
});

export const {
	useFetchUserQuery,
	useLoginUserMutation,
	useRegisterUserMutation,
	useFetchGostQuery,
	useFetchUsersQuery,
	useFetchUserInfoQuery,
	useEditUserMutation,
	useToggleAdminMutation,
	useEditSelfMutation,
	useAddGostMutation,
	useChangeGostStatusMutation,
	useUpdateGostMutation,
	useResetPasswordMutation,
	useLazyGetViewsStatsQuery,
	useLazyGetChangesStatsQuery,
	useDeleteGostMutation,
	useLazyFetchGostsPageQuery,
	useFetchGostsCountQuery,
} = apiSlice;