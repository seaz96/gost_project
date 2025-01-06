import {type PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type {User} from "../../entities/user/userModel.ts";
import {apiSlice} from "../api/apiSlice.ts";

interface UserState {
	user: User | null;
	loading: boolean;
	error: string | null;
}

const initialState: UserState = {
	user: null,
	loading: false,
	error: null,
};

export const logoutUser = createAsyncThunk<void, void>('user/logoutUser', async (_, { dispatch }) => {
	localStorage.removeItem('jwt_token');
	dispatch(setUser(null));
	window.location.href = '/login';
});

export const userSlice = createSlice({
	name: 'user',
	initialState,
	reducers: {
		setUser: (state, action: PayloadAction<User | null>) => {
			state.user = action.payload;
		},
		setLoading: (state, action: PayloadAction<boolean>) => {
			state.loading = action.payload;
		},
	},
	extraReducers: (builder) => {
		builder
			.addCase(logoutUser.fulfilled, (state: UserState) => {
				state.user = null;
			})
			.addMatcher(apiSlice.endpoints.fetchUser.matchPending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addMatcher(apiSlice.endpoints.fetchUser.matchFulfilled, (state, action: PayloadAction<User>) => {
				state.loading = false;
				state.user = action.payload;
			})
			.addMatcher(apiSlice.endpoints.fetchUser.matchRejected, (state, action) => {
				state.loading = false;
				state.error = `Ошибка загрузки данных пользователя (${action.error.message})`;
			})
			.addMatcher(apiSlice.endpoints.loginUser.matchPending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addMatcher(apiSlice.endpoints.loginUser.matchFulfilled, (state, action: PayloadAction<User>) => {
				state.loading = false;
				state.user = action.payload;
				state.error = null;
				localStorage.setItem('jwt_token', action.payload.token);
			})
			.addMatcher(apiSlice.endpoints.loginUser.matchRejected, (state, action) => {
				state.loading = false;
				state.user = null;
				state.error = action.error.code === 'ERR_BAD_REQUEST' ? "Неверный логин или пароль" : `Ошибка входа в систему (${action.error.message})`;
			})
			.addMatcher(apiSlice.endpoints.registerUser.matchPending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addMatcher(apiSlice.endpoints.registerUser.matchFulfilled, (state, action: PayloadAction<User>) => {
				state.loading = false;
				state.user = action.payload;
				state.error = null;
				localStorage.setItem('jwt_token', action.payload.token);
			})
			.addMatcher(apiSlice.endpoints.registerUser.matchRejected, (state, action) => {
				state.loading = false;
				state.user = null;
				state.error = action.error.code === 'ERR_BAD_REQUEST' ? "Пользователь с таким логином уже существует" : `Ошибка регистрации (${action.error.message})`;
			});
	},
});

export const { setUser, setLoading } = userSlice.actions;
export default userSlice.reducer;