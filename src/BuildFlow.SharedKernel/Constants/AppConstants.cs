namespace BuildFlow.SharedKernel;

/// <summary>
/// Application-wide constants. Single source of truth for magic strings and configuration defaults.
/// </summary>
public static class AppConstants
{
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string TenantAdmin = "TenantAdmin";
        public const string ProjectManager = "ProjectManager";
        public const string SiteEngineer = "SiteEngineer";
        public const string Supervisor = "Supervisor";
        public const string Accountant = "Accountant";
        public const string PurchasingOfficer = "PurchasingOfficer";
        public const string Contractor = "Contractor";
        public const string Customer = "Customer";
        public const string Worker = "Worker";
    }

    public static class Claims
    {
        public const string TenantId = "tenant_id";
        public const string UserId = "user_id";
        public const string Email = "email";
        public const string FullName = "full_name";
        public const string Role = "role";
    }

    public static class Policies
    {
        public const string RequireTenant = "RequireTenant";
        public const string RequireSuperAdmin = "RequireSuperAdmin";
        public const string RequireTenantAdmin = "RequireTenantAdmin";
    }

    public static class Pagination
    {
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 100;
    }

    public static class Cache
    {
        public const int DefaultExpiryMinutes = 5;
        public const int LongExpiryMinutes = 60;
    }
}
