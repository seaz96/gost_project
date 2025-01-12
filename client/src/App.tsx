import { Suspense } from "react";
import { BrowserRouter } from "react-router-dom";
import AppRouter from "./pages/AppRouter.tsx";
import { Loader } from "./shared/components";
import "./styles/index.scss";

export const App = () => {
	return (
		<BrowserRouter>
			<Suspense fallback={<Loader />}>
				<AppRouter />
			</Suspense>
		</BrowserRouter>
	);
};