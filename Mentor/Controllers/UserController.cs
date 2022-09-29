using Mentor.Db;
using Mentor.Enum;
using Mentor.IHelper;
using Mentor.Models;
using Mentor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserHelper _userHelper;
        private IDropdownHelper _dropdownHelper;
        public UserController(AppDbContext context, UserManager<ApplicationUser> userManager, IUserHelper userHelper, IDropdownHelper dropdownHelper, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userHelper = userHelper;
            _dropdownHelper = dropdownHelper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var logedInUser = _userHelper.FindByUser(User.Identity.Name);
            if (logedInUser != null)
            {
                var currentUser = _userHelper.GetCurrentLoginUser(logedInUser);
                if (currentUser != null)
                {
                    return View(currentUser);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult CompleteRegistration()
        {
            ViewBag.Department = _dropdownHelper.GetDropdownsByKey(DropdownEnums.Department).Result;
            ViewBag.Country = _dropdownHelper.GetCountries().Result;
            ViewBag.State = _dropdownHelper.GetStates().Result;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetState(int countryId)
        {
            var states = _dropdownHelper.GetStatesByCountryId(countryId).Result;
            if (states.Count > 0)
            {
                return Json(new SelectList(states, "Id", "Name"));
            }
            return Json(states);
        }
        [HttpGet]
        public IActionResult UserProfile(string userId)
        {
            if (userId != null)
            {
                var user = _userHelper.GetUserToBeReasignedHostel(userId);
                if (user != null)
                {
                    return View(user);
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult CompleteRegistration(string user)
        {
            if (user != null)
            {
                var loddegInUser = _userHelper.FindByUser(User.Identity.Name);
                if (loddegInUser == null)
                {
                    return Json(new { isError = false, msg = "User Registration To Be Completed In Progress" });
                }
                var userDetails = JsonConvert.DeserializeObject<ApplicationUserViewModel>(user);
                if (userDetails != null)
                {
                    if (userDetails.NextOfKin == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your NextOfKin Name!!!" });
                    }
                    if (userDetails.NextOfKinPhoneNumber == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your NextOfKin Phone Number !!!" });
                    }
                    if (userDetails.HomeAddress == null)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your HomeAddress!!!" });
                    }
                    if (userDetails.DepartmentId == null && userDetails.DepartmentId == 0)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Department!!!" });
                    }
                    if (userDetails.CountryId == null && userDetails.CountryId == 0)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your Country!!!" });
                    }
                    if (userDetails.StateId == null && userDetails.StateId == 0)
                    {
                        return Json(new { isError = true, msg = "Please Enter Your State!!!" });
                    }

                    var updateUser = _userHelper.UpdateUser(userDetails, loddegInUser);
                    if (updateUser != null)
                    {
                        return Json(new { isError = false, id = loddegInUser.Id, msg = "User Registration Completed Successfully" });
                    }
                    return Json(new { isError = true, msg = "User Unable To Complete Registration" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpGet]
        public JsonResult EditUserProfile(string userId)
        {
            ViewBag.Country = _dropdownHelper.GetCountries().Result;
            if (userId != null)
            {
                var user = _userHelper.GetUserToBeReasignedHostel(userId);
                if (user != null)
                {
                    var country = ViewBag.Country;
                    var state = _dropdownHelper.GetStatesByCountryId(user.Country?.Id).Result;
                    return Json(new { currentUser = user, allCountries = country, allState = state });

                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult EditUserProfile(string userProfileDetails, string id)
        {
            if (userProfileDetails != null && id != null)
            {
                var currentLoggedInUser = _userHelper.GetUserByUserId(id);
                var editedUser = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userProfileDetails);
                if (editedUser != null)
                {
                    var userDetails = _userHelper.EditUserProfile(editedUser, currentLoggedInUser);
                    if (userDetails != null)
                    {
                        return Json(new { isError = false, msg = "User Profile Edited Successfully", userId = currentLoggedInUser.Id });
                    }
                    return Json(new { isError = true, msg = "Unable To Edit User Profile" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult EditUserProfilePicture(string id, string base64)
        {
            if (base64 != null && id != null)
            {
                var uploadUserPicture = _userHelper.EditUserProfilePicture(base64, id);
                if (uploadUserPicture != null)
                {
                    return Json(new { isError = false, msg = "User Profile Edited Successfully", UserId = id });
                }
                return Json(new { isError = true, msg = "Unable To Edit User Profile Picture" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult ChangeUsersPassword(string userProfileDetails, string id)
        {
            if (userProfileDetails != null && id != null)
            {
                var currentUser = _userHelper.GetUserByUserId(id);
                if (currentUser != null)
                {
                    var userDetails = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userProfileDetails);
                    if (userDetails != null)
                    {
                        if (currentUser.Password != userDetails.Password)
                        {
                            return Json(new { isError = true, msg = "Please enter the old password correctly" });
                        }
                        if (userDetails.NewPassword != userDetails.ConfirmPassword)
                        {
                            return Json(new { isError = true, msg = "Password and confirm password must match" });
                        }
                        var result = _userManager.ChangePasswordAsync(currentUser, userDetails.Password, userDetails.NewPassword).Result;
                        if (result.Succeeded)
                        {
                            return Json(new { isError = false, msg = "User Password change Successfully", userId = currentUser.Id });
                        }
                        return Json(new { isError = false, msg = "Unable to change User Password", userId = currentUser.Id });
                    }
                    return Json(new { isError = true, msg = "Error occured" });
                }
            }
            return Json(new { isError = true, msg = "Error occured" });
        }
    }
}
