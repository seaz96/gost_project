namespace Gost_Project.Data.Models;

public class DocWithViewsModel
{
    public long DocId { get; set; }
    
    public string? Designation { get; set; }

    public string? FullName { get; set; }

    public int Views { get; set; }
}