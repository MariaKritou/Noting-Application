using SQW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCookieAuth.Data
{
  public class Note
  {
    [SQWFieldMap("NOTE_ID")]
    public int noteId { get; set; }

    [Required]
    [SQWFieldMap("CATEGORY_ENUM")]
    public string category { get; set; }

    [Required]
    [SQWFieldMap("TEXT")]
    public string text { get; set; }
    [SQWFieldMap("SUBTEXT")]
    public string subtext { get; set; }
    public bool isChecked { get; set; }
    [SQWFieldMap("DATE_UPLOADED")]
    public DateTime date { get; set; }
  }
}
