using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }
        [JsonIgnore]
        public byte[]? Salt { get; set; }
    }
}
