"use client"

import Link from "next/link"
import { useQuery } from "@tanstack/react-query"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Briefcase, FileText, Bookmark, Clock, ArrowRight, Loader2 } from "lucide-react"
import api, { type JobApplication, type ApplicationStats, type PaginatedResult, type Job } from "@/lib/api"
import { useAuth } from "@/providers/auth-provider"
import { timeAgo } from "@/lib/utils"

export default function CandidateDashboard() {
  const { user } = useAuth()

  const { data: applications, isLoading: appsLoading } = useQuery<PaginatedResult<JobApplication>>({
    queryKey: ["my-applications"],
    queryFn: async () => {
      const { data } = await api.get("/Applications/my?pageSize=5")
      return data
    },
  })

  const { data: stats, isLoading: statsLoading } = useQuery<ApplicationStats>({
    queryKey: ["application-stats"],
    queryFn: async () => {
      const { data } = await api.get("/Applications/my/stats")
      return data
    },
  })

  const { data: recommendedJobs, isLoading: jobsLoading } = useQuery<Job[]>({
    queryKey: ["recommended-jobs"],
    queryFn: async () => {
      const { data } = await api.get("/Jobs/featured?limit=3")
      return data
    },
  })

  const statCards = [
    { title: "Applications", value: stats?.totalApplied ?? 0, icon: <FileText className="h-5 w-5" /> },
    { title: "Screening", value: stats?.screening ?? 0, icon: <Clock className="h-5 w-5" /> },
    { title: "Interviews", value: stats?.interview ?? 0, icon: <Briefcase className="h-5 w-5" /> },
    { title: "Offers", value: stats?.offer ?? 0, icon: <Bookmark className="h-5 w-5" /> },
  ]

  return (
    <div className="container py-8">
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-2xl font-bold">Welcome back!</h1>
          <p className="text-muted-foreground">Here&apos;s your job search overview</p>
        </div>
        <Link href="/jobs">
          <Button>
            <Briefcase className="mr-2 h-4 w-4" /> Find Jobs
          </Button>
        </Link>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8">
        {statCards.map((stat) => (
          <Card key={stat.title}>
            <CardContent className="p-6">
              <div className="flex items-center justify-between mb-2">
                <span className="text-muted-foreground">{stat.title}</span>
                <div className="h-8 w-8 rounded-lg bg-primary/10 flex items-center justify-center text-primary">
                  {stat.icon}
                </div>
              </div>
              <div className="text-2xl font-bold">
                {statsLoading ? <Loader2 className="h-6 w-6 animate-spin" /> : stat.value}
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      <div className="grid lg:grid-cols-2 gap-6">
        {/* Recent Applications */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-lg">Recent Applications</CardTitle>
          </CardHeader>
          <CardContent>
            {appsLoading ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="h-6 w-6 animate-spin text-primary" />
              </div>
            ) : applications?.items.length === 0 ? (
              <div className="text-center py-8">
                <p className="text-muted-foreground mb-4">No applications yet.</p>
                <Link href="/jobs">
                  <Button>Browse Jobs</Button>
                </Link>
              </div>
            ) : (
              <div className="space-y-4">
                {applications?.items.map((app) => (
                  <Link key={app.id} href={`/jobs/${app.jobId}`} className="block">
                    <div className="flex items-center justify-between p-3 rounded-lg bg-muted/50 hover:bg-muted transition-colors">
                      <div>
                        <p className="font-medium text-sm">{app.jobTitle}</p>
                        <p className="text-xs text-muted-foreground">{app.companyName}</p>
                      </div>
                      <div className="text-right">
                        <Badge variant={app.status === "Interview" ? "default" : "secondary"}>
                          {app.status}
                        </Badge>
                        <p className="text-xs text-muted-foreground mt-1">{timeAgo(app.appliedAt)}</p>
                      </div>
                    </div>
                  </Link>
                ))}
              </div>
            )}
          </CardContent>
        </Card>

        {/* Recommended Jobs */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-lg">Recommended for You</CardTitle>
            <Link href="/jobs">
              <Button variant="ghost" size="sm">
                View All <ArrowRight className="ml-1 h-4 w-4" />
              </Button>
            </Link>
          </CardHeader>
          <CardContent>
            {jobsLoading ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="h-6 w-6 animate-spin text-primary" />
              </div>
            ) : (
              <div className="space-y-4">
                {recommendedJobs?.map((job) => (
                  <Link key={job.id} href={`/jobs/${job.id}`} className="block p-3 rounded-lg hover:bg-muted/50 transition-colors">
                    <p className="font-medium text-sm">{job.title}</p>
                    <p className="text-xs text-muted-foreground">{job.companyName} · {job.location || "Remote"}</p>
                  </Link>
                ))}
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
