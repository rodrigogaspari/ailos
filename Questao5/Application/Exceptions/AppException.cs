namespace Questao5.Application.Exceptions
{
    public class AppException(IList<string> errors) : Exception(string.Join(", ", errors))
    {
        public IList<string> Errors { get; set; } = errors;
    }
}
