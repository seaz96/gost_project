export type UserLoginDTO = {
  login: string,
  password: string
}

export type UserRegisterDTO = {
  login: string,
  name: string,
  password: string,
  orgName: string,
  orgBranch: string,
  orgActivity: string
}