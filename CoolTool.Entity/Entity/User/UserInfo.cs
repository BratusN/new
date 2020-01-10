using CoolTool.Entity.Identity;
using System;

namespace CoolTool.Entity.User
{
    public class UserInfo
    {
        public long UserInfoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Company Company { get; set; }

        public long IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

        public DateTime? CreateTs { get; set; }
        public DateTime? UpdateTs { get; set; }
    }
}
