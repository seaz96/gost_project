import "./index.css";
import {StrictMode} from "react";
import {createRoot} from "react-dom/client";
import {Provider} from "react-redux";
import {AppWithProviders} from "./app/App";
import {store} from "./app/store/store.ts";

createRoot(document.getElementById("root")!).render(
	<StrictMode>
		<Provider store={store}>
			<AppWithProviders />
		</Provider>
	</StrictMode>,
);