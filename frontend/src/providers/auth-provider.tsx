"use client"

import { createContext, useContext, useState, useEffect, useCallback, type ReactNode } from "react"
import { useRouter } from "next/navigation"
import api, { type AuthResponse, type UserRole } from "@/lib/api"

interface User {
  userId: string
  email: string
  role: UserRole
}

interface AuthContextType {
  user: User | null
  token: string | null
  isLoading: boolean
  login: (email: string, password: string) => Promise<{ error?: string }>
  register: (data: {
    email: string
    password: string
    role: UserRole
    firstName: string
    lastName: string
  }) => Promise<{ error?: string }>
  logout: () => void
  isCandidate: boolean
  isCompany: boolean
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null)
  const [token, setToken] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(true)
  const router = useRouter()

  useEffect(() => {
    const storedToken = localStorage.getItem("token")
    const storedUser = localStorage.getItem("user")
    if (storedToken && storedUser) {
      setToken(storedToken)
      setUser(JSON.parse(storedUser))
    }
    setIsLoading(false)
  }, [])

  const login = useCallback(
    async (email: string, password: string) => {
      try {
        const { data } = await api.post<AuthResponse>("/Auth/login", { email, password })
        localStorage.setItem("token", data.token)
        localStorage.setItem("refreshToken", data.refreshToken)
        const userData: User = { userId: data.userId, email: data.email, role: data.role }
        localStorage.setItem("user", JSON.stringify(userData))
        setToken(data.token)
        setUser(userData)
        router.push(data.role === "Company" ? "/dashboard/company" : "/dashboard/candidate")
        return {}
      } catch (err: unknown) {
        const message =
          (err as { response?: { data?: string } })?.response?.data || "Login failed. Please try again."
        return { error: message }
      }
    },
    [router]
  )

  const register = useCallback(
    async (data: {
      email: string
      password: string
      role: UserRole
      firstName: string
      lastName: string
    }) => {
      try {
        const { data: authData } = await api.post<AuthResponse>("/Auth/register", data)
        localStorage.setItem("token", authData.token)
        localStorage.setItem("refreshToken", authData.refreshToken)
        const userData: User = { userId: authData.userId, email: authData.email, role: authData.role }
        localStorage.setItem("user", JSON.stringify(userData))
        setToken(authData.token)
        setUser(userData)
        router.push(userData.role === "Company" ? "/dashboard/company" : "/dashboard/candidate")
        return {}
      } catch (err: unknown) {
        const message =
          (err as { response?: { data?: string } })?.response?.data || "Registration failed. Please try again."
        return { error: message }
      }
    },
    [router]
  )

  const logout = useCallback(() => {
    localStorage.removeItem("token")
    localStorage.removeItem("refreshToken")
    localStorage.removeItem("user")
    setToken(null)
    setUser(null)
    router.push("/")
  }, [router])

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        isLoading,
        login,
        register,
        logout,
        isCandidate: user?.role === "Candidate",
        isCompany: user?.role === "Company",
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider")
  }
  return context
}
