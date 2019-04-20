using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CmsEngine
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
        Published = 0,

        [Display(Name = "Pending approval")]
        PendingApproval = 1,

        Draft = 2
    }

    /// <summary>
    /// Represents the general status of an object
    /// <para>It's analogous to Bootstrap status colors</para>
    /// </summary>
    public enum GeneralStatus
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark
    }

    public enum PageType
    {
        Create,
        Edit,
        List
    }

    /// <summary>
    /// In addition to allowing you to use your own image, Gravatar has a number of built in options which you can also use as defaults. Most of these work by taking the requested email hash and using it to generate a themed image that is unique to that email address
    /// </summary>
    public enum DefaultImage
    {
        /// <summary>Default Gravatar logo</summary>
        [Description("")]
        Default,

        /// <summary>404 - do not load any image if none is associated with the email hash, instead return an HTTP 404 (File Not Found) response</summary>
        [Description("404")]
        Http404,

        /// <summary>Mystery-Man - a simple, cartoon-style silhouetted outline of a person (does not vary by email hash)</summary>
        [Description("mm")]
        MysteryMan,

        /// <summary>Identicon - a geometric pattern based on an email hash</summary>
        [Description("identicon")]
        Identicon,

        /// <summary>MonsterId - a generated 'monster' with different colors, faces, etc</summary>
        [Description("monsterid")]
        MonsterId,

        /// <summary>Wavatar - generated faces with differing features and backgrounds</summary>
        [Description("wavatar")]
        Wavatar,

        /// <summary>Retro - awesome generated, 8-bit arcade-style pixelated faces</summary>
        [Description("retro")]
        Retro
    }

    /// <summary>
    /// Gravatar allows users to self-rate their images so that they can indicate if an image is appropriate for a certain audience. By default, only 'G' rated images are displayed unless you indicate that you would like to see higher ratings
    /// </summary>
    public enum Rating
    {
        /// <summary>Suitable for display on all websites with any audience type</summary>
        [Description("g")]
        G,

        /// <summary>May contain rude gestures, provocatively dressed individuals, the lesser swear words, or mild violence</summary>
        [Description("pg")]
        PG,

        /// <summary>May contain such things as harsh profanity, intense violence, nudity, or hard drug use</summary>
        [Description("r")]
        R,

        /// <summary>May contain hardcore sexual imagery or extremely disturbing violence</summary>
        [Description("x")]
        X
    }
}
