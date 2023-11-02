namespace Api.Security
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationAttribute : Attribute
    {
        public readonly AuthorizationType? Authorization = null;
        public readonly bool OnlySuperUser = false;

        public AuthorizationAttribute(AuthorizationType authorization, bool onlySuperUser = false)
        {
            this.Authorization = authorization;
            this.OnlySuperUser = onlySuperUser;
        }
    }
}
