import Link from "next/link"
import { Briefcase } from "lucide-react"

export function Footer() {
  return (
    <footer className="border-t bg-gray-50">
      <div className="container py-12 md:py-16">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
          <div className="col-span-2 md:col-span-1">
            <Link href="/" className="flex items-center gap-2 font-bold text-lg mb-4">
              <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-gradient-to-br from-blue-600 to-violet-600">
                <Briefcase className="h-5 w-5 text-white" />
              </div>
              <span className="text-gradient">TalentHub</span>
            </Link>
            <p className="text-sm text-muted-foreground">
              Find your dream job or hire the best talent.
            </p>
          </div>

          <div>
            <h4 className="font-semibold mb-4">For Candidates</h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li><Link href="/jobs" className="hover:text-foreground transition-colors">Browse Jobs</Link></li>
              <li><Link href="/register" className="hover:text-foreground transition-colors">Create Profile</Link></li>
              <li><Link href="/dashboard/candidate/applications" className="hover:text-foreground transition-colors">My Applications</Link></li>
            </ul>
          </div>

          <div>
            <h4 className="font-semibold mb-4">For Companies</h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li><Link href="/pricing" className="hover:text-foreground transition-colors">Pricing</Link></li>
              <li><Link href="/register" className="hover:text-foreground transition-colors">Post a Job</Link></li>
              <li><Link href="/dashboard/company" className="hover:text-foreground transition-colors">Dashboard</Link></li>
            </ul>
          </div>

          <div>
            <h4 className="font-semibold mb-4">Company</h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li><Link href="/about" className="hover:text-foreground transition-colors">About</Link></li>
              <li><Link href="/privacy" className="hover:text-foreground transition-colors">Privacy</Link></li>
              <li><Link href="/terms" className="hover:text-foreground transition-colors">Terms</Link></li>
            </ul>
          </div>
        </div>

        <div className="mt-12 pt-8 border-t text-center text-sm text-muted-foreground">
          <p>&copy; {new Date().getFullYear()} TalentHub. All rights reserved.</p>
        </div>
      </div>
    </footer>
  )
}
