namespace Asvarduil.Penumbra.DataCore.Models
{
    public class OperationResult
    {
        public string Message;

        public bool IsSuccessful => string.IsNullOrEmpty(Message);
    }
}
