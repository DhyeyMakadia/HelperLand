using System;
using System.Collections.Generic;

#nullable disable

namespace Helperland.Models
{
    public partial class FavoriteAndBlocked
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TargetUserId { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsBlocked { get; set; }

        public virtual User TargetUser { get; set; }
        public virtual User User { get; set; }
    }
}
