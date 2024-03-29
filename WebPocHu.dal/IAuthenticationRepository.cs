using WebPocHub.Models;

namespace WebPocHu.Dal
{
    public interface IAuthenticationRepository
    {
        int RegisterUser(User user); 
        User? CheckCredentials(User user);
        string GetUserRole(int roleId);
    }
} 
