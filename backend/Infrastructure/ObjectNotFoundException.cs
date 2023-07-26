namespace backend.Infrastructure
{
    [Serializable]
    public sealed class ObjectNotFoundException : System.Data.DataException
    {
        public string Property { get; protected set; }

        public ObjectNotFoundException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
