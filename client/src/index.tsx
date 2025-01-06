import "./styles/index.css";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { Provider } from "react-redux";
import { ToastContainer } from "react-toastify";
import { App } from "./App.tsx";
import { store } from "./app/store.ts";

const rootElement = document.getElementById("root");

if (rootElement) {
	createRoot(rootElement).render(
		<StrictMode>
			<Provider store={store}>
				<App />
				<ToastContainer />
			</Provider>
		</StrictMode>,
	);
} else {
	console.error("Root element not found");
}
