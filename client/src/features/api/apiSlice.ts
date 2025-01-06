import type { BaseQueryFn, FetchArgs, FetchBaseQueryError } from "@reduxjs/toolkit/query";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { toast } from "react-toastify";
import type { UserAuthorization } from "../../components/AuthorizationForm/authorizationModel.ts";
import type { GostToSave } from "../../components/GostForm/newGostModel.ts";
import type { UserRegistration } from "../../components/RegistrationForm/registrationModel.ts";
import type { UserEditType } from "../../components/UserEditForm/userEditModel.ts";
import type { Gost, GostViewInfo } from "../../entities/gost/gostModel.ts";
import type { User } from "../../entities/user/userModel";
import { baseURL } from "../../shared/configs/apiConfig.ts";

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
			query: () => "/accounts/list",
		}),
		fetchUserInfo: builder.query<User, number>({
			query: (id) => ({
				url: "/accounts/get-user-info",
				params: { id },
			}),
		}),
		editUser: builder.mutation<void, UserEditType & { id: number }>({
			query: (userData) => ({
				url: "/accounts/admin-edit",
				method: "POST",
				body: userData,
			}),
		}),
		toggleAdmin: builder.mutation<void, { userId: number; isAdmin: boolean }>({
			query: (data) => ({
				url: "/accounts/make-admin",
				method: "POST",
				body: data,
			}),
		}),
		editSelf: builder.mutation<void, UserEditType>({
			query: (userData) => ({
				url: "/accounts/self-edit",
				method: "POST",
				body: userData,
			}),
		}),
		fetchGost: builder.query<Gost, string>({
			query: (id) => ({
				url: `/docs/${id}`,
			}),
		}),
		addGost: builder.mutation<void, GostToSave>({
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
			}),
		}),
		uploadGostFile: builder.mutation<void, { docId: string; file: File }>({
			query: ({ docId, file }) => {
				const formData = new FormData();
				formData.append("File", file);
				formData.append("Extension", file.name.split(".").pop() || "");
				return {
					url: `/docs/${docId}/upload-file`,
					method: "POST",
					body: formData,
				};
			},
		}),
		updateGost: builder.mutation<void, { id: string; gost: GostToSave }>({
			query: ({ id, gost }) => ({
				url: `/docs/update/${id}`,
				method: "PUT",
				body: gost,
			}),
		}),
		actualizeGost: builder.mutation<void, { id: string; gost: GostToSave }>({
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
			any,
			{ startDate: string; endDate: string; designation?: string; codeOks?: string; activityField?: string }
		>({
			query: (params) => ({
				url: "/stats/get-views",
				params: {
					...params,
					StartDate: params.startDate,
					EndDate: params.endDate,
				},
			}),
		}),
		getChangesStats: builder.query<any, { status: number; count: number; StartDate: string; EndDate: string }>({
			query: (params) => ({
				url: "/stats/get-count",
				params,
			}),
		}),
		updateViews: builder.mutation<void, string>({
			query: (docId) => ({
				url: `/stats/update-views/${docId}`,
				method: "POST",
				params: { docId },
			}),
		}),
		deleteGost: builder.mutation<void, string>({
			query: (id) => ({
				url: `/docs/delete/${id}`,
				method: "DELETE",
			}),
		}),
		fetchGostsPage: builder.query<GostViewInfo[], { url: string; offset: number; limit: number; params?: any }>({
			query: ({ url, offset, limit, params }) => ({
				url,
				params: { ...params, offset, limit },
			}),
		}),
		fetchGostsCount: builder.query<number, { url: string; params?: any }>({
			query: ({ url, params }) => ({
				url: `${url}-count`,
				params,
			}),
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
	useGetViewsStatsQuery,
	useGetChangesStatsQuery,
	useUpdateViewsMutation,
	useDeleteGostMutation,
	useFetchGostsPageQuery,
	useFetchGostsCountQuery,
} = apiSlice;
