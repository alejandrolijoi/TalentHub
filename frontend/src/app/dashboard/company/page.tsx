"use client"

import Link from "next/link"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Plus, Users, Eye, FileText, TrendingUp, ArrowRight, Briefcase } from "lucide-react"

const stats = [
  { title: "Active Jobs", value: "8", icon: <Briefcase className="h-5 w-5" />, change: "2 new this month" },
  { title: "Applications", value: "156", icon: <FileText className="h-5 w-5" />, change: "+23 this week" },
  { title: "Job Views", value: "2,340", icon: <Eye className="h-5 w-5" />, change: "+12%" },
  { title: "Conversion", value: "6.7%", icon: <TrendingUp className="h-5 w-5" />, change: "+0.5%" },
]

const recentApplications = [
  { candidate: "John Doe", job: "Senior React Developer", status: "Applied", date: "2 hours ago" },
  { candidate: "Jane Smith", job: ".NET Backend Engineer", status: "Screening", date: "5 hours ago" },
  { candidate: "Mike Johnson", job: "DevOps Lead", status: "Interview", date: "1 day ago" },
]

const activeJobs = [
  { title: "Senior React Developer", applications: 45, views: 890, status: "Active" },
  { title: ".NET Backend Engineer", applications: 32, views: 650, status: "Active" },
  { title: "DevOps Lead", applications: 28, views: 520, status: "Active" },
]

export default function CompanyDashboard() {
  return (
    <div className="container py-8">
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-2xl font-bold">Welcome back, TechCorp!</h1>
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
              <div className="text-2xl font-bold">{stat.value}</div>
              <p className="text-xs text-muted-foreground mt-1">{stat.change}</p>
            </CardContent>
          </Card>
        ))}
      </div>

      <div className="grid lg:grid-cols-2 gap-6">
        {/* Active Jobs */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-lg">Active Jobs</CardTitle>
            <Link href="/dashboard/company/jobs">
              <Button variant="ghost" size="sm">
                Manage <ArrowRight className="ml-1 h-4 w-4" />
              </Button>
            </Link>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {activeJobs.map((job, i) => (
                <div key={i} className="flex items-center justify-between p-3 rounded-lg bg-muted/50">
                  <div>
                    <p className="font-medium text-sm">{job.title}</p>
                    <p className="text-xs text-muted-foreground">
                      {job.applications} applications · {job.views} views
                    </p>
                  </div>
                  <Badge variant="success">{job.status}</Badge>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Recent Applications */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-lg">Recent Applications</CardTitle>
            <Link href="/dashboard/company/applicants">
              <Button variant="ghost" size="sm">
                View All <ArrowRight className="ml-1 h-4 w-4" />
              </Button>
            </Link>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {recentApplications.map((app, i) => (
                <div key={i} className="flex items-center justify-between p-3 rounded-lg bg-muted/50">
                  <div className="flex items-center gap-3">
                    <div className="h-8 w-8 rounded-full bg-primary/10 flex items-center justify-center text-primary font-medium text-sm">
                      {app.candidate[0]}
                    </div>
                    <div>
                      <p className="font-medium text-sm">{app.candidate}</p>
                      <p className="text-xs text-muted-foreground">{app.job}</p>
                    </div>
                  </div>
                  <div className="text-right">
                    <Badge variant={app.status === "Interview" ? "default" : "secondary"}>
                      {app.status}
                    </Badge>
                    <p className="text-xs text-muted-foreground mt-1">{app.date}</p>
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
