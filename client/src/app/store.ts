import { type Middleware, type MiddlewareAPI, configureStore, isRejectedWithValue } from "@reduxjs/toolkit";
import type { FetchBaseQueryError } from "@reduxjs/toolkit/query";
import { apiSlice } from "features/api/apiSlice.ts";
import { toast } from "react-toastify";
import userReducer from "../features/user/userSlice.ts";

export const rtkQueryErrorLogger: Middleware = (_: MiddlewareAPI) => (next) => (action) => {
	if (isRejectedWithValue(action)) {
		console.warn("Rejected action:", action);
		const payload = action.payload as FetchBaseQueryError | undefined;
		if (payload?.status) {
			switch (payload.status) {
				case 400:
					toast.error("Ошибка запроса (400)");
					break;
				case 401:
					toast.error("Ошибка авторизации (401)");
					break;
				case 403:
					toast.error("Ошибка доступа (403)");
					break;
				case 404:
					toast.error("Документ не найден (404)");
					break;
				default:
					if (typeof payload.status === "number" && payload.status >= 500)
						toast.error(`Ошибка сервера (${payload.status})`);
					else toast.error(`Неизвестная ошибка (${payload.status})`);
			}
		} else {
			if (action.error.message) {
				toast.error(`Ошибка запроса: ${action.error.message}`);
			} else toast.error("Неизвестная ошибка запроса");
		}
	}
	return next(action);
};

export const store = configureStore({
	reducer: {
		user: userReducer,
		[apiSlice.reducerPath]: apiSlice.reducer,
	},
	middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(apiSlice.middleware).concat(rtkQueryErrorLogger),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;