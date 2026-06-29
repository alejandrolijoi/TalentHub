"use client"

import Link from "next/link"
import { useQuery } from "@tanstack/react-query"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Plus, Users, Eye, FileText, TrendingUp, ArrowRight, Briefcase, Loader2 } from "lucide-react"
import api, { type Job, type PaginatedResult } from "@/lib/api"
import { useAuth } from "@/providers/auth-provider"

export default function CompanyDashboard() {
  const { user } = useAuth()

  const { data: myJobs, isLoading: jobsLoading } = useQuery<PaginatedResult<Job>>({
    queryKey: ["my-jobs"],
    queryFn: async () => {
      const { data } = await api.get("/Jobs/my?pageSize=5")
      return data
    },
  })

  const stats = [
    { title: "Active Jobs", value: myJobs?.totalCount ?? 0, icon: <Briefcase className="h-5 w-5" /> },
  ]

  return (
    <div className="container py-8">
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-2xl font-bold">Welcome back!</h1>
          <p className="text-muted-foreground">Here&apos;s your hiring overview</p>
        </div>
        <Link href="/dashboard/company/jobs/new">
          <Button>
            <Plus className="mr-2 h-4 w-4" /> Post a Job
          </Button>
        </Link>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8">
        {stats.map((stat) => (
          <Card key={stat.title}>
            <CardContent className="p-6">
              <div className="flex items-center justify-between mb-2">
                <span className="text-muted-foreground">{stat.title}</span>
                <div className="h-8 w-8 rounded-lg bg-primary/10 flex items-center justify-center text-primary">
                  {stat.icon}
                </div>
              </div>
              <div className="text-2xl font-bold">
                {jobsLoading ? <Loader2 className="h-6 w-6 animate-spin" /> : stat.value}
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Active Jobs */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between">
          <CardTitle className="text-lg">Your Jobs</CardTitle>
        </CardHeader>
        <CardContent>
          {jobsLoading ? (
            <div className="flex items-center justify-center py-8">
              <Loader2 className="h-6 w-6 animate-spin text-primary" />
            </div>
          ) : myJobs?.items.length === 0 ? (
            <div className="text-center py-8">
              <p className="text-muted-foreground mb-4">You haven&apos;t posted any jobs yet.</p>
              <Link href="/dashboard/company/jobs/new">
                <Button><Plus className="mr-2 h-4 w-4" /> Post Your First Job</Button>
              </Link>
            </div>
          ) : (
            <div className="space-y-4">
              {myJobs?.items.map((job) => (
                <Link key={job.id} href={`/jobs/${job.id}`} className="block">
                  <div className="flex items-center justify-between p-3 rounded-lg bg-muted/50 hover:bg-muted transition-colors">
                    <div>
                      <p className="font-medium text-sm">{job.title}</p>
                      <p className="text-xs text-muted-foreground">
                        {job.applicationCount} applications · {job.viewCount} views
                      </p>
                    </div>
                    <Badge variant={job.status === "Active" ? "success" : "secondary"}>
                      {job.status}
                    </Badge>
                  </div>
                </Link>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
