namespace Asvarduil.Penumbra.Services.Settings
{
    /// <summary>
    /// Describes the application's execution environment.
    /// </summary>
    public enum Environment
    {
        /// <summary>
        /// Used for working on the application.
        /// </summary>
        DEV,
        /// <summary>
        /// A prod-like environment for testing work.
        /// </summary>
        TEST,
        /// <summary>
        /// What end-users actually get to use.
        /// </summary>
        PROD
    }
}
