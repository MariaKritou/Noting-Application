using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SQW.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorCookieAuth.Data
{
  public class UserService
  {
    private readonly ISQWWorker worker;

    public UserService(ISQWWorker worker)
    {
      this.worker = worker;
    }

    public void createUser(User user)
    {
      worker.run(context =>
      {
        context.createCommand(@"INSERT INTO MARIADEMO.NOTEBOOK_USERS (NAME, PASSWORD) 
                                                          VALUES(:NAME, :PASSWORD)")
        .addStringInParam("NAME", user.name)
        .addStringInParam("PASSWORD", user.password)
        .execute();
      });
    }

    public void editUser(User user)
    {
      worker.run(context =>
      {
        context
        .createCommand("UPDATE MARIADEMO.NOTEBOOK_USERS SET NAME=:NAME, PASSWORD=:PASSWORD WHERE USER_ID=:USER_ID")
        .addStringInParam("NAME", user.name)
        .addStringInParam("PASSWORD", user.password)
        .addNumericInParam("USER_ID", user.userId)
        .execute();
      });
    }

    public void deleteUser(int userId)
    {
      worker.run(context =>
      {
        context
        .createCommand("DELETE FROM MARIADEMO.NOTEBOOK_USERS WHERE USER_ID=:USER_ID")
        .addNumericInParam("USER_ID", userId)
        .execute();
      });
    }

    public User getUserById(int userId)
    {

      User user = null;

      worker.run(context =>
      {
        user = context
        .createCommand("SELECT * FROM MARIADEMO.NOTEBOOK_USERS WHERE USER_ID=:USER_ID")
        .addNumericInParam("USER_ID", userId)
        .first<User>();
      });

      return user;
    }

    public User login(string name, string password)
    {

      User user = null;
      worker.run(context =>
      {
        user = context
        .createCommand("SELECT * FROM MARIADEMO.NOTEBOOK_USERS WHERE NAME=:NAME AND PASSWORD=:PASSWORD")
        .addStringInParam("NAME", name)
        .addStringInParam("PASSWORD", password)
        .first<User>();

      });

      if (user == null)
      {
        return user;
      }

      if (password == user.password)
      {
        return user;
      }
      return null;
    }
  }
}
