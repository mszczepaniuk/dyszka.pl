namespace Web.Services.Interfaces
{
    public interface IIdentityService
    {
        public void AddAuthentication<T>(T requestSender);
        public void RemoveAuthentication<T>(T requestSender);
    }
}