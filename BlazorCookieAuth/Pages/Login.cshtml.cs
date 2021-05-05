using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorCookieAuth.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorCookieAuth.Server.Pages
{
  [AllowAnonymous]
  public class LoginModel : PageModel
  {
    private readonly UserService userService;
    public string ReturnUrl { get; set; }

    public LoginModel(UserService userService)
    {
      this.userService = userService;
    }

    public async Task<IActionResult>
        OnGetAsync(string paramUsername, string paramPassword)
    {
      string returnUrl = Url.Content("~/");

      try
      {
        // Clear the existing external cookie
        await HttpContext
            .SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
      }
      catch { }

      // *** !!! This is where you would validate the user !!! ***
      // In this example we just log the user in
      // (Always log the user in for this demo)
      if (ModelState.IsValid)
      {
        var user = userService.login(paramUsername, paramPassword);

        if (user != null)
        {
          var claims = new List<Claim>
        {
          new Claim(ClaimTypes.Role, "user"),
          new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
          new Claim(ClaimTypes.Name, user.name)
        };

          var claimsIdentity = new ClaimsIdentity(
              claims, CookieAuthenticationDefaults.AuthenticationScheme);        

          try
          {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
          }
          catch (Exception ex)
          {
            string error = ex.Message;
          }
          return LocalRedirect(returnUrl);
        }
        return RedirectToPage("/");
      }
      return RedirectToPage("/");
    }
  }
}