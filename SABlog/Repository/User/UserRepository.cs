using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SABlog.Repository
{
    public class UserRepository : BlogDatabaseEntities1, IUserRepository
    {

        private readonly BlogDatabaseEntities1 _context;
        private const int SaltSize = 24;
        private const int HashSize = 24;
        private const int Iterations = 10000;

        public UserRepository(BlogDatabaseEntities1 context)
        {
            _context = context;
        }

        public string hashPassword(string password, string saltString)
        {
            byte[] salt;
            if(saltString == "")
            {
                // Generate a salt
                RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
                salt = new byte[SaltSize];
                provider.GetBytes(salt);
            }
            else
            {
                salt = Convert.FromBase64String(saltString);
            }

            // Generate the hash
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            return string.Join(":",
                Convert.ToBase64String(hash),
                Convert.ToBase64String(salt));
        }

        public User validateUser(string email, string password)
        {
            if (email == null || password == null) return null;
            User user = _context.Users.Where(it => it.Email == email || it.UserName == email).FirstOrDefault();
            if (user == null) return null;


            string hashedPassword = hashPassword(password, user.Password.Split(':').Last());
            if (hashedPassword != user.Password) return null;
            
            return user;
        }

       public int AddUser(User userEntity)
        {
            if (userEntity == null) return -1;
            if (_context.Users.Where(it => it.Email == userEntity.Email).Count() != 0) return -1;
            if (_context.Users.Where(it => it.UserName == userEntity.UserName).Count() != 0) return -1;

            userEntity.Password = hashPassword(userEntity.Password, "");

            if(userEntity.Role == null)
            {
                userEntity.Role = _context.Roles.Where(it => it.Name == "user").First();
            }
            userEntity.CreatedAt = DateTime.Now;
            _context.Users.Add(userEntity);
            _context.SaveChanges();
            return userEntity.UserId;
        }

       public void DeleteUser(int userId)
        {
            User user = _context.Users.Find(userId);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

       public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

       public int UpdateUser(User currentUser, User updatedUser)
        {
            if (currentUser == null || updatedUser == null) return -1;
            if(updatedUser.Password != currentUser.Password)
            {
                updatedUser.Password = hashPassword(updatedUser.Password, currentUser.Password.Split(':').Last());
            }

            if (currentUser.Role == null)
            {
                currentUser.Role = _context.Roles.Where(it => it.Name == "user").First();
            }

            _context.Entry(currentUser).CurrentValues.SetValues(updatedUser);
            _context.SaveChanges();
            return currentUser.UserId;
        }

       public int UpdatePassword(User currentUser, string newPassword)
        {
            if (currentUser == null || newPassword == null) return -1;

            string hashedPassword = hashPassword(newPassword, currentUser.Password.Split(':').Last());
            if (currentUser.Role == null)
            {
                currentUser.Role = _context.Roles.Where(it => it.Name == "user").First();
            }

            _context.Entry(currentUser).CurrentValues.SetValues(new { Password = hashedPassword });
            _context.SaveChanges();
            return currentUser.UserId;
        }


        public bool checkUsername(string username)
        {
            return _context.Users.Where(it => it.UserName == username).Count() != 0;
        }

        public bool checkEmail(string email)
        {
            return _context.Users.Where(it => it.Email == email).Count() != 0;
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.Where(it => it.UserName == username).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.Where(it => it.Email == email).FirstOrDefault();
        }

        public void PopulateUserRolesIfNotPresent()
        {
            if (_context.Roles.Count() == 0)
            {
                var user = new Models.Role();
                user.Name = "user";
                user.Description = "Perdorues i thjeshte";
                user.CreatedAt = DateTime.Now;

                var admin = new Models.Role();
                admin.Name = "admin";
                admin.Description = "Perdorues i privilegjuar";
                admin.CreatedAt = DateTime.Now;

                _context.Roles.Add(admin);
                _context.Roles.Add(user);
                _context.SaveChanges();
            }
        }
    }
}