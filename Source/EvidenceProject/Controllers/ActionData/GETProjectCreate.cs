using EvidenceProject.Data.DataModels;

namespace EvidenceProject.Controllers.ActionData
{
    public class GETProjectCreate
    {
        public List<DialCode>? DialCodes { get; set; }
        public List<DialInfo>? DialInfos { get; set; }

        public List<AuthUser>? Users { get; set; }
    }
}
