using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SABlog.Repository
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int userId);

        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        int AddUser(User userEntity);
        int UpdateUser(User currentUser, User updatedUser);
        int UpdatePassword(User currentUser, string newPassword);
        void DeleteUser(int userId);

        User validateUser(string email, string password);

        string hashPassword(string password, string saltString);

        bool checkUsername(string username);
        bool checkEmail(string email);
        void PopulateUserRolesIfNotPresent();
    }
}
