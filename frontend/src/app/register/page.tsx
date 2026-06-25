"use client"

import Link from "next/link"
import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Briefcase, Mail, Lock, User } from "lucide-react"

export default function RegisterPage() {
  const [role, setRole] = useState<"Candidate" | "Company">("Candidate")

  return (
    <div className="container flex items-center justify-center min-h-[calc(100vh-200px)] py-12">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <div className="mx-auto h-12 w-12 rounded-xl bg-gradient-to-br from-blue-600 to-violet-600 flex items-center justify-center mb-4">
            <Briefcase className="h-6 w-6 text-white" />
          </div>
          <CardTitle className="text-2xl">Create your account</CardTitle>
          <CardDescription>Join TalentHub today</CardDescription>
        </CardHeader>
        <CardContent>
          {/* Role Toggle */}
          <div className="grid grid-cols-2 gap-2 mb-6 p-1 bg-muted rounded-lg">
            <button
              onClick={() => setRole("Candidate")}
              className={`py-2 px-4 rounded-md text-sm font-medium transition-colors ${
                role === "Candidate" ? "bg-white shadow-sm text-foreground" : "text-muted-foreground"
              }`}
            >
              I&apos;m looking for a job
            </button>
            <button
              onClick={() => setRole("Company")}
              className={`py-2 px-4 rounded-md text-sm font-medium transition-colors ${
                role === "Company" ? "bg-white shadow-sm text-foreground" : "text-muted-foreground"
              }`}
            >
              I&apos;m hiring
            </button>
          </div>

          <form className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="firstName">First Name</Label>
                <div className="relative">
                  <User className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                  <Input id="firstName" placeholder="John" className="pl-9" />
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="lastName">Last Name</Label>
                <Input id="lastName" placeholder="Doe" />
              </div>
            </div>
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <div className="relative">
                <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                <Input id="email" type="email" placeholder="you@example.com" className="pl-9" />
              </div>
            </div>
            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <div className="relative">
                <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                <Input id="password" type="password" placeholder="••••••••" className="pl-9" />
              </div>
            </div>
            <Button className="w-full" size="lg">
              Create Account
            </Button>
          </form>

          <p className="text-center text-xs text-muted-foreground mt-4">
            By signing up, you agree to our{" "}
            <Link href="/terms" className="text-primary hover:underline">Terms</Link>
            {" "}and{" "}
            <Link href="/privacy" className="text-primary hover:underline">Privacy Policy</Link>
          </p>

          <p className="text-center text-sm text-muted-foreground mt-6">
            Already have an account?{" "}
            <Link href="/login" className="text-primary hover:underline">
              Sign in
            </Link>
          </p>
        </CardContent>
      </Card>
    </div>
  )
}
