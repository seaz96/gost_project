import React from "react";
import { UserContextType } from "../model/userModel";

export const UserContext = React.createContext<UserContextType>({
    user: null,
    setUser:() => {}
})