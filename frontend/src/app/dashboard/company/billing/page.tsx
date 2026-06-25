"use client"

import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Check, CreditCard, Download, ExternalLink } from "lucide-react"

const currentPlan = {
  name: "Starter",
  price: 29,
  period: "month",
  nextBilling: "February 15, 2026",
  jobsUsed: 12,
  jobsLimit: 25,
}

const invoices = [
  { id: "1", date: "January 15, 2026", amount: 29, status: "Paid" },
  { id: "2", date: "December 15, 2025", amount: 29, status: "Paid" },
  { id: "3", date: "November 15, 2025", amount: 29, status: "Paid" },
]

export default function BillingPage() {
  const [loading, setLoading] = useState(false)

  const handleManageSubscription = async () => {
    setLoading(true)
    // API call to get billing portal URL
    setTimeout(() => setLoading(false), 2000)
  }

  return (
    <div className="container py-8 max-w-4xl">
      <h1 className="text-2xl font-bold mb-8">Billing & Subscription</h1>

      <div className="grid gap-6">
        {/* Current Plan */}
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Current Plan</CardTitle>
                <CardDescription>Manage your subscription</CardDescription>
              </div>
              <Badge variant="success">Active</Badge>
            </div>
          </CardHeader>
          <CardContent>
            <div className="flex items-center justify-between p-4 bg-muted rounded-lg mb-4">
              <div>
                <p className="text-2xl font-bold">{currentPlan.name}</p>
                <p className="text-muted-foreground">${currentPlan.price}/{currentPlan.period}</p>
              </div>
              <div className="text-right">
                <p className="text-sm text-muted-foreground">Next billing</p>
                <p className="font-medium">{currentPlan.nextBilling}</p>
              </div>
            </div>

            <div className="mb-4">
              <div className="flex items-center justify-between text-sm mb-2">
                <span>Job posts used</span>
                <span>{currentPlan.jobsUsed} / {currentPlan.jobsLimit}</span>
              </div>
              <div className="h-2 bg-muted rounded-full overflow-hidden">
                <div
                  className="h-full bg-primary rounded-full"
                  style={{ width: `${(currentPlan.jobsUsed / currentPlan.jobsLimit) * 100}%` }}
                />
              </div>
            </div>

            <div className="flex gap-3">
              <Button onClick={handleManageSubscription} disabled={loading}>
                <CreditCard className="mr-2 h-4 w-4" />
                {loading ? "Loading..." : "Manage Subscription"}
              </Button>
              <Button variant="outline">
                Change Plan
              </Button>
            </div>
          </CardContent>
        </Card>

        {/* Plan Features */}
        <Card>
          <CardHeader>
            <CardTitle>Plan Features</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid md:grid-cols-2 gap-4">
              {[
                "25 job posts per month",
                "Featured job posts",
                "Basic analytics",
                "Priority email support",
                "Applicant management",
                "Custom company page",
              ].map((feature) => (
                <div key={feature} className="flex items-center gap-2">
                  <Check className="h-4 w-4 text-green-500" />
                  <span className="text-sm">{feature}</span>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Invoice History */}
        <Card>
          <CardHeader>
            <CardTitle>Invoice History</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {invoices.map((invoice) => (
                <div key={invoice.id} className="flex items-center justify-between p-3 rounded-lg bg-muted/50">
                  <div>
                    <p className="font-medium text-sm">{invoice.date}</p>
                    <p className="text-xs text-muted-foreground">Invoice #{invoice.id}</p>
                  </div>
                  <div className="flex items-center gap-4">
                    <Badge variant="success">{invoice.status}</Badge>
                    <span className="font-medium">${invoice.amount}</span>
                    <Button variant="ghost" size="icon">
                      <Download className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
