import { type PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios from "axios";
import type { User } from "../../entities/user/model/userModel.ts";

export const fetchUser = createAsyncThunk<User, number>(
	"user/fetchUser",
	async (userId) => {
		const response = await axios.get(`/api/users/${userId}`);
		return response.data;
	}
);

export const loginUser = createAsyncThunk<User, { login: string; password: string }>(
	"user/loginUser",
	async (credentials) => {
		const response = await axios.post("/api/login", credentials);
		return response.data;
	}
);

export const logoutUser = createAsyncThunk<void, void>(
	"user/logoutUser",
	async () => {
		localStorage.removeItem("jwt_token");
	}
);

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
		},
		logout: (state) => {
			state.user = null;
		},
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
			})
			.addCase(loginUser.rejected, (state, action) => {
				state.loading = false;
				state.error = action.error.message || "Failed to login";
			})
			.addCase(logoutUser.pending, (state) => {
				state.loading = true;
				state.error = null;
			})
			.addCase(logoutUser.fulfilled, (state) => {
				state.loading = false;
				state.user = null;
			})
	},
});

export const { setUser, setLoading } = userSlice.actions;
export default userSlice.reducer;