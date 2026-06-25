"use client"

import Link from "next/link"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { ArrowLeft, MapPin, Briefcase, Clock, Building2, ExternalLink, Share2, Bookmark, CheckCircle2 } from "lucide-react"

const job = {
  id: "1",
  title: "Senior React Developer",
  company: "TechCorp Inc.",
  companyLogo: "T",
  location: "Remote (US/EU)",
  type: "Full-time",
  remote: "Remote",
  salary: "$80,000 - $100,000",
  posted: "January 15, 2026",
  description: `We are looking for a Senior React Developer to join our growing engineering team. You will work on building and maintaining our core web applications, collaborating with designers and backend engineers to deliver exceptional user experiences.

As a Senior React Developer, you will be responsible for architecting and implementing complex UI components, mentoring junior developers, and contributing to our technical decisions.`,
  requirements: [
    "5+ years of experience with React/TypeScript",
    "Strong understanding of modern web architecture",
    "Experience with Next.js and server-side rendering",
    "Proficiency with state management (Redux, Zustand, or similar)",
    "Experience with testing (Jest, React Testing Library)",
    "Excellent problem-solving skills",
    "Strong communication skills",
  ],
  benefits: [
    "Unlimited PTO",
    "Health, Dental & Vision Insurance",
    "401k with 4% match",
    "Remote-first culture",
    "Annual learning budget",
    "Home office stipend",
  ],
  skills: ["React", "TypeScript", "Next.js", "Redux", "GraphQL", "Jest", "CSS-in-JS"],
}

export default function JobDetailPage() {
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
                <div className="h-16 w-16 rounded-xl bg-gradient-to-br from-blue-500 to-cyan-500 flex items-center justify-center text-white font-bold text-2xl">
                  {job.companyLogo}
                </div>
                <div>
                  <h1 className="text-2xl md:text-3xl font-bold mb-1">{job.title}</h1>
                  <p className="text-lg text-muted-foreground">{job.company}</p>
                </div>
              </div>

              <div className="flex flex-wrap gap-3 mb-6">
                <Badge variant="outline" className="flex items-center gap-1">
                  <MapPin className="h-3 w-3" /> {job.location}
                </Badge>
                <Badge variant="outline" className="flex items-center gap-1">
                  <Briefcase className="h-3 w-3" /> {job.type}
                </Badge>
                <Badge variant="outline" className="flex items-center gap-1">
                  <Clock className="h-3 w-3" /> {job.posted}
                </Badge>
                <Badge className="flex items-center gap-1">{job.salary}</Badge>
              </div>

              <div className="space-y-6">
                <div>
                  <h2 className="text-xl font-semibold mb-3">About the Role</h2>
                  <div className="text-muted-foreground whitespace-pre-line">
                    {job.description}
                  </div>
                </div>

                <div>
                  <h2 className="text-xl font-semibold mb-3">Requirements</h2>
                  <ul className="space-y-2">
                    {job.requirements.map((req, i) => (
                      <li key={i} className="flex items-start gap-2 text-muted-foreground">
                        <CheckCircle2 className="h-5 w-5 text-green-500 mt-0.5 flex-shrink-0" />
                        {req}
                      </li>
                    ))}
                  </ul>
                </div>

                <div>
                  <h2 className="text-xl font-semibold mb-3">Benefits</h2>
                  <ul className="space-y-2">
                    {job.benefits.map((benefit, i) => (
                      <li key={i} className="flex items-start gap-2 text-muted-foreground">
                        <CheckCircle2 className="h-5 w-5 text-blue-500 mt-0.5 flex-shrink-0" />
                        {benefit}
                      </li>
                    ))}
                  </ul>
                </div>

                <div>
                  <h2 className="text-xl font-semibold mb-3">Skills</h2>
                  <div className="flex flex-wrap gap-2">
                    {job.skills.map((skill) => (
                      <Badge key={skill} variant="secondary">{skill}</Badge>
                    ))}
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          <Card className="sticky top-24">
            <CardContent className="p-6">
              <div className="space-y-4">
                <Button className="w-full" size="lg">
                  Apply Now
                </Button>
                <Button variant="outline" className="w-full">
                  <Bookmark className="mr-2 h-4 w-4" /> Save Job
                </Button>
                <Button variant="ghost" className="w-full">
                  <Share2 className="mr-2 h-4 w-4" /> Share
                </Button>
              </div>

              <div className="mt-6 pt-6 border-t">
                <h3 className="font-semibold mb-4">About Company</h3>
                <div className="space-y-3 text-sm">
                  <div className="flex items-center gap-2 text-muted-foreground">
                    <Building2 className="h-4 w-4" /> TechCorp Inc.
                  </div>
                  <div className="flex items-center gap-2 text-muted-foreground">
                    <MapPin className="h-4 w-4" /> San Francisco, CA
                  </div>
                  <div className="flex items-center gap-2 text-muted-foreground">
                    <Briefcase className="h-4 w-4" /> 50-200 employees
                  </div>
                  <a href="#" className="flex items-center gap-2 text-primary hover:underline">
                    <ExternalLink className="h-4 w-4" /> Visit Website
                  </a>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  )
}
