import { useEffect } from "react";
import { Suspense } from "react";
import { useDispatch } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import { useAppSelector } from "./app/hooks.ts";
import { setLoading, setUser } from "./features/user/userSlice.ts";
import AppRouter from "./pages/AppRouter.tsx";
import { Loader } from "./shared/components";
import { axiosInstance } from "./shared/configs/axiosConfig.ts";
import "./index.scss";

export const App = () => {
    const dispatch = useDispatch();
    const loading = useAppSelector((state) => state.user.loading);

    useEffect(() => {
        if (localStorage.getItem("jwt_token")) {
            dispatch(setLoading(true));
            axiosInstance
                .get("/accounts/self-info")
                .then((response) => {
                    dispatch(setLoading(false));
                    dispatch(setUser(response.data));
                })
                .catch((error) => {
                    dispatch(setLoading(false));
                    console.log(error);
                });
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