import axios from "axios"

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000/api",
  headers: {
    "Content-Type": "application/json",
  },
})

api.interceptors.request.use((config) => {
  if (typeof window !== "undefined") {
    const token = localStorage.getItem("token")
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      if (typeof window !== "undefined") {
        localStorage.removeItem("token")
        localStorage.removeItem("refreshToken")
        localStorage.removeItem("user")
        window.location.href = "/login"
      }
    }
    return Promise.reject(error)
  }
)

export default api

export type UserRole = "Candidate" | "Company" | "Admin"

export interface AuthResponse {
  userId: string
  email: string
  role: UserRole
  token: string
  refreshToken: string
}

export interface Job {
  id: string
  title: string
  description: string
  requirements?: string
  benefits?: string
  companyId: string
  companyName: string
  companyLogoUrl?: string
  categoryId?: string
  categoryName?: string
  employmentType: string
  experienceLevel: string
  location?: string
  remoteType: string
  salaryMin?: number
  salaryMax?: number
  currency: string
  status: string
  isFeatured: boolean
  viewCount: number
  applicationCount: number
  skills: string[]
  createdAt: string
}

export interface Company {
  id: string
  name: string
  description?: string
  logoUrl?: string
  website?: string
  linkedInUrl?: string
  industry?: string
  companySize?: string
  location?: string
  foundedYear?: number
  createdAt: string
}

export interface CompanyProfile extends Company {
  totalJobs?: number
}

export interface Candidate {
  id: string
  firstName: string
  lastName: string
  phone?: string
  bio?: string
  title?: string
  resumeUrl?: string
  linkedInUrl?: string
  githubUrl?: string
  website?: string
  location?: string
  yearsExperience?: number
  skills: string[]
  createdAt: string
}

export interface JobApplication {
  id: string
  jobId: string
  jobTitle: string
  companyName: string
  candidateId: string
  candidateName: string
  status: string
  coverLetter?: string
  appliedAt: string
}

export interface Plan {
  id: string
  name: string
  priceMonthly: number
  priceYearly: number
  currency: string
  maxJobsPerMonth: number
  maxApplicantsPerJob?: number
  features: Record<string, unknown>
}

export interface PaginatedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export interface ApplicationStats {
  totalApplied: number
  screening: number
  interview: number
  offer: number
  hired: number
  rejected: number
}

export interface Subscription {
  id: string
  planId: string
  planName: string
  paymentProvider: string
  status: string
  currentPeriodStart: string
  currentPeriodEnd: string
  cancelAtPeriodEnd: boolean
}

export interface Invoice {
  id: string
  amount: number
  currency: string
  status: string
  invoiceUrl?: string
  createdAt: string
}

export interface JobSearchParams {
  query?: string
  categoryId?: string
  employmentType?: string
  experienceLevel?: string
  remoteType?: string
  salaryMin?: number
  salaryMax?: number
  location?: string
  page?: number
  pageSize?: number
}
