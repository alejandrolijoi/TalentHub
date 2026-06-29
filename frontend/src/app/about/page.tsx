import type { Metadata } from "next"
import { Briefcase } from "lucide-react"

export const metadata: Metadata = {
  title: "About - TalentHub",
  description: "Learn about TalentHub - the platform connecting tech talent with great companies.",
}

export default function AboutPage() {
  return (
    <div className="container py-12 max-w-4xl">
      <div className="flex items-center gap-3 mb-8">
        <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-gradient-to-br from-blue-600 to-violet-600">
          <Briefcase className="h-6 w-6 text-white" />
        </div>
        <h1 className="text-3xl font-bold">About TalentHub</h1>
      </div>

      <div className="space-y-6 text-muted-foreground leading-relaxed">
        <p>
          TalentHub is a modern job marketplace built for the tech industry. We connect talented
          professionals with innovative companies looking to build world-class teams.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">Our Mission</h2>
        <p>
          We believe that finding the right job — or the right hire — should be simple, transparent,
          and effective. Our platform leverages smart search and AI-powered matching to surface
          the most relevant opportunities, so you spend less time searching and more time doing
          meaningful work.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">For Candidates</h2>
        <p>
          Whether you&apos;re a seasoned engineer, a creative designer, or a data wizard, TalentHub
          helps you discover roles that match your skills and aspirations. Create a profile, apply
          in seconds, and track your applications — all in one place.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">For Companies</h2>
        <p>
          Post jobs, review applicants, and build your team with confidence. Our platform provides
          the tools you need to manage your hiring pipeline efficiently, with transparent pricing
          and no hidden fees.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">Our Values</h2>
        <ul className="list-disc pl-6 space-y-2">
          <li><strong>Transparency</strong> — Clear pricing, real reviews, honest job listings.</li>
          <li><strong>Quality</strong> — We verify companies and curate opportunities to ensure a high-quality experience.</li>
          <li><strong>Inclusivity</strong> — We champion remote and hybrid work to open doors for talent everywhere.</li>
          <li><strong>Innovation</strong> — We continuously improve our platform with smart features that make hiring easier.</li>
        </ul>
      </div>
    </div>
  )
}
