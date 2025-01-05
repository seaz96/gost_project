import { setLoading, setUser } from "entities/user/model/store/userSlice";
import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { Loader } from "shared/components";
import { axiosInstance } from "shared/configs/axiosConfig";
import { useAppSelector } from "../store/hooks";

export const withUser = (component: () => React.ReactNode) => () => {
  const dispatch = useDispatch();
  const loading = useAppSelector(state => state.user.loading);

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
  return component();
};