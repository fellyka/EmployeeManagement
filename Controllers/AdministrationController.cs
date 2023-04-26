using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AdministrationController : Controller
    {
        public readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole( CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                //Create Role in tthe Database
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                //Should errors occur

                foreach(IdentityError error in result.Errors)
                {
                   ModelState.AddModelError("",error.Description);
                }

            }
           
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles() 
        {
          var roles =   _roleManager.Roles;
          return View(roles);
        }
    }

}
