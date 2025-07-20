using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class OneOfRolesHandler : AuthorizationHandler<OneOfRolesRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OneOfRolesRequirement requirement)
    {
        // Проверяем, имеет ли пользователь хотя бы одну из требуемых ролей
        if (requirement.Roles.Any(role => context.User.IsInRole(role)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
