"use client"

import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Check } from "lucide-react"

const plans = [
  {
    name: "Free",
    description: "Perfect for trying out TalentHub",
    priceMonthly: 0,
    priceYearly: 0,
    maxJobs: 3,
    features: [
      "3 job posts per month",
      "Basic applicant tracking",
      "Email support",
      "Company profile",
    ],
    cta: "Get Started",
    popular: false,
  },
  {
    name: "Starter",
    description: "For growing teams",
    priceMonthly: 29,
    priceYearly: 290,
    maxJobs: 25,
    features: [
      "25 job posts per month",
      "Featured job posts",
      "Basic analytics",
      "Priority email support",
      "Applicant management",
      "Custom company page",
    ],
    cta: "Start Free Trial",
    popular: true,
  },
  {
    name: "Pro",
    description: "For established companies",
    priceMonthly: 79,
    priceYearly: 790,
    maxJobs: -1,
    features: [
      "Unlimited job posts",
      "Featured job posts",
      "Advanced analytics",
      "Priority support",
      "Candidate search",
      "API access",
      "Team collaboration",
    ],
    cta: "Start Free Trial",
    popular: false,
  },
]

export default function PricingPage() {
  const [annual, setAnnual] = useState(false)

  return (
    <div className="container py-16">
      <div className="text-center mb-12">
        <h1 className="text-4xl font-bold mb-4">Simple, Transparent Pricing</h1>
        <p className="text-lg text-muted-foreground max-w-2xl mx-auto">
          Choose the plan that fits your hiring needs. All plans include a 14-day free trial.
        </p>

        {/* Toggle */}
        <div className="flex items-center justify-center gap-4 mt-8">
          <span className={`text-sm ${!annual ? "font-medium" : "text-muted-foreground"}`}>Monthly</span>
          <button
            onClick={() => setAnnual(!annual)}
            className={`relative inline-flex h-6 w-11 items-center rounded-full transition-colors ${
              annual ? "bg-primary" : "bg-muted"
            }`}
          >
            <span
              className={`inline-block h-4 w-4 transform rounded-full bg-white transition-transform ${
                annual ? "translate-x-6" : "translate-x-1"
              }`}
            />
          </button>
          <span className="text-sm">
            Annual <Badge variant="secondary" className="ml-1">Save 17%</Badge>
          </span>
        </div>
      </div>

      <div className="grid md:grid-cols-3 gap-8 max-w-5xl mx-auto">
        {plans.map((plan) => (
          <Card key={plan.name} className={`relative ${plan.popular ? "border-primary shadow-lg scale-105" : ""}`}>
            {plan.popular && (
              <div className="absolute -top-3 left-1/2 -translate-x-1/2">
                <Badge>Most Popular</Badge>
              </div>
            )}
            <CardHeader>
              <CardTitle>{plan.name}</CardTitle>
              <CardDescription>{plan.description}</CardDescription>
              <div className="mt-4">
                <span className="text-4xl font-bold">
                  ${annual ? Math.floor(plan.priceYearly / 12) : plan.priceMonthly}
                </span>
                <span className="text-muted-foreground">/month</span>
                {annual && plan.priceMonthly > 0 && (
                  <p className="text-sm text-muted-foreground mt-1">
                    Billed ${plan.priceYearly}/year
                  </p>
                )}
              </div>
            </CardHeader>
            <CardContent>
              <ul className="space-y-3 mb-6">
                {plan.features.map((feature) => (
                  <li key={feature} className="flex items-start gap-2 text-sm">
                    <Check className="h-5 w-5 text-green-500 mt-0.5 flex-shrink-0" />
                    {feature}
                  </li>
                ))}
              </ul>
              <Button
                className="w-full"
                variant={plan.popular ? "default" : "outline"}
                size="lg"
              >
                {plan.cta}
              </Button>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* FAQ */}
      <div className="mt-20 max-w-3xl mx-auto">
        <h2 className="text-2xl font-bold text-center mb-8">Frequently Asked Questions</h2>
        <div className="space-y-6">
          {[
            { q: "Can I change plans at any time?", a: "Yes, you can upgrade or downgrade your plan at any time. Changes take effect immediately." },
            { q: "What payment methods do you accept?", a: "We accept all major credit cards (Visa, Mastercard, American Express) through Stripe, and also support MercadoPago for Latin America." },
            { q: "Is there a free trial?", a: "Yes! All paid plans come with a 14-day free trial. No credit card required to start." },
            { q: "Can I cancel anytime?", a: "Absolutely. You can cancel your subscription at any time. You'll continue to have access until the end of your billing period." },
          ].map((faq, i) => (
            <div key={i} className="border-b pb-6">
              <h3 className="font-semibold mb-2">{faq.q}</h3>
              <p className="text-muted-foreground text-sm">{faq.a}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
