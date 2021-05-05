using SQW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCookieAuth.Data
{
  public class User
  {
    [SQWFieldMap("USER_ID")]
    public int userId { get; set; }
    [Required]
    [SQWFieldMap("NAME")]
    public string name { get; set; }
    [Required]
    [SQWFieldMap("PASSWORD")]
    public string password { get; set; }
  }
}
