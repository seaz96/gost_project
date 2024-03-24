import axios from "axios";
import { UserContext, userModel } from "entities/user";
import { useEffect, useState } from "react";
import { Loader } from "shared/components";
import { axiosInstance } from "shared/configs/axiosConfig";

export const withUser = (component: () => React.ReactNode) => () => {
    const [user, setUser] = useState<userModel.User | null>(null)
    const [loading, setLoading] = useState(false)

    useEffect(() => {
      if(localStorage.getItem('jwt_token')) {
        setLoading(true)
        axiosInstance.get<userModel.User>('/accounts/self-info')
        .then((repsonce) => {
          setLoading(false)
          setUser(repsonce.data)
        })
        .catch(error => {
          setLoading(false)
          console.log(error)
        })
      }
    }, [])

    if(loading) return <Loader />
    return (
        <UserContext.Provider value={{user, setUser}}>
            {component()}
        </UserContext.Provider>  
    )
}
