"use client"

import Link from "next/link"
import { useState } from "react"
import { Card, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Search, MapPin, Filter, Briefcase, Clock, Bookmark } from "lucide-react"

const jobs = [
  { id: "1", title: "Senior React Developer", company: "TechCorp", location: "Remote", salary: "$80k-$100k", type: "Full-time", remote: "Remote", posted: "2 days ago", logo: "T", color: "from-blue-500 to-cyan-500", featured: true },
  { id: "2", title: ".NET Backend Engineer", company: "StartupXYZ", location: "New York", salary: "$70k-$90k", type: "Full-time", remote: "Hybrid", posted: "3 days ago", logo: "S", color: "from-violet-500 to-purple-500", featured: false },
  { id: "3", title: "DevOps Lead", company: "CloudInc", location: "Remote", salary: "$90k-$110k", type: "Full-time", remote: "Remote", posted: "1 day ago", logo: "C", color: "from-green-500 to-emerald-500", featured: true },
  { id: "4", title: "Full Stack Developer", company: "DigitalCo", location: "Austin, TX", salary: "$75k-$95k", type: "Full-time", remote: "Onsite", posted: "5 days ago", logo: "D", color: "from-orange-500 to-amber-500", featured: false },
  { id: "5", title: "Data Scientist", company: "AI Labs", location: "Remote", salary: "$95k-$120k", type: "Full-time", remote: "Remote", posted: "4 days ago", logo: "A", color: "from-pink-500 to-rose-500", featured: false },
  { id: "6", title: "Product Manager", company: "SaaS Inc", location: "San Francisco", salary: "$85k-$105k", type: "Full-time", remote: "Hybrid", posted: "1 week ago", logo: "S", color: "from-indigo-500 to-blue-500", featured: false },
  { id: "7", title: "Junior Frontend Developer", company: "WebAgency", location: "Remote", salary: "$45k-$60k", type: "Full-time", remote: "Remote", posted: "6 hours ago", logo: "W", color: "from-teal-500 to-cyan-500", featured: false },
  { id: "8", title: "Senior Python Developer", company: "DataFlow", location: "Boston", salary: "$85k-$105k", type: "Full-time", remote: "Hybrid", posted: "2 days ago", logo: "D", color: "from-yellow-500 to-orange-500", featured: false },
]

export default function JobsPage() {
  const [searchQuery, setSearchQuery] = useState("")
  const [locationQuery, setLocationQuery] = useState("")

  return (
    <div className="container py-8">
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold mb-2">Find Jobs</h1>
        <p className="text-muted-foreground">{jobs.length} jobs found</p>
      </div>

      {/* Search */}
      <Card className="mb-6 p-4">
        <div className="flex flex-col md:flex-row gap-4">
          <div className="flex-1 flex items-center gap-2">
            <Search className="h-5 w-5 text-muted-foreground" />
            <Input
              placeholder="Search jobs..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="border-0 focus-visible:ring-0"
            />
          </div>
          <div className="flex-1 flex items-center gap-2">
            <MapPin className="h-5 w-5 text-muted-foreground" />
            <Input
              placeholder="Location..."
              value={locationQuery}
              onChange={(e) => setLocationQuery(e.target.value)}
              className="border-0 focus-visible:ring-0"
            />
          </div>
          <Button>
            <Search className="mr-2 h-4 w-4" /> Search
          </Button>
        </div>
      </Card>

      {/* Filters */}
      <div className="flex flex-wrap gap-2 mb-6">
        {["All", "Remote", "Hybrid", "Onsite", "Full-time", "Part-time", "Contract"].map((filter) => (
          <Button key={filter} variant={filter === "All" ? "default" : "outline"} size="sm">
            {filter}
          </Button>
        ))}
      </div>

      {/* Job List */}
      <div className="space-y-4">
        {jobs.map((job) => (
          <Link key={job.id} href={`/jobs/${job.id}`}>
            <Card className="card-hover cursor-pointer group">
              <CardContent className="p-6">
                <div className="flex items-start gap-4">
                  <div className={`h-12 w-12 rounded-xl bg-gradient-to-br ${job.color} flex items-center justify-center text-white font-bold text-lg flex-shrink-0`}>
                    {job.logo}
                  </div>
                  <div className="flex-1 min-w-0">
                    <div className="flex items-start justify-between gap-4">
                      <div>
                        <h3 className="font-semibold text-lg group-hover:text-primary transition-colors">
                          {job.title}
                        </h3>
                        <p className="text-muted-foreground">{job.company}</p>
                      </div>
                      <Button variant="ghost" size="icon" className="flex-shrink-0">
                        <Bookmark className="h-5 w-5" />
                      </Button>
                    </div>
                    <div className="flex flex-wrap items-center gap-4 mt-3 text-sm text-muted-foreground">
                      <span className="flex items-center gap-1">
                        <MapPin className="h-4 w-4" /> {job.location}
                      </span>
                      <span className="flex items-center gap-1">
                        <Briefcase className="h-4 w-4" /> {job.type}
                      </span>
                      <span className="flex items-center gap-1">
                        <Clock className="h-4 w-4" /> {job.posted}
                      </span>
                    </div>
                    <div className="flex items-center gap-2 mt-3">
                      {job.featured && <Badge>Featured</Badge>}
                      <Badge variant="outline">{job.remote}</Badge>
                      <Badge variant="secondary">{job.salary}</Badge>
                    </div>
                  </div>
                </div>
              </CardContent>
            </Card>
          </Link>
        ))}
      </div>
    </div>
  )
}
