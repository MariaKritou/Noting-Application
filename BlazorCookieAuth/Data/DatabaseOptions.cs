using Newtonsoft.Json;

namespace BlazorCookieAuth.Data
{
  public class DatabaseOptions
  {
    [JsonProperty("dbServer")]
    public string dbServer { get; set; }

    [JsonProperty("dbInstance")]
    public string dbInstance { get; set; }

    [JsonProperty("userName")]
    public string userName { get; set; }

    [JsonProperty("password")]
    public string password { get; set; }
  }
}
