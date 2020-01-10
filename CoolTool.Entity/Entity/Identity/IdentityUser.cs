using Microsoft.AspNetCore.Identity;

namespace CoolTool.Entity.Identity
{
    /// <summary>
    /// Identifying information used for authorization, authentication, role definition and available user actions
    /// </summary>
    public class IdentityUser : IdentityUser<long>
    { }
}
