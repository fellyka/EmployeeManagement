using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
           _userManager = userManager;
           _signInManager = signInManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /*
         This method does client-side validation. It check whether the email has been used 
         by another person in the system to register himself. This validation is done on the
         server side
         */
        [HttpPost]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
           if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid) // Is model valid?
            { 
                //Copy data from RegisterViewModel to IdentityUser
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, City = model.City };

                //Store data in AspNetUsers db table
                //The password is present in our model of incoming object - Read also about Data Binding
                //The Password is hashed and securely stored in the underlying database

                var result = await _userManager.CreateAsync(user, model.Password);

                /* If user is succesfully created, sign-in the user using SignInManager and redirect to 
                   index action of HomeController
                 */
                if(result.Succeeded) 
                {
                    if(_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers","Administration");
                    }

                  await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                /* If there are any errors, add them to the ModelState object which will be 
                   displayed by the validation summary tag helper */
                foreach(var error in result.Errors) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        /* We'll se a POST request to log the user out. Using a GET request to log out the user is not recommended
           because the approcah may be absued. A malicious user may trick you into clicking an image element where the 
           src attribute is set to the application logout url. As a result you'll be unknowingly logget out */
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","home");
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid) //If data are ok
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,model.Password,model.RememberMe,false);

                if(result.Succeeded) 
                {
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        /* This can cause a serious security risk : The Open Redirect Attack 
                           Meaning you might be redirected to an unknown(or uncoded) site

                        An application becomes vulnerable to Open Redirect Attacks:
                          **It redirects to an URL that's specified via the request such as
                          **uerystring or from data
                          *
                          **The redirection is performed without checking if the URL is a local URL
                         */
                       // return Redirect(returnUrl);
                       return LocalRedirect(returnUrl);
                    }
                   
                }
                else
                {
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
