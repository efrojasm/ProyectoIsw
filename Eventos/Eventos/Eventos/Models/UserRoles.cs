using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventos.Models
{
    public class UserRoles
    {
        public List<SelectListItem> userRoles;
        public UserRoles()
        {
            userRoles = new List<SelectListItem>();
        }
        public List<SelectListItem> getRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToList();
            foreach(var Data in roles)
            {
                userRoles.Add(new SelectListItem()
                {
                    Value= Data.Id,
                    Text= Data.Name
                });
            }
            return userRoles;
        }
    }
}
