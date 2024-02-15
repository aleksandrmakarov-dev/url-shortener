namespace Server.Csharp.Data.Entities
{
    public class Role:DomainObject
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
