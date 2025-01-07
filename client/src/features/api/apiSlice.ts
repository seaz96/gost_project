import type { BaseQueryFn, FetchArgs, FetchBaseQueryError } from "@reduxjs/toolkit/query";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { toast } from "react-toastify";
import type { UserAuthorization } from "../../components/AuthorizationForm/authorizationModel.ts";
import type { UserRegistration } from "../../components/RegistrationForm/registrationModel.ts";
import type { UserEditType } from "../../components/UserEditForm/userEditModel.ts";
import type {
	GostChanges,
	GostFetchModel,
	GostRequestModel,
	GostViewInfo,
	GostViews,
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

const baseQueryWithErrorHandling: BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> = async (
	args,
	api,
	extraOptions,
) => {
	const result = await baseQueryWithAuth(args, api, extraOptions);
	if (result.error) {
		const errorMessage = (result.error.data as { message?: string })?.message || result.error.status;
		toast.error(`Ошибка получения данных (${errorMessage})`);
	}
	return result;
};

export const apiSlice = createApi({
	reducerPath: "api",
	baseQuery: baseQueryWithErrorHandling,
	endpoints: (builder) => ({
		fetchUser: builder.query<User, void>({
			query: () => ({
				url: "/accounts/self-info",
			}),
		}),
		loginUser: builder.mutation<User, UserAuthorization>({
			query: (credentials) => ({
				url: "/accounts/login",
				method: "POST",
				body: credentials,
			}),
		}),
		registerUser: builder.mutation<User, UserRegistration>({
			query: (credentials) => ({
				url: "/accounts/register",
				method: "POST",
				body: credentials,
			}),
		}),
		fetchUsers: builder.query<User[], void>({
			query: () => "/admin/users",
			keepUnusedDataFor: 0,
		}),
		fetchUserInfo: builder.query<User, number>({
			query: (id) => ({
				url: `/admin/users/${id}`,
			}),
			keepUnusedDataFor: 0,
		}),
		editUser: builder.mutation<void, UserEditType & { id: number }>({
			query: (userData) => ({
				url: "/admin/edit-user",
				method: "POST",
				body: userData,
			}),
		}),
		toggleAdmin: builder.mutation<void, { userId: number; isAdmin: boolean }>({
			query: (data) => ({
				url: "/admin/make-admin",
				method: "POST",
				body: data,
			}),
		}),
		editSelf: builder.mutation<void, UserEditType>({
			query: (userData) => ({
				url: "/accounts/edit",
				method: "POST",
				body: userData,
			}),
		}),
		fetchGost: builder.query<GostFetchModel, string>({
			query: (id) => ({
				url: `/docs/${id}`,
			}),
			keepUnusedDataFor: 0,
		}),
		addGost: builder.mutation<void, GostRequestModel>({
			query: (gost) => ({
				url: "/docs/add",
				method: "POST",
				body: gost,
			}),
		}),
		changeGostStatus: builder.mutation<void, { id: string | number; status: number }>({
			query: ({ id, status }) => ({
				url: "/docs/change-status",
				method: "PUT",
				body: { id, status },
				responseHandler: (response) => response.text(),
			}),
		}),
		uploadGostFile: builder.mutation<void, { docId: string; file: File }>({
			query: ({ docId, file }) => {
				const formData = new FormData();
				formData.append("File", file);
				return {
					url: `/docs/${docId}/upload-file`,
					method: "POST",
					body: formData,
				};
			},
		}),
		updateGost: builder.mutation<void, { id: string; gost: GostRequestModel }>({
			query: ({ id, gost }) => ({
				url: `/docs/update/${id}`,
				method: "PUT",
				body: gost,
			}),
		}),
		actualizeGost: builder.mutation<void, { id: string; gost: GostRequestModel }>({
			query: ({ id, gost }) => ({
				url: `/docs/actualize/${id}`,
				method: "PUT",
				body: gost,
				params: { docId: id },
			}),
		}),
		resetPassword: builder.mutation<void, { login: string; old_password: string; new_password: string }>({
			query: (credentials) => ({
				url: "/accounts/change-password",
				method: "POST",
				body: credentials,
			}),
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
			keepUnusedDataFor: 0,
		}),
		getChangesStats: builder.query<GostChanges[], { status: string; StartDate: string; EndDate: string }>({
			query: (params) => ({
				url: "/actions/list",
				params,
			}),
			keepUnusedDataFor: 0,
		}),
		deleteGost: builder.mutation<void, string>({
			query: (id) => ({
				url: `/docs/delete/${id}`,
				method: "DELETE",
			}),
		}),
		fetchGostsPage: builder.query<GostViewInfo[], { url: string; offset: number; limit: number; params?: object }>({
			query: ({ url, offset, limit, params }) => ({
				url,
				params: { ...flattenParams(params), offset, limit },
			}),
			keepUnusedDataFor: 0,
		}),
		fetchGostsCount: builder.query<number, { url: string; params?: object }>({
			query: ({ url, params }) => ({
				url: `${url}-count`,
				params: flattenParams(params),
			}),
			keepUnusedDataFor: 0,
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
	useUploadGostFileMutation,
	useUpdateGostMutation,
	useActualizeGostMutation,
	useResetPasswordMutation,
	useLazyGetViewsStatsQuery,
	useLazyGetChangesStatsQuery,
	useDeleteGostMutation,
	useFetchGostsPageQuery,
	useFetchGostsCountQuery,
} = apiSlice;
