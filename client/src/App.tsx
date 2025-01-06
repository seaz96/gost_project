import {Suspense, useEffect} from "react";
import {BrowserRouter} from "react-router-dom";
import {useAppDispatch, useAppSelector} from "./app/hooks.ts";
import {fetchUser} from "./features/user/userSlice.ts";
import AppRouter from "./pages/AppRouter.tsx";
import {Loader} from "./shared/components";
import "./index.scss";

export const App = () => {
	const dispatch = useAppDispatch();
	const loading = useAppSelector((state) => state.user.loading);

	useEffect(() => {
		if (localStorage.getItem("jwt_token")) {
			dispatch(fetchUser());
		}
	}, [dispatch]);

	if (loading) return <Loader />;

	return (
		<BrowserRouter>
			<Suspense fallback={<Loader />}>
				<AppRouter />
			</Suspense>
		</BrowserRouter>
	);
};