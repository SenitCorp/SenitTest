namespace Senit.Core.Messaging.Commands
{
    public class CommandResponse<T> where T : ICommandResponse
    {
        public CommandResponse() { }
        public CommandResponse(T response) => Response = response;

        public CommandResponse(CommandError error) => Error = error;

        public bool IsSuccessful => Error == null;

        public CommandError Error { get; set; }

        public T Response { get; set; }

        public static CommandResponse<T> FromResponse(T response) => new CommandResponse<T>(response);

        public static CommandResponse<T> FromError(CommandError error) => new CommandResponse<T>(error);
    }
}