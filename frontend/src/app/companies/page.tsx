"use client"

import Link from "next/link"
import { useState } from "react"
import { useQuery } from "@tanstack/react-query"
import { Card, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Search, MapPin, Briefcase, ExternalLink, Loader2 } from "lucide-react"
import api, { type CompanyProfile, type PaginatedResult } from "@/lib/api"

export default function CompaniesPage() {
  const [searchQuery, setSearchQuery] = useState("")
  const [page, setPage] = useState(1)

  const { data, isLoading } = useQuery<PaginatedResult<CompanyProfile>>({
    queryKey: ["companies", searchQuery, page],
    queryFn: async () => {
      const params = new URLSearchParams()
      if (searchQuery) params.set("query", searchQuery)
      params.set("page", String(page))
      params.set("pageSize", "12")
      const { data } = await api.get(`/Companies?${params.toString()}`)
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
        <h1 className="text-3xl font-bold mb-2">Top Companies</h1>
        <p className="text-muted-foreground">Discover amazing companies hiring now</p>
      </div>

      {/* Search */}
      <form onSubmit={handleSearch} className="flex gap-4 mb-8">
        <div className="flex-1 flex items-center gap-2 bg-muted rounded-lg px-4">
          <Search className="h-5 w-5 text-muted-foreground" />
          <Input
            placeholder="Search companies..."
            className="border-0 bg-transparent focus-visible:ring-0"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
        <Button type="submit">Search</Button>
      </form>

      {/* Companies Grid */}
      {isLoading ? (
        <div className="flex items-center justify-center py-20">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      ) : data?.items.length === 0 ? (
        <div className="text-center py-20">
          <p className="text-lg font-medium mb-2">No companies found</p>
          <p className="text-muted-foreground">Try a different search term</p>
        </div>
      ) : (
        <>
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {data?.items.map((company) => (
              <Link key={company.id} href={`/companies/${company.id}`}>
                <Card className="card-hover cursor-pointer group h-full">
                  <CardContent className="p-6">
                    <div className="flex items-start gap-4 mb-4">
                      {company.logoUrl ? (
                        <img src={company.logoUrl} alt={company.name} className="h-14 w-14 rounded-xl object-cover" />
                      ) : (
                        <div className="h-14 w-14 rounded-xl bg-gradient-to-br from-blue-500 to-violet-500 flex items-center justify-center text-white font-bold text-xl">
                          {company.name[0]}
                        </div>
                      )}
                      <div className="flex-1">
                        <h3 className="font-semibold text-lg group-hover:text-primary transition-colors">
                          {company.name}
                        </h3>
                        {company.industry && (
                          <p className="text-sm text-muted-foreground">{company.industry}</p>
                        )}
                      </div>
                    </div>

                    <div className="space-y-2 text-sm text-muted-foreground mb-4">
                      {company.location && (
                        <div className="flex items-center gap-2">
                          <MapPin className="h-4 w-4" /> {company.location}
                        </div>
                      )}
                      {company.companySize && (
                        <div className="flex items-center gap-2">
                          <Briefcase className="h-4 w-4" /> {company.companySize} employees
                        </div>
                      )}
                    </div>

                    <div className="flex items-center justify-between pt-4 border-t">
                      <Badge variant="secondary">
                        {company.totalJobs ?? 0} open positions
                      </Badge>
                      <Button variant="ghost" size="sm">
                        View Profile <ExternalLink className="ml-1 h-4 w-4" />
                      </Button>
                    </div>
                  </CardContent>
                </Card>
              </Link>
            ))}
          </div>

          {data && data.totalPages > 1 && (
            <div className="flex items-center justify-center gap-2 mt-8">
              <Button variant="outline" size="sm" disabled={!data.hasPreviousPage} onClick={() => setPage((p) => p - 1)}>
                Previous
              </Button>
              <span className="text-sm text-muted-foreground">
                Page {data.page} of {data.totalPages}
              </span>
              <Button variant="outline" size="sm" disabled={!data.hasNextPage} onClick={() => setPage((p) => p + 1)}>
                Next
              </Button>
            </div>
          )}
        </>
      )}
    </div>
  )
}
