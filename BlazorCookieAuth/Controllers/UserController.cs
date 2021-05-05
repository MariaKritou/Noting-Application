using Microsoft.AspNetCore.Mvc;
using BlazorCookieAuth.Data;
using System.Security.Claims;

namespace BlazorCookieAuth.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : Controller
  {
    // /api/User/GetUser
    [HttpGet("[action]")]
    public User GetUser()
    {
      // Instantiate a UserModel
      var userModel = new User();
      userModel.name =User.Identity.Name;
      userModel.userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);  

      return userModel;
    }
  }
}