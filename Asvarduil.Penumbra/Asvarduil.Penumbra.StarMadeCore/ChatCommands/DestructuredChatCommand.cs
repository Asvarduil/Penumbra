namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands
{
    public class DestructuredChatCommand
    {
        public string SourcePlayerName;
        public string Command;

        public bool IsCommand => Command.StartsWith("!");
    }
}
