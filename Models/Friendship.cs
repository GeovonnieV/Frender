using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Frender.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipId {get; set;}

        public int FriendId {get; set;}
        public virtual User Friend { get; set; }

        public int FriendWithId {get; set;}
        public virtual User FriendWith { get; set; }

    }

}