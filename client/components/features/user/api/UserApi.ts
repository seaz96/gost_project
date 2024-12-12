import fetch from "~/components/shared/libs/fetch"
import type { UserLoginDTO, UserRegisterDTO } from "../model/types"
import type { User } from "~/components/shared/types/user"

export const UserApi = () => {
  const getMe = async () => {
    return await fetch("/api/accounts/self-info")
  }

  const login = async (data: UserLoginDTO) => {
    return await fetch("/api/accounts/login", {
      method: 'post',
      body: {
        ...data
      }
    }) as User
  }

  const register = async (data: UserRegisterDTO) => {
    return await fetch("/api/accounts/register", {
      method: 'post',
      body: {
        ...data
      }
    }) as User
  }

  return {
    getMe,
    login,
    register
  }
}