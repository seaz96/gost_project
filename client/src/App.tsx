import { Suspense } from "react";
import { BrowserRouter } from "react-router-dom";
import { useAppSelector } from "./app/hooks.ts";
import AppRouter from "./pages/AppRouter.tsx";
import { Loader } from "./shared/components";
import "./styles/index.scss";

export const App = () => {
	const loading = useAppSelector((state) => state.user.loading);

	if (loading) return <Loader />;

	return (
		<BrowserRouter>
			<Suspense fallback={<Loader />}>
				<AppRouter />
			</Suspense>
		</BrowserRouter>
	);
};
