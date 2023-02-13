namespace KFEOCH.Models.Binding
{
    public class UserWithRoles
    {
        public string? Id { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
