using Enteties.Abstract;

namespace Enteties
{
    public class SoundEntity : BaseEntity
    {
        public string SoundName { get; set; }
        public string HotKey { get; set; }
        public string FullPath { get; set; }
    }
}
