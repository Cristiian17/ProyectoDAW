using Proyecto_Front_MuresanCristian.Entities;

namespace Proyecto_Front_MuresanCristian.Services.Interfaces
{
    public interface IRestService
    {
        public Task<bool> AddUsser(User user);
        public Task<UserInfo> Login(User user);
        public Task<List<Note>> GetUserNotes(UserInfo userInfo);
        public Task<Note> GetSharedUserNotes();
    }
}
