namespace Asvarduil.Penumbra.UnixCore
{
    /// <summary>
    /// Results of running a UNIX command.
    /// </summary>
    public class UnixCommandResult
    {
        public string Output = string.Empty;
        public string Errors = string.Empty;

        public bool IsSuccessful => Errors.Trim().Length == 0;
    }
}
