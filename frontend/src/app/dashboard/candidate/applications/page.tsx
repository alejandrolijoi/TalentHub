"use client"

import Link from "next/link"
import { useQuery } from "@tanstack/react-query"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Briefcase, Clock, ArrowLeft, Loader2 } from "lucide-react"
import api, { type JobApplication, type PaginatedResult } from "@/lib/api"
import { useAuth } from "@/providers/auth-provider"
import { timeAgo } from "@/lib/utils"
import { useState } from "react"

export default function CandidateApplicationsPage() {
  const { user, isCandidate } = useAuth()
  const [page, setPage] = useState(1)

  const { data, isLoading } = useQuery<PaginatedResult<JobApplication>>({
    queryKey: ["my-applications-full", page],
    queryFn: async () => {
      const { data } = await api.get(`/Applications/my?page=${page}&pageSize=10`)
      return data
    },
  })

  if (!user) return null

  if (!isCandidate) {
    return (
      <div className="container py-12 text-center">
        <p className="text-muted-foreground mb-4">Only candidates can view applications.</p>
        <Link href="/dashboard/company">
          <Button>Go to Company Dashboard</Button>
        </Link>
      </div>
    )
  }

  return (
    <div className="container py-8">
      <div className="flex items-center gap-4 mb-8">
        <Link href="/dashboard/candidate">
          <Button variant="ghost" size="icon">
            <ArrowLeft className="h-5 w-5" />
          </Button>
        </Link>
        <div>
          <h1 className="text-2xl font-bold">My Applications</h1>
          <p className="text-muted-foreground">
            {isLoading ? "Loading..." : `${data?.totalCount ?? 0} applications`}
          </p>
        </div>
      </div>

      {isLoading ? (
        <div className="flex items-center justify-center py-20">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      ) : data?.items.length === 0 ? (
        <Card>
          <CardContent className="text-center py-12">
            <Briefcase className="h-12 w-12 mx-auto text-muted-foreground mb-4" />
            <p className="text-lg font-medium mb-2">No applications yet</p>
            <p className="text-muted-foreground mb-6">Start applying to jobs that match your skills.</p>
            <Link href="/jobs">
              <Button>Browse Jobs</Button>
            </Link>
          </CardContent>
        </Card>
      ) : (
        <>
          <div className="space-y-4">
            {data?.items.map((app) => (
              <Link key={app.id} href={`/jobs/${app.jobId}`} className="block">
                <Card className="card-hover cursor-pointer group">
                  <CardContent className="p-6">
                    <div className="flex items-start justify-between">
                      <div>
                        <h3 className="font-semibold group-hover:text-primary transition-colors">
                          {app.jobTitle}
                        </h3>
                        <p className="text-sm text-muted-foreground">{app.companyName}</p>
                      </div>
                      <div className="text-right">
                        <Badge variant={app.status === "Interview" ? "default" : "secondary"}>
                          {app.status}
                        </Badge>
                        <p className="text-xs text-muted-foreground mt-1 flex items-center gap-1">
                          <Clock className="h-3 w-3" /> {timeAgo(app.appliedAt)}
                        </p>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              </Link>
            ))}
          </div>

          {data && data.totalPages > 1 && (
            <div className="flex items-center justify-center gap-2 mt-8">
              <Button variant="outline" size="sm" disabled={!data.hasPreviousPage} onClick={() => setPage((p) => p - 1)}>
                Previous
              </Button>
              <span className="text-sm text-muted-foreground">
                Page {data.page} of {data.totalPages}
              </span>
              <Button variant="outline" size="sm" disabled={!data.hasNextPage} onClick={() => setPage((p) => p + 1)}>
                Next
              </Button>
            </div>
          )}
        </>
      )}
    </div>
  )
}
