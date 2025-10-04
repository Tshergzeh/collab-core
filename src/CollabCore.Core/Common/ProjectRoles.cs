namespace CollabCore.Core.Common
{
    public static class ProjectRoles
    {
        public const string Owner = "Owner";
        public const string PM = "PM";
        public const string Contributor = "Contributor";

        public static readonly string[] All = [Owner, PM, Contributor];

        public static bool IsValid(string role)
        {
            return All.Contains(role, StringComparer.OrdinalIgnoreCase);
        }
    }
}