namespace CmsEngine.Application.Services.Interfaces;

public interface IService : IDisposable
{
    InstanceViewModel Instance { get; }
    UserViewModel CurrentUser { get; }
}
