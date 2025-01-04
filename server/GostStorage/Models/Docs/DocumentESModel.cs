using GostStorage.Entities;

namespace GostStorage.Models.Docs;

public class DocumentESModel
{
    public long Id { get; set; }

    public Field Field { get; set; }

    public string Data { get; set; }
}