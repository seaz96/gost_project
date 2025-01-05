import {type PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios from "axios";
import type {User} from "../../entities/user/userModel.ts";

export const fetchUser = createAsyncThunk<User, void>("user/fetchUser", async () => {
	const response = await axios.get("/accounts/self-info");
	return response.data;
});

export const loginUser = createAsyncThunk<User, { login: string; password: string }>(
	"user/loginUser",
	async (credentials) => {
		const response = await axios.post("/accounts/login", credentials);
		localStorage.setItem("jwt_token", response.data.token);
		return response.data;
	},
);

export const registerUser = createAsyncThunk<User, { login: string; password: string }>(
	"user/registerUser",
	async (credentials) => {
		const response = await axios.post("/accounts/register", credentials);
		localStorage.setItem("jwt_token", response.data.token);
		return response.data;
	},
);

export const logoutUser = createAsyncThunk<void, void>("user/logoutUser", async () => {
	localStorage.removeItem("jwt_token");
	window.location.href = "/login";
});

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

export const userSlice = createSlice({
	name: "user",
	initialState,
	reducers: {
		setUser: (state, action: PayloadAction<User | null>) => {
			state.user = action.payload;
		},
		setLoading: (state, action: PayloadAction<boolean>) => {
			state.loading = action.payload;
		}
	},
	extraReducers: (builder) => {
		builder
			.addCase(fetchUser.pending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addCase(fetchUser.fulfilled, (state, action: PayloadAction<User>) => {
				state.loading = false;
				state.user = action.payload;
			})
			.addCase(fetchUser.rejected, (state, action) => {
				state.loading = false;
				state.error = action.error.message || "Failed to fetch user";
			})
			.addCase(loginUser.pending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addCase(loginUser.fulfilled, (state, action: PayloadAction<User>) => {
				state.loading = false;
				state.user = action.payload;
				state.error = null;
			})
			.addCase(loginUser.rejected, (state, action) => {
				state.loading = false;
				state.user = null;
				state.error = action.error.message || "Invalid credentials";
			})
			.addCase(logoutUser.pending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addCase(logoutUser.fulfilled, (state) => {
				state.loading = false;
				state.user = null;
			})
			.addCase(registerUser.pending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addCase(registerUser.fulfilled, (state, action: PayloadAction<User>) => {
				state.loading = false;
				state.user = action.payload;
				state.error = null;
			})
			.addCase(registerUser.rejected, (state, action) => {
				state.loading = false;
				state.user = null;
				state.error = action.error.message || "Failed to register";
			});
	},
});

export const { setUser, setLoading } = userSlice.actions;
export default userSlice.reducer;