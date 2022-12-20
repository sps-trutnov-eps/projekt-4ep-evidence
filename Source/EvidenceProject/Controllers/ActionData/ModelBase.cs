namespace EvidenceProject.Controllers.ActionData;
public class ModelBase    
{
    public string? Response { get; set; }

    public ModelBase SetError(string? error = null)
    {
        Response = error == null ? UniversalHelper.SomethingWentWrongMessage : error;
        return this;
    }
}
