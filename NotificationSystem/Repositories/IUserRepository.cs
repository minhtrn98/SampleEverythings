using NotificationSystem.Entities;

namespace NotificationSystem.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);
}
