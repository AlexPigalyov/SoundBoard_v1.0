using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Data.Repositories.Abstract;
using Enteties;

namespace Data.Repositories
{
    public class SoundRepository : BaseRepository<SoundEntity>, ISoundRepository
    {
        public SoundRepository(SoundBoardContext context) : base(context)
        {

        }
    }
}
