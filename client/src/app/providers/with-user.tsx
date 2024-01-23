import axios from "axios";
import { UserContext, userModel } from "entities/user";
import { useEffect, useState } from "react";

export const withUser = (component: () => React.ReactNode) => () => {
    const [user, setUser] = useState<userModel.User | null>(null)

    useEffect(() => {
        if(localStorage.getItem('jwt_token')) {
          axios.get<userModel.User>('https://backend-seaz96.kexogg.ru/api/accounts/self-info', {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem('jwt_token')}`
            }
          })
          .then((repsonce) => {
            setUser(repsonce.data)
          })
          .catch(error => console.log(error))
        }
      }, [])

    return (
        <UserContext.Provider value={{user, setUser}}>
            {component()}
        </UserContext.Provider>  
    )
}
