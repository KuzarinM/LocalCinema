using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class AuthorizeOneOfRolesAttribute : AuthorizeAttribute
{
    public AuthorizeOneOfRolesAttribute(params string[] roles)
    {
        // Формируем уникальное имя политики
        Policy = $"OneOfRoles:{string.Join(",", roles)}";
    }
}
