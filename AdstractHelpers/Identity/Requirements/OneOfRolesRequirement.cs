using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class OneOfRolesRequirement : IAuthorizationRequirement
{
    public string[] Roles { get; }

    public OneOfRolesRequirement(params string[] roles)
    {
        Roles = roles;
    }
}