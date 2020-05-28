using Models.Abstract;

namespace Models
{
    public class SourceModel : BaseModel
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int Channels { get; set; }
    }
}
