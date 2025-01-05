import AppRouter from "../pages/AppRouter.tsx";
import { withProviders } from "./providers";
import "./styles/index.scss";

export const App = () => {
	return <AppRouter />;
};

export const AppWithProviders = withProviders(App);