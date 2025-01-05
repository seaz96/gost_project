import {createContext} from "react";
import type { UserContextType } from "../model/userModel";

export const UserContext = createContext<UserContextType>({
	user: null,
	setUser: () => {},
});