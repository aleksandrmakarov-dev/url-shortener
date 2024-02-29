namespace Server.Infrastructure.Models;

public class Pagination
{
    public int Page { get; set; }   
    public int Size { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}