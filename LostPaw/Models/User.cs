using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LostPaw.Models;

public class User : IdentityUser
{
    public string FullName { get; set; }
    public IEnumerable<PetPost> Posts { get; set; }
}

