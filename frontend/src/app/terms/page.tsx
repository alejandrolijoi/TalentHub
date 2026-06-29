import type { Metadata } from "next"
import { FileText } from "lucide-react"

export const metadata: Metadata = {
  title: "Terms of Service - TalentHub",
  description: "TalentHub Terms of Service - rules and guidelines for using our platform.",
}

export default function TermsPage() {
  return (
    <div className="container py-12 max-w-4xl">
      <div className="flex items-center gap-3 mb-8">
        <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-gradient-to-br from-blue-600 to-violet-600">
          <FileText className="h-6 w-6 text-white" />
        </div>
        <h1 className="text-3xl font-bold">Terms of Service</h1>
      </div>

      <div className="space-y-6 text-muted-foreground leading-relaxed">
        <p className="text-sm text-muted-foreground">Last updated: June 2026</p>

        <h2 className="text-xl font-semibold text-foreground mt-8">1. Acceptance of Terms</h2>
        <p>
          By accessing or using TalentHub, you agree to be bound by these Terms of Service. If you
          do not agree, please do not use our platform.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">2. User Accounts</h2>
        <p>
          You are responsible for maintaining the confidentiality of your account credentials and
          for all activities under your account. You must provide accurate information when creating
          an account.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">3. Acceptable Use</h2>
        <p>
          You agree not to misuse the platform by posting false information, attempting to
          manipulate job listings, or engaging in any fraudulent activity. Companies must provide
          accurate job descriptions and fair hiring practices.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">4. Job Listings</h2>
        <p>
          Companies are responsible for the accuracy of their job listings. TalentHub reserves the
          right to remove listings that violate our policies or applicable laws.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">5. Limitation of Liability</h2>
        <p>
          TalentHub acts as a marketplace connecting candidates and companies. We are not
          responsible for the hiring decisions, employment terms, or conduct of either party.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">6. Changes to Terms</h2>
        <p>
          We may update these terms from time to time. We will notify users of material changes via
          email or through the platform. Continued use after changes constitutes acceptance.
        </p>

        <h2 className="text-xl font-semibold text-foreground mt-8">7. Contact</h2>
        <p>
          For questions about these terms, contact us at legal@talenthub.dev.
        </p>
      </div>
    </div>
  )
}
