using System;
using System.ComponentModel.DataAnnotations;

namespace Enteties.Abstract
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
