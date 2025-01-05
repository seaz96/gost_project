import { type PayloadAction, createSlice } from "@reduxjs/toolkit";
import type { User } from "../userModel.ts";

interface UserState {
	user: User | null;
	loading: boolean;
}

const initialState: UserState = {
	user: null,
	loading: false,
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
});

export const { setUser, setLoading } = userSlice.actions;
export default userSlice.reducer;
