using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        /*The UserManager<IdentityUser> uses a generic parameter, the IdentityUser but we can create our own type that inherits from the IdentityUser.
          The UserManaher has got methods like CreateAsync, DeleteAsync, UpdateAsync,Etc... You can go to the definition of this methods to find out more.
          SignInManager is a service used for users sign in. It contains methods like: SignInAsync, SignOutAsync, IsSignedIn, Etc... It also accepts a generic
          parameter, so we can specify our own as well.
         */

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
          SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;

        //public AccountController(UserManager<ApplicationUser> userManager,
        //    SignInManager<ApplicationUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}

        //--------------------------------------------------Register Actions-----------------------------------------------------------------
        /*Why do we have a pair of Actions for Register? The first one respond to the HppGet et the second one respond to the HttpPost*/
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /*When we click the Register button, this method sends a post request for us to Register a new user.
         That's why we used the UserManager and the SignInManager provided by the .Net Core
         */
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            /*Check is model is valid*/
            if (ModelState.IsValid)
            {
                /*If the model is correct, let create a new identity object and copy the data that we have in the model. These data, if we refer to our
                 Register form are the user name(whicl in this case is the email),and the email which is the email */
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                /*We used the second overloaded version of the CreateAsync so that we can create the password as well. The password is hashed and securely
                 stored in the underlying database table
                 */
               var result = await _userManager.CreateAsync(user, model.Password);

                /*Let check if the user is created succesfully*/
                if(result.Succeeded)
                {
                    /*SignIn the specified user using the SignInManager*/
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "home");
                }
                /*If there are error when siging in, collect them here*/
                foreach (var error in result.Errors)
                {
                    /*As we're looping through the error, let us add that to the ModelState -
                     This validation error can be displayed in the register view
                     */
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            /*If the modelstate is not valid, we want to re-render the Register View - For that we pass the model to the View*/
            return View(model);
        }

        //-------------------------------------------------Login Actions------------------------------------------------------------------

     
        [HttpGet]
        /*HTTP Error 404.15 - Not Found will be thrown if the [AllowAnonymous] attribute isn't present.
         * What will happen is that the ReturnUrl will be long that the server won't be able to process it 
         * and will give up and throw this error. This Filer method in Program.cs is responsible for it
         
         builder.Services.AddControllersWithViews(options =>
            {
                /*Let build an Authorisation policy, so that we don't have to use [Authorize] attribute on the controller*/
        //var policy = new AuthorizationPolicyBuilder()
        //                         .RequireAuthenticatedUser()
        //                         .Build();
        ///*Add a filter*/
        //options.Filters.Add(new AuthorizeFilter(policy));
        //    }).AddXmlSerializerFormatters();
        // */

         [AllowAnonymous]
        /*When we click to Edit or Create, we were supposed to be redicted to these Actions in the HomeComntrollers. Since we've apply the [Authorize] attribute to these
         Actions, we are immediately asked to Login. The Login View is called and this url: "https://localhost:44349/Account/Login?ReturnUrl=%2Fhome%2Fedit%2F1" is presented to
         us. As we can see, this url contains the "...ReturnUrl..." So, we want to return the user to the Action that he's supposed to go to.
         For that, data binding helps us by doing the mapping- We're passing the value of the ReturnUrl in the parameter of the Login Action method.*/
        public IActionResult Login()
        {
            return View();
        }

        /*After we've filled in the View(form), this method sends a post request for us to Login the user.
         This method receives the LoginViewModel as a parameter of the Login method
         */
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            /*Check is model is valid*/
            if (ModelState.IsValid)
            {
                /*If the model is valid, we can signin the new user. To signin/out the user, we use the injected _signInManager
                 The PasswordSignInAsync() has 2 overloads. The one we use, take the Email as the user name, the Password,checks the 
                 persistancy of the session (session cookie or persistant cookie), lock the account on failure.
                 */

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                /*The result is of type SignInResult and it has a boolean property Succeded that let us 
                  check if the user is created succesfully*/
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl)) /*If returnUrl contains info, redirect to that url - else go to index of home*/
                    {
                        /*The Redirect(returnUrl is Ok but can cuase a serious security risk called Open Redirect Attack,
                         * meaning you might be ridereted to an unknown site - This can make your application vulnerable to
                         * Open Redirect Attacks since the application redirects to an URL thats specified in the request. 
                         * To avoid that, Let use: return LocalRedirect(returnUrl)  )*/

                        //return Redirect(returnUrl);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        /*The user has signed in, let him be redirected to the index page of the home controller */
                        return RedirectToAction("Index", "home");
                    }
                   
                }
                /*If there are error when siging in, collect them here*/
                 ModelState.AddModelError(string.Empty, "Invalid Login Attempt");                
            }
            /*If the modelstate is not good, we want to re-render the Login View - For that we pass the model to the View*/
            return View(model);
        }


        //--------------------------------------------------Logout Action------------------------------------------------------------------
        [HttpPost] /*Always logout a user using a post method. See the _Layout.cshtml for more info*/
        public async Task<IActionResult> Logout()
        {
            /*To sign in/out a user, we use the _signinManager injected service*/

            await _signInManager.SignOutAsync();
            //After the user has signed out, let redirect him to the index action of the HomeControllers
            return RedirectToAction("Index","home");
        }

        //------------------------------------------IsEmailInUse-------------------------------------------------------------------------
        /*An Action method to check that the email hasn't been used by another user*/
        [AcceptVerbs("Get","Post")] /*This method responds to both HttpGet and HttpPost*/
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                /*Nobody is using thie account/email*/
                /*We returning a JsonResult bcoz .Net MVC uses JQuerry validate methods to call the server side function
                 An Ajax call is issued to the IsEmailInUse() method and a Jquerry validator method expects a Json response
                from the IsEmailInUse() method. So that's why we use the JsonResult
                 */
                return Json(true);
            }
            else
            {
                return Json($"Email: {email} is already in use");
            }
        }
    }
}
