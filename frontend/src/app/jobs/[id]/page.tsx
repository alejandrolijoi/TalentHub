"use client"

import Link from "next/link"
import { useParams } from "next/navigation"
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { Card, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { ArrowLeft, MapPin, Briefcase, Clock, Building2, ExternalLink, Share2, Bookmark, CheckCircle2, Loader2 } from "lucide-react"
import api, { type Job, type CompanyProfile } from "@/lib/api"
import { useAuth } from "@/providers/auth-provider"
import { formatSalary, timeAgo } from "@/lib/utils"
import { useState } from "react"

export default function JobDetailPage() {
  const params = useParams()
  const id = params.id as string
  const { user, isCandidate } = useAuth()
  const queryClient = useQueryClient()
  const [applied, setApplied] = useState(false)

  const { data: job, isLoading, error } = useQuery<Job>({
    queryKey: ["job", id],
    queryFn: async () => {
      const { data } = await api.get(`/Jobs/${id}`)
      return data
    },
    enabled: !!id,
  })

  const { data: company } = useQuery<CompanyProfile>({
    queryKey: ["company", job?.companyId],
    queryFn: async () => {
      const { data } = await api.get(`/Companies/${job!.companyId}`)
      return data
    },
    enabled: !!job?.companyId,
  })

  const applyMutation = useMutation({
    mutationFn: async () => {
      await api.post("/Applications", { jobId: id, coverLetter: "" })
    },
    onSuccess: () => {
      setApplied(true)
      queryClient.invalidateQueries({ queryKey: ["job", id] })
    },
  })

  if (isLoading) {
    return (
      <div className="container py-8 flex items-center justify-center min-h-[50vh]">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    )
  }

  if (error || !job) {
    return (
      <div className="container py-8">
        <Link href="/jobs" className="inline-flex items-center text-sm text-muted-foreground hover:text-foreground mb-6">
          <ArrowLeft className="mr-2 h-4 w-4" /> Back to Jobs
        </Link>
        <div className="text-center py-20">
          <p className="text-lg font-medium mb-2">Job not found</p>
          <p className="text-muted-foreground">This job listing may have been removed.</p>
        </div>
      </div>
    )
  }

  const requirements = job.requirements?.split("\n").filter(Boolean) || []
  const benefits = job.benefits?.split("\n").filter(Boolean) || []

  return (
    <div className="container py-8">
      <Link href="/jobs" className="inline-flex items-center text-sm text-muted-foreground hover:text-foreground mb-6">
        <ArrowLeft className="mr-2 h-4 w-4" /> Back to Jobs
      </Link>

      <div className="grid lg:grid-cols-3 gap-8">
        {/* Main Content */}
        <div className="lg:col-span-2 space-y-6">
          <Card>
            <CardContent className="p-8">
              <div className="flex items-start gap-4 mb-6">
                {job.companyLogoUrl ? (
                  <img src={job.companyLogoUrl} alt={job.companyName} className="h-16 w-16 rounded-xl object-cover" />
                ) : (
                  <div className="h-16 w-16 rounded-xl bg-gradient-to-br from-blue-500 to-violet-500 flex items-center justify-center text-white font-bold text-2xl">
                    {job.companyName[0]}
                  </div>
                )}
                <div>
                  <h1 className="text-2xl md:text-3xl font-bold mb-1">{job.title}</h1>
                  <p className="text-lg text-muted-foreground">{job.companyName}</p>
                </div>
              </div>

              <div className="flex flex-wrap gap-3 mb-6">
                {job.location && (
                  <Badge variant="outline" className="flex items-center gap-1">
                    <MapPin className="h-3 w-3" /> {job.location}
                  </Badge>
                )}
                <Badge variant="outline" className="flex items-center gap-1">
                  <Briefcase className="h-3 w-3" /> {job.employmentType}
                </Badge>
                <Badge variant="outline" className="flex items-center gap-1">
                  <Clock className="h-3 w-3" /> {timeAgo(job.createdAt)}
                </Badge>
                <Badge className="flex items-center gap-1">
                  {formatSalary(job.salaryMin, job.salaryMax, job.currency)}
                </Badge>
              </div>

              <div className="space-y-6">
                <div>
                  <h2 className="text-xl font-semibold mb-3">About the Role</h2>
                  <div className="text-muted-foreground whitespace-pre-line">{job.description}</div>
                </div>

                {requirements.length > 0 && (
                  <div>
                    <h2 className="text-xl font-semibold mb-3">Requirements</h2>
                    <ul className="space-y-2">
                      {requirements.map((req, i) => (
                        <li key={i} className="flex items-start gap-2 text-muted-foreground">
                          <CheckCircle2 className="h-5 w-5 text-green-500 mt-0.5 flex-shrink-0" />
                          {req}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}

                {benefits.length > 0 && (
                  <div>
                    <h2 className="text-xl font-semibold mb-3">Benefits</h2>
                    <ul className="space-y-2">
                      {benefits.map((benefit, i) => (
                        <li key={i} className="flex items-start gap-2 text-muted-foreground">
                          <CheckCircle2 className="h-5 w-5 text-blue-500 mt-0.5 flex-shrink-0" />
                          {benefit}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}

                {job.skills.length > 0 && (
                  <div>
                    <h2 className="text-xl font-semibold mb-3">Skills</h2>
                    <div className="flex flex-wrap gap-2">
                      {job.skills.map((skill) => (
                        <Badge key={skill} variant="secondary">{skill}</Badge>
                      ))}
                    </div>
                  </div>
                )}
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          <Card className="sticky top-24">
            <CardContent className="p-6">
              <div className="space-y-4">
                {isCandidate && (
                  <Button
                    className="w-full"
                    size="lg"
                    onClick={() => applyMutation.mutate()}
                    disabled={applied || applyMutation.isPending}
                  >
                    {applyMutation.isPending ? (
                      <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    ) : applied ? (
                      "Applied"
                    ) : (
                      "Apply Now"
                    )}
                  </Button>
                )}
                {!isCandidate && !user && (
                  <Link href="/login">
                    <Button className="w-full" size="lg">Sign in to Apply</Button>
                  </Link>
                )}
                <Button variant="outline" className="w-full" onClick={(e) => e.preventDefault()}>
                  <Bookmark className="mr-2 h-4 w-4" /> Save Job
                </Button>
                <Button variant="ghost" className="w-full" onClick={(e) => e.preventDefault()}>
                  <Share2 className="mr-2 h-4 w-4" /> Share
                </Button>
              </div>

              {company && (
                <div className="mt-6 pt-6 border-t">
                  <h3 className="font-semibold mb-4">About Company</h3>
                  <div className="space-y-3 text-sm">
                    <div className="flex items-center gap-2 text-muted-foreground">
                      <Building2 className="h-4 w-4" /> {company.name}
                    </div>
                    {company.location && (
                      <div className="flex items-center gap-2 text-muted-foreground">
                        <MapPin className="h-4 w-4" /> {company.location}
                      </div>
                    )}
                    {company.companySize && (
                      <div className="flex items-center gap-2 text-muted-foreground">
                        <Briefcase className="h-4 w-4" /> {company.companySize} employees
                      </div>
                    )}
                    {company.website && (
                      <a href={company.website} target="_blank" rel="noopener noreferrer" className="flex items-center gap-2 text-primary hover:underline">
                        <ExternalLink className="h-4 w-4" /> Visit Website
                      </a>
                    )}
                  </div>
                </div>
              )}
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  )
}
