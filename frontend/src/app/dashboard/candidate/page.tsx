"use client"

import Link from "next/link"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Briefcase, FileText, Bookmark, Clock, TrendingUp, ArrowRight } from "lucide-react"

const stats = [
  { title: "Applications", value: "12", icon: <FileText className="h-5 w-5" />, change: "+3 this week" },
  { title: "Interviews", value: "3", icon: <Clock className="h-5 w-5" />, change: "1 upcoming" },
  { title: "Saved Jobs", value: "8", icon: <Bookmark className="h-5 w-5" />, change: "2 new" },
  { title: "Profile Views", value: "47", icon: <TrendingUp className="h-5 w-5" />, change: "+12%" },
]

const recentApplications = [
  { job: "Senior React Developer", company: "TechCorp", status: "Screening", date: "2 days ago" },
  { job: ".NET Backend Engineer", company: "StartupXYZ", status: "Applied", date: "3 days ago" },
  { job: "DevOps Lead", company: "CloudInc", status: "Interview", date: "1 week ago" },
]

const recommendedJobs = [
  { title: "Full Stack Developer", company: "DigitalCo", location: "Remote" },
  { title: "Data Scientist", company: "AI Labs", location: "Remote" },
  { title: "Product Manager", company: "SaaS Inc", location: "SF Hybrid" },
]

export default function CandidateDashboard() {
  return (
    <div className="container py-8">
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-2xl font-bold">Welcome back, John!</h1>
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
        {/* Recent Applications */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-lg">Recent Applications</CardTitle>
            <Link href="/dashboard/candidate/applications">
              <Button variant="ghost" size="sm">
                View All <ArrowRight className="ml-1 h-4 w-4" />
              </Button>
            </Link>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {recentApplications.map((app, i) => (
                <div key={i} className="flex items-center justify-between p-3 rounded-lg bg-muted/50">
                  <div>
                    <p className="font-medium text-sm">{app.job}</p>
                    <p className="text-xs text-muted-foreground">{app.company}</p>
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
            <div className="space-y-4">
              {recommendedJobs.map((job, i) => (
                <Link key={i} href="/jobs/1" className="block p-3 rounded-lg hover:bg-muted/50 transition-colors">
                  <p className="font-medium text-sm">{job.title}</p>
                  <p className="text-xs text-muted-foreground">{job.company} · {job.location}</p>
                </Link>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
