namespace Post.Application.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, Guid id)
            : base($"{entityName} with Id {id} was not found.") { }

        public EntityNotFoundException(string message)
            : base(message) { }
    }
}
