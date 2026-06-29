"use client"

import Link from "next/link"
import { useState } from "react"
import { useQuery } from "@tanstack/react-query"
import { Card, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Search, MapPin, Briefcase, Clock, Bookmark, Loader2 } from "lucide-react"
import api, { type Job, type PaginatedResult } from "@/lib/api"
import { formatSalary, timeAgo } from "@/lib/utils"

const remoteFilters = [
  { label: "All", value: "" },
  { label: "Remote", value: "Remote" },
  { label: "Hybrid", value: "Hybrid" },
  { label: "Onsite", value: "Onsite" },
]

const typeFilters = [
  { label: "Full-time", value: "FullTime" },
  { label: "Part-time", value: "PartTime" },
  { label: "Contract", value: "Contract" },
]

export default function JobsPage() {
  const [searchQuery, setSearchQuery] = useState("")
  const [locationQuery, setLocationQuery] = useState("")
  const [activeRemote, setActiveRemote] = useState("")
  const [activeType, setActiveType] = useState("")
  const [page, setPage] = useState(1)

  const { data, isLoading, isFetching } = useQuery<PaginatedResult<Job>>({
    queryKey: ["jobs", searchQuery, locationQuery, activeRemote, activeType, page],
    queryFn: async () => {
      const params = new URLSearchParams()
      if (searchQuery) params.set("query", searchQuery)
      if (locationQuery) params.set("location", locationQuery)
      if (activeRemote) params.set("remoteType", activeRemote)
      if (activeType) params.set("employmentType", activeType)
      params.set("page", String(page))
      params.set("pageSize", "10")
      const { data } = await api.get(`/Jobs?${params.toString()}`)
      return data
    },
  })

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault()
    setPage(1)
  }

  return (
    <div className="container py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold mb-2">Find Jobs</h1>
        <p className="text-muted-foreground">
          {isLoading ? "Loading..." : `${data?.totalCount ?? 0} jobs found`}
        </p>
      </div>

      {/* Search */}
      <Card className="mb-6 p-4">
        <form onSubmit={handleSearch}>
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
            <Button type="submit">
              <Search className="mr-2 h-4 w-4" /> Search
            </Button>
          </div>
        </form>
      </Card>

      {/* Filters */}
      <div className="flex flex-wrap gap-2 mb-6">
        {remoteFilters.map((filter) => (
          <Button
            key={filter.value}
            variant={activeRemote === filter.value && filter.value !== "" ? "default" : filter.value === "" && activeRemote === "" ? "default" : "outline"}
            size="sm"
            onClick={() => {
              setActiveRemote(filter.value)
              setPage(1)
            }}
          >
            {filter.label}
          </Button>
        ))}
        <div className="w-px bg-border mx-1" />
        {typeFilters.map((filter) => (
          <Button
            key={filter.value}
            variant={activeType === filter.value ? "default" : "outline"}
            size="sm"
            onClick={() => {
              setActiveType(activeType === filter.value ? "" : filter.value)
              setPage(1)
            }}
          >
            {filter.label}
          </Button>
        ))}
      </div>

      {/* Job List */}
      {isLoading ? (
        <div className="flex items-center justify-center py-20">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      ) : data?.items.length === 0 ? (
        <div className="text-center py-20">
          <p className="text-lg font-medium mb-2">No jobs found</p>
          <p className="text-muted-foreground">Try adjusting your search filters</p>
        </div>
      ) : (
        <>
          <div className="space-y-4">
            {data?.items.map((job) => (
              <Link key={job.id} href={`/jobs/${job.id}`}>
                <Card className="card-hover cursor-pointer group">
                  <CardContent className="p-6">
                    <div className="flex items-start gap-4">
                      {job.companyLogoUrl ? (
                        <img
                          src={job.companyLogoUrl}
                          alt={job.companyName}
                          className="h-12 w-12 rounded-xl object-cover flex-shrink-0"
                        />
                      ) : (
                        <div className="h-12 w-12 rounded-xl bg-gradient-to-br from-blue-500 to-violet-500 flex items-center justify-center text-white font-bold text-lg flex-shrink-0">
                          {job.companyName[0]}
                        </div>
                      )}
                      <div className="flex-1 min-w-0">
                        <div className="flex items-start justify-between gap-4">
                          <div>
                            <h3 className="font-semibold text-lg group-hover:text-primary transition-colors">
                              {job.title}
                            </h3>
                            <p className="text-muted-foreground">{job.companyName}</p>
                          </div>
                          <Button variant="ghost" size="icon" className="flex-shrink-0" onClick={(e) => e.preventDefault()}>
                            <Bookmark className="h-5 w-5" />
                          </Button>
                        </div>
                        <div className="flex flex-wrap items-center gap-4 mt-3 text-sm text-muted-foreground">
                          {job.location && (
                            <span className="flex items-center gap-1">
                              <MapPin className="h-4 w-4" /> {job.location}
                            </span>
                          )}
                          <span className="flex items-center gap-1">
                            <Briefcase className="h-4 w-4" /> {job.employmentType}
                          </span>
                          <span className="flex items-center gap-1">
                            <Clock className="h-4 w-4" /> {timeAgo(job.createdAt)}
                          </span>
                        </div>
                        <div className="flex items-center gap-2 mt-3">
                          {job.isFeatured && <Badge>Featured</Badge>}
                          <Badge variant="outline">{job.remoteType}</Badge>
                          <Badge variant="secondary">
                            {formatSalary(job.salaryMin, job.salaryMax, job.currency)}
                          </Badge>
                        </div>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              </Link>
            ))}
          </div>

          {/* Pagination */}
          {data && data.totalPages > 1 && (
            <div className="flex items-center justify-center gap-2 mt-8">
              <Button
                variant="outline"
                size="sm"
                disabled={!data.hasPreviousPage}
                onClick={() => setPage((p) => p - 1)}
              >
                Previous
              </Button>
              <span className="text-sm text-muted-foreground">
                Page {data.page} of {data.totalPages}
              </span>
              <Button
                variant="outline"
                size="sm"
                disabled={!data.hasNextPage}
                onClick={() => setPage((p) => p + 1)}
              >
                Next
              </Button>
            </div>
          )}
        </>
      )}
    </div>
  )
}
