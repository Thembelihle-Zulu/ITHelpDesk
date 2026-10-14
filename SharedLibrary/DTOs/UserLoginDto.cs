using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class UserLoginDto
    {
        [JsonPropertyName("username")]
        public string username { get; set; } = null!;

        [JsonPropertyName("password")]
        public string password { get; set; } = null!;


    }
}
