namespace TalentHub.Domain.Enums;

public enum UserRole
{
    Candidate = 0,
    Company = 1,
    Admin = 2
}

public enum EmploymentType
{
    FullTime = 0,
    PartTime = 1,
    Contract = 2,
    Internship = 3,
    Freelance = 4
}

public enum ExperienceLevel
{
    Entry = 0,
    Mid = 1,
    Senior = 2,
    Lead = 3,
    Executive = 4
}

public enum RemoteType
{
    Remote = 0,
    Hybrid = 1,
    Onsite = 2
}

public enum JobStatus
{
    Draft = 0,
    Active = 1,
    Closed = 2,
    Expired = 3
}

public enum ApplicationStatus
{
    Applied = 0,
    Screening = 1,
    Interview = 2,
    TechnicalTest = 3,
    Offer = 4,
    Hired = 5,
    Rejected = 6,
    Withdrawn = 7
}

public enum SubscriptionStatus
{
    Active = 0,
    PastDue = 1,
    Canceled = 2,
    Trialing = 3,
    Paused = 4
}

public enum PaymentProvider
{
    Stripe = 0,
    MercadoPago = 1
}

public enum PaymentStatus
{
    Pending = 0,
    Succeeded = 1,
    Failed = 2,
    Refunded = 3
}
