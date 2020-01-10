namespace CoolTool.Dto
{
    public class ExternalRegisterDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Provider { get; set; }

        public string ProviderUserId { get; set; }
    }
}
