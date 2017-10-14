namespace Asvarduil.Penumbra.StarMadeCore.ServerEventCommands
{
    public interface IServerEvent
    {
        bool TryParseServerEvent(string rawData, out string[] eventArgs);
        string OnEventRecognized(string[] eventArgs);
    }
}
