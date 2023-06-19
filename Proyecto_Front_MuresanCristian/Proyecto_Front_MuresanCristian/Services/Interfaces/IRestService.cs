using Proyecto_Front_MuresanCristian.Entities;

namespace Proyecto_Front_MuresanCristian.Services.Interfaces
{
    public interface IRestService
    {
        public Task<bool> AddUsser(User user);
        public Task<UserInfo> Login(User user);
        public Task<UserInfo> RegenerateToken(UserInfo userInfo);
        public Task<List<Note>> GetUserNotes(UserInfo userInfo);
        public Task<List<Note>> GetSharedUserNotes(UserInfo userInfo);
        public Task<Note> AddNote(Note newNote, UserInfo userInfo);
        public Task<Note> EditNote(Note editedNote, UserInfo userInfo);
        public Task<bool> DeleteNote(Note note, UserInfo userInfo);
        public Task<bool> AddFav(Note note, UserInfo userInfo);
        public Task<bool> RemoveFav(Note note, UserInfo userInfo);
        public Task<bool> GetUserByEmail(string email, UserInfo userInfo);
        public Task<bool> AddSharedNote(string email, Note note, UserInfo userInfo);


    }
}
