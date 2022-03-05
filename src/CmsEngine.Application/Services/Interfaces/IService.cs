namespace CmsEngine.Application.Services.Interfaces;

public interface IService
{
    InstanceViewModel Instance { get; }
    UserViewModel CurrentUser { get; }
}
