"use client"

import Link from "next/link"
import { useState } from "react"
import { useRouter } from "next/navigation"
import { useMutation, useQuery } from "@tanstack/react-query"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { ArrowLeft, Loader2, Plus, X } from "lucide-react"
import api from "@/lib/api"
import { useAuth } from "@/providers/auth-provider"

type EmploymentType = "FullTime" | "PartTime" | "Contract" | "Internship" | "Freelance"
type ExperienceLevel = "Entry" | "Mid" | "Senior" | "Lead" | "Executive"
type RemoteType = "Remote" | "Hybrid" | "Onsite"

interface Category {
  id: string
  name: string
  slug: string
}

interface Skill {
  id: string
  name: string
}

export default function NewJobPage() {
  const router = useRouter()
  const { user, isCompany } = useAuth()

  const [title, setTitle] = useState("")
  const [description, setDescription] = useState("")
  const [requirements, setRequirements] = useState("")
  const [benefits, setBenefits] = useState("")
  const [categoryId, setCategoryId] = useState("")
  const [employmentType, setEmploymentType] = useState<EmploymentType>("FullTime")
  const [experienceLevel, setExperienceLevel] = useState<ExperienceLevel>("Mid")
  const [location, setLocation] = useState("")
  const [remoteType, setRemoteType] = useState<RemoteType>("Remote")
  const [salaryMin, setSalaryMin] = useState("")
  const [salaryMax, setSalaryMax] = useState("")
  const [currency, setCurrency] = useState("USD")
  const [skillInput, setSkillInput] = useState("")
  const [selectedSkillIds, setSelectedSkillIds] = useState<string[]>([])
  const [error, setError] = useState("")

  const { data: categories } = useQuery<Category[]>({
    queryKey: ["categories"],
    queryFn: async () => {
      const { data } = await api.get("/Categories")
      return data
    },
  })

  const { data: allSkills } = useQuery<Skill[]>({
    queryKey: ["skills"],
    queryFn: async () => {
      const { data } = await api.get("/Skills")
      return data
    },
  })

  const createMutation = useMutation({
    mutationFn: async () => {
      const body = {
        title,
        description,
        requirements: requirements || null,
        benefits: benefits || null,
        categoryId: categoryId || null,
        employmentType,
        experienceLevel,
        location: location || null,
        remoteType,
        salaryMin: salaryMin ? Number(salaryMin) : null,
        salaryMax: salaryMax ? Number(salaryMax) : null,
        currency,
        skillIds: selectedSkillIds.length > 0 ? selectedSkillIds : null,
      }
      const { data } = await api.post("/Jobs", body)
      return data
    },
    onSuccess: () => {
      router.push("/dashboard/company")
    },
    onError: (err: any) => {
      setError(err.response?.data?.message || "Failed to create job. Please try again.")
    },
  })

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    setError("")
    createMutation.mutate()
  }

  const addSkill = () => {
    const trimmed = skillInput.trim()
    if (!trimmed) return
    const found = allSkills?.find((s) => s.name.toLowerCase() === trimmed.toLowerCase())
    if (found && !selectedSkillIds.includes(found.id)) {
      setSelectedSkillIds([...selectedSkillIds, found.id])
    } else if (!found && trimmed) {
      const customId = `custom-${Date.now()}`
      setCustomSkills([...customSkills, { id: customId, name: trimmed }])
    }
    setSkillInput("")
  }

  const [customSkills, setCustomSkills] = useState<{ id: string; name: string }[]>([])

  const removeSkill = (id: string) => {
    setSelectedSkillIds(selectedSkillIds.filter((s) => s !== id))
    setCustomSkills(customSkills.filter((s) => s.id !== id))
  }

  const allSelectedSkills = [
    ...(allSkills?.filter((s) => selectedSkillIds.includes(s.id)) || []),
    ...customSkills,
  ]

  if (!user) return null

  if (!isCompany) {
    return (
      <div className="container py-12 text-center">
        <p className="text-muted-foreground mb-4">Only companies can post jobs.</p>
        <Link href="/dashboard/candidate">
          <Button>Go to Candidate Dashboard</Button>
        </Link>
      </div>
    )
  }

  return (
    <div className="container py-8 max-w-3xl">
      <div className="flex items-center gap-4 mb-8">
        <Link href="/dashboard/company">
          <Button variant="ghost" size="icon">
            <ArrowLeft className="h-5 w-5" />
          </Button>
        </Link>
        <div>
          <h1 className="text-2xl font-bold">Post a New Job</h1>
          <p className="text-muted-foreground">Create a listing to attract top talent</p>
        </div>
      </div>

      <form onSubmit={handleSubmit}>
        <div className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Basic Information</CardTitle>
              <CardDescription>Job title, description, and requirements</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              {error && (
                <div className="p-3 text-sm text-destructive bg-destructive/10 rounded-lg">{error}</div>
              )}

              <div className="space-y-2">
                <Label htmlFor="title">Job Title *</Label>
                <Input id="title" value={title} onChange={(e) => setTitle(e.target.value)} placeholder="e.g. Senior .NET Engineer" required />
              </div>

              <div className="space-y-2">
                <Label htmlFor="description">Description *</Label>
                <textarea
                  id="description"
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  placeholder="Describe the role, responsibilities, and what makes it exciting..."
                  className="flex min-h-[120px] w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                  required
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="requirements">Requirements</Label>
                <textarea
                  id="requirements"
                  value={requirements}
                  onChange={(e) => setRequirements(e.target.value)}
                  placeholder="Required skills, experience, and qualifications..."
                  className="flex min-h-[100px] w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="benefits">Benefits</Label>
                <textarea
                  id="benefits"
                  value={benefits}
                  onChange={(e) => setBenefits(e.target.value)}
                  placeholder="Salary range, perks, remote policy, etc..."
                  className="flex min-h-[100px] w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Details</CardTitle>
              <CardDescription>Employment type, experience level, and location</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="employmentType">Employment Type *</Label>
                  <select
                    id="employmentType"
                    value={employmentType}
                    onChange={(e) => setEmploymentType(e.target.value as EmploymentType)}
                    className="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                  >
                    <option value="FullTime">Full-time</option>
                    <option value="PartTime">Part-time</option>
                    <option value="Contract">Contract</option>
                    <option value="Internship">Internship</option>
                    <option value="Freelance">Freelance</option>
                  </select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="experienceLevel">Experience Level *</Label>
                  <select
                    id="experienceLevel"
                    value={experienceLevel}
                    onChange={(e) => setExperienceLevel(e.target.value as ExperienceLevel)}
                    className="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                  >
                    <option value="Entry">Entry</option>
                    <option value="Mid">Mid</option>
                    <option value="Senior">Senior</option>
                    <option value="Lead">Lead</option>
                    <option value="Executive">Executive</option>
                  </select>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="category">Category</Label>
                  <select
                    id="category"
                    value={categoryId}
                    onChange={(e) => setCategoryId(e.target.value)}
                    className="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                  >
                    <option value="">Select category</option>
                    {categories?.map((cat) => (
                      <option key={cat.id} value={cat.id}>{cat.name}</option>
                    ))}
                  </select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="remoteType">Remote Type *</Label>
                  <select
                    id="remoteType"
                    value={remoteType}
                    onChange={(e) => setRemoteType(e.target.value as RemoteType)}
                    className="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                  >
                    <option value="Remote">Remote</option>
                    <option value="Hybrid">Hybrid</option>
                    <option value="Onsite">On-site</option>
                  </select>
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="location">Location</Label>
                <Input id="location" value={location} onChange={(e) => setLocation(e.target.value)} placeholder="e.g. Remote, Buenos Aires, AR" />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Compensation</CardTitle>
              <CardDescription>Salary range and currency</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="grid grid-cols-3 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="salaryMin">Min Salary</Label>
                  <Input id="salaryMin" type="number" value={salaryMin} onChange={(e) => setSalaryMin(e.target.value)} placeholder="60000" />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="salaryMax">Max Salary</Label>
                  <Input id="salaryMax" type="number" value={salaryMax} onChange={(e) => setSalaryMax(e.target.value)} placeholder="120000" />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="currency">Currency</Label>
                  <select
                    id="currency"
                    value={currency}
                    onChange={(e) => setCurrency(e.target.value)}
                    className="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                  >
                    <option value="USD">USD</option>
                    <option value="EUR">EUR</option>
                    <option value="GBP">GBP</option>
                    <option value="ARS">ARS</option>
                    <option value="BRL">BRL</option>
                    <option value="MXN">MXN</option>
                    <option value="COP">COP</option>
                    <option value="CLP">CLP</option>
                  </select>
                </div>
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Skills</CardTitle>
              <CardDescription>Add required skills for this position</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex gap-2">
                <Input
                  value={skillInput}
                  onChange={(e) => setSkillInput(e.target.value)}
                  placeholder="Type a skill name and press Add..."
                  onKeyDown={(e) => { if (e.key === "Enter") { e.preventDefault(); addSkill() } }}
                  list="skill-suggestions"
                />
                <Button type="button" variant="outline" onClick={addSkill}>
                  <Plus className="h-4 w-4" />
                </Button>
                <datalist id="skill-suggestions">
                  {allSkills?.filter((s) => !selectedSkillIds.includes(s.id)).map((s) => (
                    <option key={s.id} value={s.name} />
                  ))}
                </datalist>
              </div>

              {allSelectedSkills.length > 0 && (
                <div className="flex flex-wrap gap-2">
                  {allSelectedSkills.map((skill) => (
                    <div key={skill.id} className="flex items-center gap-1 px-3 py-1 rounded-full bg-primary/10 text-sm">
                      {skill.name}
                      <button type="button" onClick={() => removeSkill(skill.id)} className="hover:text-destructive">
                        <X className="h-3 w-3" />
                      </button>
                    </div>
                  ))}
                </div>
              )}
            </CardContent>
          </Card>

          <div className="flex gap-4 justify-end">
            <Link href="/dashboard/company">
              <Button type="button" variant="outline">Cancel</Button>
            </Link>
            <Button type="submit" disabled={createMutation.isPending}>
              {createMutation.isPending ? (
                <><Loader2 className="mr-2 h-4 w-4 animate-spin" /> Posting...</>
              ) : (
                "Post Job"
              )}
            </Button>
          </div>
        </div>
      </form>
    </div>
  )
}
