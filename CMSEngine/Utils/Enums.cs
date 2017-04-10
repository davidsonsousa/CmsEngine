using System.ComponentModel;

namespace CMSEngine
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum Operations
    {
        Coalesce,
        Equals,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        NotContain,
        StartsWith,
        EndsWith
    }

    public enum LogicalOperators
    {
        And,
        AndAlso,
        Or,
        OrElse
    }

    public enum SignInStatus
    {
        Success,
        LockedOut,
        RequiresVerification,
        Failure
    }

    /// <summary>
    /// Represents the status of the documents created in the website
    /// </summary>
    public enum DocumentStatus
    {
        [Description("Published")]
        Published = 0,
        [Description("Pending approval")]
        PendingApproval = 1,
        [Description("Draft")]
        Draft = 2
    }

    /// <summary>
    /// Represents the general status of an object
    /// <para>It's analogous to Bootstrap status colors</para>
    /// </summary>
    public enum GeneralStatus
    {
        Default,
        Primary,
        Success,
        Info,
        Warning,
        Danger
    }
}