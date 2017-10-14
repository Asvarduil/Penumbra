namespace Asvarduil.Penumbra.DataCore.Models
{
    public class BountyPostResult
    {
        public string Message;

        public bool IsSuccessful => string.IsNullOrEmpty(Message);
    }
}
