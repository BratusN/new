using System;
using System.Collections.Generic;

namespace CoolTool.Entity.User
{
    public class Company
    {
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public List<UserInfo> Users { get; set; }
    }
}
