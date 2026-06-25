import Link from "next/link"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Card, CardContent } from "@/components/ui/card"
import { Search, MapPin, Building2, ArrowRight, Star, Zap, Shield, Users } from "lucide-react"

export default function Home() {
  return (
    <div className="flex flex-col">
      {/* Hero Section */}
      <section className="relative overflow-hidden bg-gradient-to-br from-blue-50 via-white to-violet-50">
        <div className="absolute inset-0 bg-[url('/grid.svg')] opacity-5" />
        <div className="container relative py-24 md:py-32">
          <div className="mx-auto max-w-3xl text-center">
            <Badge className="mb-4">1,234+ Jobs Available</Badge>
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
            <div className="mx-auto max-w-2xl">
              <Card className="p-2 shadow-xl">
                <div className="flex flex-col sm:flex-row gap-2">
                  <div className="flex-1 flex items-center gap-2 px-4">
                    <Search className="h-5 w-5 text-muted-foreground" />
                    <input
                      type="text"
                      placeholder="Job title, keyword, or company"
                      className="w-full py-3 bg-transparent outline-none"
                    />
                  </div>
                  <div className="flex-1 flex items-center gap-2 px-4 border-t sm:border-t-0 sm:border-l">
                    <MapPin className="h-5 w-5 text-muted-foreground" />
                    <input
                      type="text"
                      placeholder="Location or Remote"
                      className="w-full py-3 bg-transparent outline-none"
                    />
                  </div>
                  <Button size="lg" className="px-8">
                    Search Jobs
                  </Button>
                </div>
              </Card>
            </div>

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

        {/* Floating Elements */}
        <div className="absolute top-20 left-10 animate-float hidden lg:block">
          <div className="glass rounded-xl p-3 shadow-lg">
            <div className="flex items-center gap-2">
              <div className="h-10 w-10 rounded-full bg-blue-100 flex items-center justify-center">
                <Building2 className="h-5 w-5 text-blue-600" />
              </div>
              <div>
                <p className="text-sm font-medium">Google</p>
                <p className="text-xs text-muted-foreground">12 open positions</p>
              </div>
            </div>
          </div>
        </div>
        <div className="absolute bottom-20 right-10 animate-float hidden lg:block" style={{ animationDelay: "1s" }}>
          <div className="glass rounded-xl p-3 shadow-lg">
            <div className="flex items-center gap-2">
              <div className="h-10 w-10 rounded-full bg-green-100 flex items-center justify-center">
                <Zap className="h-5 w-5 text-green-600" />
              </div>
              <div>
                <p className="text-sm font-medium">New Match!</p>
                <p className="text-xs text-muted-foreground">95% profile match</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Featured Companies */}
      <section className="py-16 border-b">
        <div className="container">
          <p className="text-center text-sm text-muted-foreground mb-8">
            Trusted by 500+ companies worldwide
          </p>
          <div className="flex flex-wrap justify-center items-center gap-8 md:gap-16 opacity-50">
            {["Google", "Microsoft", "Amazon", "Meta", "Apple", "Netflix"].map((company) => (
              <div key={company} className="text-xl md:text-2xl font-bold text-gray-400">
                {company}
              </div>
            ))}
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

          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            {[
              { title: "Senior React Developer", company: "TechCorp", location: "Remote", salary: "$80k-$100k", badge: "Featured", color: "from-blue-500 to-cyan-500" },
              { title: ".NET Backend Engineer", company: "StartupXYZ", location: "NYC Hybrid", salary: "$70k-$90k", badge: "Urgent", color: "from-violet-500 to-purple-500" },
              { title: "DevOps Lead", company: "CloudInc", location: "Remote", salary: "$90k-$110k", badge: "New", color: "from-green-500 to-emerald-500" },
              { title: "Full Stack Developer", company: "DigitalCo", location: "Austin, TX", salary: "$75k-$95k", badge: null, color: "from-orange-500 to-amber-500" },
              { title: "Data Scientist", company: "AI Labs", location: "Remote", salary: "$95k-$120k", badge: "Featured", color: "from-pink-500 to-rose-500" },
              { title: "Product Manager", company: "SaaS Inc", location: "SF Hybrid", salary: "$85k-$105k", badge: null, color: "from-indigo-500 to-blue-500" },
            ].map((job, i) => (
              <Link key={i} href={`/jobs/${i}`}>
                <Card className="h-full card-hover cursor-pointer group">
                  <CardContent className="p-6">
                    <div className="flex items-start justify-between mb-4">
                      <div className={`h-12 w-12 rounded-xl bg-gradient-to-br ${job.color} flex items-center justify-center text-white font-bold text-lg`}>
                        {job.company[0]}
                      </div>
                      {job.badge && (
                        <Badge variant={job.badge === "Featured" ? "default" : job.badge === "Urgent" ? "destructive" : "secondary"}>
                          {job.badge}
                        </Badge>
                      )}
                    </div>
                    <h3 className="font-semibold text-lg mb-1 group-hover:text-primary transition-colors">
                      {job.title}
                    </h3>
                    <p className="text-sm text-muted-foreground mb-3">{job.company}</p>
                    <div className="flex items-center gap-4 text-sm text-muted-foreground">
                      <span className="flex items-center gap-1">
                        <MapPin className="h-4 w-4" /> {job.location}
                      </span>
                    </div>
                    <div className="mt-4 pt-4 border-t flex items-center justify-between">
                      <span className="font-semibold text-primary">{job.salary}</span>
                      <span className="text-xs text-muted-foreground">2 days ago</span>
                    </div>
                  </CardContent>
                </Card>
              </Link>
            ))}
          </div>
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

      {/* Stats */}
      <section className="py-20">
        <div className="container">
          <div className="grid grid-cols-2 md:grid-cols-4 gap-8 text-center">
            {[
              { value: "10,000+", label: "Active Jobs" },
              { value: "5,000+", label: "Companies" },
              { value: "50,000+", label: "Candidates" },
              { value: "95%", label: "Satisfaction" },
            ].map((stat, i) => (
              <div key={i}>
                <div className="text-4xl md:text-5xl font-bold text-gradient mb-2">{stat.value}</div>
                <div className="text-muted-foreground">{stat.label}</div>
              </div>
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
