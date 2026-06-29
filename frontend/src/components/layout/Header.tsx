"use client"

import Link from "next/link"
import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Menu, X, Briefcase, LogOut, LayoutDashboard } from "lucide-react"
import { useAuth } from "@/providers/auth-provider"

export function Header() {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)
  const { user, logout, isLoading, isCandidate, isCompany } = useAuth()

  const dashboardHref = isCompany ? "/dashboard/company" : "/dashboard/candidate"

  return (
    <header className="sticky top-0 z-50 w-full border-b bg-white/80 backdrop-blur-lg">
      <div className="container flex h-16 items-center justify-between">
        <Link href="/" className="flex items-center gap-2 font-bold text-xl">
          <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-gradient-to-br from-blue-600 to-violet-600">
            <Briefcase className="h-5 w-5 text-white" />
          </div>
          <span className="text-gradient">TalentHub</span>
        </Link>

        <nav className="hidden md:flex items-center gap-8">
          <Link href="/jobs" className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors">
            Find Jobs
          </Link>
          <Link href="/companies" className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors">
            Companies
          </Link>
          <Link href="/pricing" className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors">
            Pricing
          </Link>
        </nav>

        <div className="hidden md:flex items-center gap-3">
          {isLoading ? null : user ? (
            <>
              <Link href={dashboardHref}>
                <Button variant="ghost">
                  <LayoutDashboard className="mr-2 h-4 w-4" /> Dashboard
                </Button>
              </Link>
              <Button variant="ghost" onClick={logout}>
                <LogOut className="mr-2 h-4 w-4" /> Logout
              </Button>
            </>
          ) : (
            <>
              <Link href="/login">
                <Button variant="ghost">Log in</Button>
              </Link>
              <Link href="/register">
                <Button>Get Started</Button>
              </Link>
            </>
          )}
        </div>

        <button className="md:hidden p-2" onClick={() => setMobileMenuOpen(!mobileMenuOpen)}>
          {mobileMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
        </button>
      </div>

      {mobileMenuOpen && (
        <div className="md:hidden border-t bg-white p-4 space-y-4">
          <Link href="/jobs" className="block py-2 text-sm font-medium" onClick={() => setMobileMenuOpen(false)}>
            Find Jobs
          </Link>
          <Link href="/companies" className="block py-2 text-sm font-medium" onClick={() => setMobileMenuOpen(false)}>
            Companies
          </Link>
          <Link href="/pricing" className="block py-2 text-sm font-medium" onClick={() => setMobileMenuOpen(false)}>
            Pricing
          </Link>
          <div className="flex flex-col gap-2 pt-4 border-t">
            {isLoading ? null : user ? (
              <>
                <Link href={dashboardHref} onClick={() => setMobileMenuOpen(false)}>
                  <Button variant="ghost" className="w-full">
                    <LayoutDashboard className="mr-2 h-4 w-4" /> Dashboard
                  </Button>
                </Link>
                <Button
                  variant="ghost"
                  className="w-full"
                  onClick={() => {
                    setMobileMenuOpen(false)
                    logout()
                  }}
                >
                  <LogOut className="mr-2 h-4 w-4" /> Logout
                </Button>
              </>
            ) : (
              <>
                <Link href="/login" onClick={() => setMobileMenuOpen(false)}>
                  <Button variant="ghost" className="w-full">Log in</Button>
                </Link>
                <Link href="/register" onClick={() => setMobileMenuOpen(false)}>
                  <Button className="w-full">Get Started</Button>
                </Link>
              </>
            )}
          </div>
        </div>
      )}
    </header>
  )
}
