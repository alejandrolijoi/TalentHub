"use client"

import Link from "next/link"
import { Card, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Search, MapPin, Briefcase, ExternalLink } from "lucide-react"

const companies = [
  { name: "TechCorp Inc.", industry: "Technology", location: "San Francisco, CA", size: "200-500", jobs: 12, logo: "T", color: "from-blue-500 to-cyan-500" },
  { name: "StartupXYZ", industry: "SaaS", location: "New York, NY", size: "50-200", jobs: 8, logo: "S", color: "from-violet-500 to-purple-500" },
  { name: "CloudInc", industry: "Cloud Computing", location: "Remote", size: "100-500", jobs: 15, logo: "C", color: "from-green-500 to-emerald-500" },
  { name: "DigitalCo", industry: "Digital Agency", location: "Austin, TX", size: "10-50", jobs: 5, logo: "D", color: "from-orange-500 to-amber-500" },
  { name: "AI Labs", industry: "Artificial Intelligence", location: "Boston, MA", size: "50-200", jobs: 10, logo: "A", color: "from-pink-500 to-rose-500" },
  { name: "SaaS Inc", industry: "Software", location: "San Francisco, CA", size: "200-500", jobs: 7, logo: "S", color: "from-indigo-500 to-blue-500" },
]

export default function CompaniesPage() {
  return (
    <div className="container py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold mb-2">Top Companies</h1>
        <p className="text-muted-foreground">Discover amazing companies hiring now</p>
      </div>

      {/* Search */}
      <div className="flex gap-4 mb-8">
        <div className="flex-1 flex items-center gap-2 bg-muted rounded-lg px-4">
          <Search className="h-5 w-5 text-muted-foreground" />
          <Input
            placeholder="Search companies..."
            className="border-0 bg-transparent focus-visible:ring-0"
          />
        </div>
        <Button>Search</Button>
      </div>

      {/* Companies Grid */}
      <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
        {companies.map((company) => (
          <Card key={company.name} className="card-hover cursor-pointer group">
            <CardContent className="p-6">
              <div className="flex items-start gap-4 mb-4">
                <div className={`h-14 w-14 rounded-xl bg-gradient-to-br ${company.color} flex items-center justify-center text-white font-bold text-xl`}>
                  {company.logo}
                </div>
                <div className="flex-1">
                  <h3 className="font-semibold text-lg group-hover:text-primary transition-colors">
                    {company.name}
                  </h3>
                  <p className="text-sm text-muted-foreground">{company.industry}</p>
                </div>
              </div>

              <div className="space-y-2 text-sm text-muted-foreground mb-4">
                <div className="flex items-center gap-2">
                  <MapPin className="h-4 w-4" /> {company.location}
                </div>
                <div className="flex items-center gap-2">
                  <Briefcase className="h-4 w-4" /> {company.size} employees
                </div>
              </div>

              <div className="flex items-center justify-between pt-4 border-t">
                <Badge variant="secondary">{company.jobs} open positions</Badge>
                <Button variant="ghost" size="sm">
                  View Profile <ExternalLink className="ml-1 h-4 w-4" />
                </Button>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  )
}
