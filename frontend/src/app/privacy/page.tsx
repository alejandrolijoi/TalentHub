import type { Metadata } from "next"
import { Shield } from "lucide-react"

export const metadata: Metadata = {
  title: "Privacy Policy - TalentHub",
  description: "TalentHub Privacy Policy - how we collect, use, and protect your data.",
}

export default function PrivacyPage() {
  return (
    <div className="container py-12 max-w-4xl">
      <div className="flex items-center gap-3 mb-8">
        <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-gradient-to-br from-blue-600 to-violet-600">
          <Shield className="h-6 w-6 text-white" />
        </div>
        <h1 className="text-3xl font-bold">Privacy Policy</h1>
      </div>

      <div className="space-y-6 text-muted-foreground leading-relaxed">
        <p className="text-sm text-muted-foreground">Last updated: June 2026</p>

        <h2 className="text-xl font-semibold text-foreground mt-8">1. Information We Collect</h2>
        <p>
          When you create an account, we collect your name, email address, and profile information
          (such as your resume, skills, work experience, and education). If you are a company, we
          collect company details including name, description, and website.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">2. How We Use Your Information</h2>
        <p>
          We use your information to provide and improve our services, including matching candidates
          with job opportunities, processing job applications, and communicating with you about your
          account and new features.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">3. Data Sharing</h2>
        <p>
          We do not sell your personal data. Your profile information is shared with companies when
          you apply for a job. Companies&apos; public information is visible to all users browsing jobs.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">4. Data Security</h2>
        <p>
          We implement industry-standard security measures including encryption in transit and at rest,
          secure authentication, and regular security audits to protect your data.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">5. Your Rights</h2>
        <p>
          You can access, update, or delete your account information at any time through your profile
          settings. If you need assistance, contact our support team.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">6. Contact</h2>
        <p>
          If you have questions about this privacy policy, please contact us at
          privacy@talenthub.dev.
        </p>
      </div>
    </div>
  )
}
