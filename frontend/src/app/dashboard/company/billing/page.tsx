"use client"

import { useState } from "react"
import { useQuery, useMutation } from "@tanstack/react-query"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Check, CreditCard, Download, ExternalLink, Loader2 } from "lucide-react"
import api, { type Subscription, type Invoice, type PaginatedResult } from "@/lib/api"

export default function BillingPage() {
  const { data: subscription, isLoading: subLoading } = useQuery<Subscription>({
    queryKey: ["current-subscription"],
    queryFn: async () => {
      const { data } = await api.get("/Subscriptions/current")
      return data
    },
  })

  const { data: invoices, isLoading: invoicesLoading } = useQuery<PaginatedResult<Invoice>>({
    queryKey: ["invoices"],
    queryFn: async () => {
      const { data } = await api.get("/Subscriptions/billing/invoices")
      return data
    },
  })

  const { data: usage } = useQuery<{ usage: { jobsPosted: number; maxJobs: number } }>({
    queryKey: ["usage"],
    queryFn: async () => {
      const { data } = await api.get("/Subscriptions/usage")
      return data
    },
  })

  const portalMutation = useMutation({
    mutationFn: async () => {
      const { data } = await api.get("/Subscriptions/billing/portal")
      return data.url as string
    },
    onSuccess: (url) => {
      window.open(url, "_blank")
    },
  })

  const cancelMutation = useMutation({
    mutationFn: async () => {
      await api.post("/Subscriptions/cancel")
    },
  })

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
              <Badge variant={subscription?.status === "Active" ? "success" : "secondary"}>
                {subscription?.status ?? "None"}
              </Badge>
            </div>
          </CardHeader>
          <CardContent>
            {subLoading ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="h-6 w-6 animate-spin text-primary" />
              </div>
            ) : !subscription ? (
              <div className="text-center py-8">
                <p className="text-muted-foreground mb-4">No active subscription.</p>
                <Button>View Plans</Button>
              </div>
            ) : (
              <>
                <div className="flex items-center justify-between p-4 bg-muted rounded-lg mb-4">
                  <div>
                    <p className="text-2xl font-bold">{subscription.planName}</p>
                    <p className="text-muted-foreground">{subscription.paymentProvider}</p>
                  </div>
                  <div className="text-right">
                    <p className="text-sm text-muted-foreground">Renews</p>
                    <p className="font-medium">
                      {new Date(subscription.currentPeriodEnd).toLocaleDateString()}
                    </p>
                  </div>
                </div>

                {usage && (
                  <div className="mb-4">
                    <div className="flex items-center justify-between text-sm mb-2">
                      <span>Job posts used this month</span>
                      <span>{usage.usage.jobsPosted} / {usage.usage.maxJobs}</span>
                    </div>
                    <div className="h-2 bg-muted rounded-full overflow-hidden">
                      <div
                        className="h-full bg-primary rounded-full"
                        style={{ width: `${Math.min((usage.usage.jobsPosted / usage.usage.maxJobs) * 100, 100)}%` }}
                      />
                    </div>
                  </div>
                )}

                <div className="flex gap-3">
                  <Button
                    onClick={() => portalMutation.mutate()}
                    disabled={portalMutation.isPending}
                  >
                    <CreditCard className="mr-2 h-4 w-4" />
                    {portalMutation.isPending ? "Loading..." : "Manage Subscription"}
                  </Button>
                </div>
              </>
            )}
          </CardContent>
        </Card>

        {/* Invoice History */}
        <Card>
          <CardHeader>
            <CardTitle>Invoice History</CardTitle>
          </CardHeader>
          <CardContent>
            {invoicesLoading ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="h-6 w-6 animate-spin text-primary" />
              </div>
            ) : invoices?.items.length === 0 ? (
              <p className="text-center text-muted-foreground py-8">No invoices yet.</p>
            ) : (
              <div className="space-y-3">
                {invoices?.items.map((invoice) => (
                  <div key={invoice.id} className="flex items-center justify-between p-3 rounded-lg bg-muted/50">
                    <div>
                      <p className="font-medium text-sm">
                        {new Date(invoice.createdAt).toLocaleDateString()}
                      </p>
                      <p className="text-xs text-muted-foreground">
                        {invoice.currency} {invoice.amount.toFixed(2)}
                      </p>
                    </div>
                    <div className="flex items-center gap-4">
                      <Badge variant={invoice.status === "Succeeded" ? "success" : "secondary"}>
                        {invoice.status}
                      </Badge>
                      {invoice.invoiceUrl && (
                        <a href={invoice.invoiceUrl} target="_blank" rel="noopener noreferrer">
                          <Button variant="ghost" size="icon">
                            <Download className="h-4 w-4" />
                          </Button>
                        </a>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
