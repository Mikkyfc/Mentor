using Mentor.Db;
using Mentor.Enum;
using Mentor.Helper;
using Mentor.IHelper;
using Mentor.Models;
using Mentor.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Controllers
{
    public class AccountController : Controller
    {


        private readonly AppDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserHelper _userHelper;
        private IDropdownHelper _dropdownHelper;
        public AccountController(AppDbContext context, UserManager<ApplicationUser> userManager, IUserHelper userHelper, IDropdownHelper dropdownHelper, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userHelper = userHelper;
            _dropdownHelper = dropdownHelper;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet]
        public ActionResult UserRegisteration()
        {
            ViewBag.Gender = _dropdownHelper.GetDropdownsByKey(DropdownEnums.Gender).Result;
            return View();
        }

        [HttpPost]
        public JsonResult UserRegisteration(string userDetails, string base64)
        {
            if (userDetails != null && base64 != null)
            {
                var applicationUserDetails = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);
                if (applicationUserDetails != null)
                {
                    if (applicationUserDetails.FirstName == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your First Name!!!" });
                    }
                    if (applicationUserDetails.LastName == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Last Name!!!" });
                    }
                    if (applicationUserDetails.Email == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Email!!!" });
                    }
                    if (applicationUserDetails.PhoneNumber == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Phone Number!!!" });
                    }

                    if (applicationUserDetails.GenderId == null && applicationUserDetails.GenderId == 0)
                    {
                        return Json(new { isError = true, msg = "Please Select Your Gender!!!" });
                    }


                    if (applicationUserDetails.Password != applicationUserDetails.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Please Password and confirm password must match" });
                    }
                    var checkExistingUser = _userHelper.FindByUser(applicationUserDetails.Email);
                    if (checkExistingUser != null)
                    {
                        return Json(new { isError = true, msg = "Email already exist" });
                    }
                    var userRegistration = _userHelper.CreateUser(applicationUserDetails, base64).Result;
                    if (userRegistration != null)
                    {
                        var userRole = _userManager.AddToRoleAsync(userRegistration, "User").Result;
                        if (userRole.Succeeded)
                        {
                            return Json(new { isError = false, msg = "Registeration Successful" });
                        }
                        return Json(new { isError = true, msg = "Unable To Asign User Role" });
                    }
                    return Json(new { isError = true, msg = "Unable to Register the User" });
                }
            }
            return Json(new { isError = true, msg = "Unable To Fetch Details" });
        }

        [HttpPost]
        public JsonResult DeactivateUser(string userName)
        {
            if (userName != null)
            {
                var deleteUser = _userHelper.DeactivateUser(userName);
                if (deleteUser)
                {
                    return Json(new { isError = false, msg = "User Deactivated Successfully" });
                }
                return Json(new { isError = false, msg = "User Deactivation Failed" });
            }
            return Json(new { isError = false, msg = "Unable to fetch user" });
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(string userDetails)
        {
            if (userDetails != null)
            {
                var applicationUserDetails = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);
                if (applicationUserDetails != null)
                {

                    var user = _userHelper.FindByUser(applicationUserDetails.Email);
                    if (user != null)
                    {
                        if (user.Deactivated)
                        {
                            return Json(new { isError = true, msg = "User has be Deactivated please Kindly Contact Your Admin" });
                        }
                        var login = _signInManager.CheckPasswordSignInAsync(user, applicationUserDetails.Password, false).Result;
                        if (login.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, false);
                            var dashboard = _userHelper.GetValidateUrl(user);
                            return Json(new { isError = false, dashboard = dashboard });
                        }
                    }
                    return Json(new { isError = true, msg = "Invalid Email" });
                }
                return null;
            }
            return null;
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult AdminRegisteration()
        {
            ViewBag.Gender = _dropdownHelper.GetDropdownsByKey(DropdownEnums.Gender).Result;
            return View();
        }



        [HttpPost]
        public JsonResult AdminRegisteration(string userDetails, string base64)
        {
            if (userDetails != null && base64 != null)
            {
                var applicationUserDetails = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);
                if (applicationUserDetails != null)
                {
                    if (applicationUserDetails.FirstName == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your First Name!!!" });
                    }
                    if (applicationUserDetails.LastName == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Last Name!!!" });
                    }
                    if (applicationUserDetails.Email == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Email!!!" });
                    }
                    if (applicationUserDetails.PhoneNumber == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Phone Number!!!" });
                    }

                    if (applicationUserDetails.GenderId == null && applicationUserDetails.GenderId == 0)
                    {
                        return Json(new { isError = true, msg = "Please Select Your Gender!!!" });
                    }
                    if (applicationUserDetails.Password != applicationUserDetails.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Please Password and confirm password must match" });
                    }
                    var checkExistingUser = _userHelper.FindByUser(applicationUserDetails.Email);
                    if (checkExistingUser != null)
                    {
                        return Json(new { isError = true, msg = "Email already exist" });
                    }
                    var userRegistration = _userHelper.CreateUser(applicationUserDetails, base64).Result;
                    if (userRegistration != null)
                    {
                        var userRole = _userManager.AddToRoleAsync(userRegistration, "Admin").Result;
                        if (userRole.Succeeded)
                        {
                            return Json(new { isError = false, msg = "Admin Registeration Successful" });
                        }
                        return Json(new { isError = true, msg = "Unable To Asign User Role" });
                    }
                    return Json(new { isError = true, msg = "Unable to Register the User" });
                }
            }
            return Json(new { isError = true, msg = "Unable To Fetch Details" });
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        public JsonResult VerifyUserToChangingPswd(string userName)
        {
            if (userName != null)
            {
                var user = _userHelper.FindByUser(userName);
                if (user != null)
                {
                    return Json(new { isError = false, data = user });
                }
                else
                {
                    return Json(new { isError = true, msg = "Your Details could not be found ", });
                }
            }
            return Json(new { isError = false, msg = " Email does not exist" });
        }

        [HttpPost]
        public JsonResult ChangeUserPassword(string applicationUserViewModel)
        {
            if (applicationUserViewModel != null)
            {
                
                 var userDetails = JsonConvert.DeserializeObject<ApplicationUserViewModel>(applicationUserViewModel);
                if (userDetails != null)
                {
                    if (userDetails.NewPassword != userDetails.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Please Password and confirm password must match" });
                    }
                    var user = _userHelper.FindByUser(userDetails.Email);
                    if (user != null)
                    {
                        var removePassword = _userManager.RemovePasswordAsync(user).Result;
                        if (removePassword.Succeeded)
                        {
                            var changePassword = _userManager.AddPasswordAsync(user, userDetails.NewPassword).Result;
                            if (changePassword.Succeeded)
                            {
                                _context.SaveChanges();
                                return Json(new { isError = false, msg = "user Password Reset Successfully" });
                            }
                        }
                        return Json(new { isError = true, msg = "Error occured" });

                    }
                    return Json(new { isError = true, msg = "The Email You entered is Invalid" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error occured" });

        }
    }
}
       


