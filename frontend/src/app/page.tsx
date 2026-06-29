"use client"

import Link from "next/link"
import { useState } from "react"
import { useRouter } from "next/navigation"
import { useQuery } from "@tanstack/react-query"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Card, CardContent } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Search, MapPin, Building2, ArrowRight, Zap, Shield, Loader2 } from "lucide-react"
import api, { type Job } from "@/lib/api"
import { formatSalary, timeAgo } from "@/lib/utils"

export default function Home() {
  const [heroSearch, setHeroSearch] = useState("")
  const [heroLocation, setHeroLocation] = useState("")
  const router = useRouter()

  const { data: featuredJobs, isLoading } = useQuery<Job[]>({
    queryKey: ["featured-jobs"],
    queryFn: async () => {
      const { data } = await api.get("/Jobs/featured?limit=6")
      return data
    },
  })

  const handleHeroSearch = (e: React.FormEvent) => {
    e.preventDefault()
    const params = new URLSearchParams()
    if (heroSearch) params.set("q", heroSearch)
    if (heroLocation) params.set("location", heroLocation)
    router.push(`/jobs?${params.toString()}`)
  }

  return (
    <div className="flex flex-col">
      {/* Hero Section */}
      <section className="relative overflow-hidden bg-gradient-to-br from-blue-50 via-white to-violet-50">
        <div className="absolute inset-0 bg-[url('/grid.svg')] opacity-5" />
        <div className="container relative py-24 md:py-32">
          <div className="mx-auto max-w-3xl text-center">
            <Badge className="mb-4">
              {isLoading ? "Loading..." : `${featuredJobs?.length ?? 0}+ Jobs Available`}
            </Badge>
            <h1 className="text-4xl md:text-6xl font-bold tracking-tight mb-6">
              Find Your{" "}
              <span className="text-gradient">Dream Job</span>
              <br />
              in Tech
            </h1>
            <p className="text-lg md:text-xl text-muted-foreground mb-8">
              Connect with top companies. Discover opportunities that match your skills and ambition.
            </p>

            {/* Search Box */}
            <form onSubmit={handleHeroSearch}>
              <div className="mx-auto max-w-2xl">
                <Card className="p-2 shadow-xl">
                  <div className="flex flex-col sm:flex-row gap-2">
                    <div className="flex-1 flex items-center gap-2 px-4">
                      <Search className="h-5 w-5 text-muted-foreground" />
                      <input
                        type="text"
                        placeholder="Job title, keyword, or company"
                        className="w-full py-3 bg-transparent outline-none"
                        value={heroSearch}
                        onChange={(e) => setHeroSearch(e.target.value)}
                      />
                    </div>
                    <div className="flex-1 flex items-center gap-2 px-4 border-t sm:border-t-0 sm:border-l">
                      <MapPin className="h-5 w-5 text-muted-foreground" />
                      <input
                        type="text"
                        placeholder="Location or Remote"
                        className="w-full py-3 bg-transparent outline-none"
                        value={heroLocation}
                        onChange={(e) => setHeroLocation(e.target.value)}
                      />
                    </div>
                    <Button size="lg" className="px-8" type="submit">
                      Search Jobs
                    </Button>
                  </div>
                </Card>
              </div>
            </form>

            {/* Tags */}
            <div className="mt-6 flex flex-wrap justify-center gap-2 text-sm text-muted-foreground">
              <span>Popular:</span>
              {["React Developer", ".NET Engineer", "DevOps", "Data Scientist", "Product Manager"].map((tag) => (
                <Link
                  key={tag}
                  href={`/jobs?q=${encodeURIComponent(tag)}`}
                  className="hover:text-foreground transition-colors"
                >
                  {tag}
                </Link>
              ))}
            </div>
          </div>
        </div>
      </section>

      {/* Featured Jobs */}
      <section className="py-20">
        <div className="container">
          <div className="flex items-center justify-between mb-8">
            <div>
              <h2 className="text-3xl font-bold">Featured Jobs</h2>
              <p className="text-muted-foreground mt-1">Hand-picked opportunities for you</p>
            </div>
            <Link href="/jobs">
              <Button variant="outline">
                View All <ArrowRight className="ml-2 h-4 w-4" />
              </Button>
            </Link>
          </div>

          {isLoading ? (
            <div className="flex items-center justify-center py-12">
              <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
          ) : (
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              {featuredJobs?.map((job) => (
                <Link key={job.id} href={`/jobs/${job.id}`}>
                  <Card className="h-full card-hover cursor-pointer group">
                    <CardContent className="p-6">
                      <div className="flex items-start justify-between mb-4">
                        {job.companyLogoUrl ? (
                          <img src={job.companyLogoUrl} alt={job.companyName} className="h-12 w-12 rounded-xl object-cover" />
                        ) : (
                          <div className="h-12 w-12 rounded-xl bg-gradient-to-br from-blue-500 to-violet-500 flex items-center justify-center text-white font-bold text-lg">
                            {job.companyName[0]}
                          </div>
                        )}
                        {job.isFeatured && <Badge>Featured</Badge>}
                      </div>
                      <h3 className="font-semibold text-lg mb-1 group-hover:text-primary transition-colors">
                        {job.title}
                      </h3>
                      <p className="text-sm text-muted-foreground mb-3">{job.companyName}</p>
                      <div className="flex items-center gap-4 text-sm text-muted-foreground">
                        <span className="flex items-center gap-1">
                          <MapPin className="h-4 w-4" /> {job.location || "Remote"}
                        </span>
                      </div>
                      <div className="mt-4 pt-4 border-t flex items-center justify-between">
                        <span className="font-semibold text-primary">
                          {formatSalary(job.salaryMin, job.salaryMax, job.currency)}
                        </span>
                        <span className="text-xs text-muted-foreground">{timeAgo(job.createdAt)}</span>
                      </div>
                    </CardContent>
                  </Card>
                </Link>
              ))}
            </div>
          )}
        </div>
      </section>

      {/* Features */}
      <section className="py-20 bg-gray-50">
        <div className="container">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-bold mb-4">Why TalentHub?</h2>
            <p className="text-muted-foreground max-w-2xl mx-auto">
              We make job searching and hiring simple, transparent, and effective.
            </p>
          </div>

          <div className="grid md:grid-cols-3 gap-8">
            {[
              {
                icon: <Search className="h-6 w-6" />,
                title: "Smart Search",
                description: "AI-powered job matching based on your skills, experience, and preferences.",
                color: "bg-blue-100 text-blue-600",
              },
              {
                icon: <Shield className="h-6 w-6" />,
                title: "Verified Companies",
                description: "All companies are verified. No scams, no fake listings.",
                color: "bg-green-100 text-green-600",
              },
              {
                icon: <Zap className="h-6 w-6" />,
                title: "Quick Apply",
                description: "Apply to jobs in seconds with your TalentHub profile.",
                color: "bg-violet-100 text-violet-600",
              },
            ].map((feature, i) => (
              <Card key={i} className="text-center p-8 card-hover">
                <div className={`inline-flex h-12 w-12 items-center justify-center rounded-xl ${feature.color} mb-4`}>
                  {feature.icon}
                </div>
                <h3 className="text-xl font-semibold mb-2">{feature.title}</h3>
                <p className="text-muted-foreground">{feature.description}</p>
              </Card>
            ))}
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="py-20 bg-gradient-to-br from-blue-600 to-violet-600 text-white">
        <div className="container text-center">
          <h2 className="text-3xl md:text-4xl font-bold mb-4">Ready to Start?</h2>
          <p className="text-lg text-white/80 mb-8 max-w-xl mx-auto">
            Join thousands of professionals who found their dream job on TalentHub.
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Link href="/register">
              <Button size="xl" className="bg-white text-blue-600 hover:bg-white/90">
                Create Free Account
              </Button>
            </Link>
            <Link href="/jobs">
              <Button size="xl" variant="outline" className="border-white text-white hover:bg-white/10">
                Browse Jobs
              </Button>
            </Link>
          </div>
        </div>
      </section>
    </div>
  )
}
