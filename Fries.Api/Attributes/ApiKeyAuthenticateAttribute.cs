namespace Fries.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiKeyAuthenticateAttribute : Attribute
    {
    }
}
