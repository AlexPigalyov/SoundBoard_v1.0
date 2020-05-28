using Models.Abstract;

namespace Models
{
    public class SoundModel : BaseModel
    {
        public string SoundName { get; set; }
        public string HotKey { get; set; }
        public string FullPath { get; set; }
    }
}
