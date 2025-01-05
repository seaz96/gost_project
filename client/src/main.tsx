import './index.css'
import {createRoot} from "react-dom/client";
import {StrictMode} from "react";
import {AppWithProviders} from "./app/App";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AppWithProviders />
  </StrictMode>
)


