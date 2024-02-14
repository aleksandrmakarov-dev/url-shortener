namespace Server.Csharp.Data.Models
{
    public class Role:DomainObject
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
