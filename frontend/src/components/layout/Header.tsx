"use client"

import Link from "next/link"
import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Menu, X, Briefcase, Search } from "lucide-react"

export function Header() {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)

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
          <Link href="/login">
            <Button variant="ghost">Log in</Button>
          </Link>
          <Link href="/register">
            <Button>Get Started</Button>
          </Link>
        </div>

        <button
          className="md:hidden p-2"
          onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
        >
          {mobileMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
        </button>
      </div>

      {mobileMenuOpen && (
        <div className="md:hidden border-t bg-white p-4 space-y-4">
          <Link href="/jobs" className="block py-2 text-sm font-medium">Find Jobs</Link>
          <Link href="/companies" className="block py-2 text-sm font-medium">Companies</Link>
          <Link href="/pricing" className="block py-2 text-sm font-medium">Pricing</Link>
          <div className="flex flex-col gap-2 pt-4 border-t">
            <Link href="/login"><Button variant="ghost" className="w-full">Log in</Button></Link>
            <Link href="/register"><Button className="w-full">Get Started</Button></Link>
          </div>
        </div>
      )}
    </header>
  )
}
