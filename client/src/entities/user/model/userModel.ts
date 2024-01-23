export type User = {
    id: number,
    login: string,
    name: string,
    orgName: string,
    orgBranch: string,
    orgActivity: string,
    role: "Admin" | "Heisenberg" | "User",
    token: string
}

export type UserContextType = {
    user: User | null,
    setUser: (user: User | null) => void
  }
  
