import { configureStore } from "@reduxjs/toolkit";
import { apiSlice } from "features/api/apiSlice.ts";
import userReducer from "../features/user/userSlice.ts";

export const store = configureStore({
	reducer: {
		user: userReducer,
		[apiSlice.reducerPath]: apiSlice.reducer,
	},
	middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(apiSlice.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
