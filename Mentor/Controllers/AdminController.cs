using Mentor.Db;
using Mentor.Enum;
using Mentor.IHelper;
using Mentor.Models;
using Mentor.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserHelper _userHelper;
        private IDropdownHelper _dropdownHelper;
        public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager, IUserHelper userHelper, IDropdownHelper dropdownHelper, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userHelper = userHelper;
            _dropdownHelper = dropdownHelper;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            var details = _userHelper.GetAdminIndexDetails();
            if (details != null)
            {
                return View(details);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CompleteAdminForm()
        {
            ViewBag.Department = _dropdownHelper.GetDropdownsByKey(DropdownEnums.Department).Result;
            ViewBag.Country = _dropdownHelper.GetCountries().Result;
            ViewBag.State = _dropdownHelper.GetStates().Result;
            return View();
        }


        [HttpPost]
        public JsonResult CompleteAdminForm(string user)
        {
            if (user != null)
            {
                var loddegInUser = _userHelper.FindByUser(User.Identity.Name);
                if (loddegInUser == null)
                {
                    return Json(new { isError = false, msg = "Admin Registration To Be Completed In Progress" });
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
                        return Json(new { isError = false, id = loddegInUser.Id, msg = "Admin Registration Completed Successfully" });
                    }
                    return Json(new { isError = true, msg = "Admin Unable To Complete Registration" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpGet]
        public IActionResult Hostels()
        {
            var hostels = _userHelper.ListOfHostel();
            if (hostels.Count > 0)
            {
                return View(hostels);
            }
            return View(hostels);
        }

        [HttpPost]
        public JsonResult AddHostel(string hostelName)
        {
            if (hostelName != null)
            {
                var checkExistingHostelName = _context.Hostels.Where(x => x.Name == hostelName && !x.Deleted).FirstOrDefault();
                if (checkExistingHostelName != null)
                {
                    return Json(new { isError = true, msg = "Hostel Name Already Exist" });
                }
                var hostels = _userHelper.CreateHostel(hostelName);
                if (hostels != null)
                {
                    return Json(new { isError = false, msg = "Hostel created Successfully" });
                }
                return Json(new { isError = true, msg = "Unable To create Hostel" });
            }
            return Json(new { isError = true, msg = "Please enter Hostel Name" });
        }

        [HttpGet]
        public JsonResult EditHostel(int id)
        {
            if (id != 0)
            {
                var hostel = _context.Hostels.Where(x => x.Id == id && !x.Deleted).FirstOrDefault();
                if (hostel != null)
                {
                    return Json(hostel);
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error Occured" });
        }

        [HttpPost]
        public JsonResult EditHostel(string hostelDetails)
        {
            if (hostelDetails != null)
            {
                var hostelViewModel = JsonConvert.DeserializeObject<HostelViewModel>(hostelDetails);
                if (hostelViewModel != null)
                {
                    var hostel = _userHelper.EditHostel(hostelViewModel);
                    if (hostel != null)
                    {
                        return Json(new { isError = false, msg = "Hostel Edited Successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable To Edit Hostel" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error Occured" });
        }


        [HttpPost]
        public JsonResult DeleteHostel(int id)
        {
            if (id != 0)
            {
                var hostel = _userHelper.DeleteHostel(id);
                if (hostel != null)
                {
                    return Json(new { isError = false, msg = "Hostel Deleted Successfully" });
                }
                return Json(new { isError = true, msg = "Unable To Delete Hostel" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }
        
        [HttpGet]
        public IActionResult AllUser()
        {
            ViewBag.Hostels = _dropdownHelper.GetHostels().Result;
            var users = _userHelper.ListOfUser();
            if (users.Count > 0)
            {
                return View(users);
            }
            return View(users);
        }

        [HttpPost]
        public JsonResult AddRoom(string roomViewModel)
        {
            if (roomViewModel != null)
            {
                var roomDetails = JsonConvert.DeserializeObject<RoomViewModel>(roomViewModel);
                if (roomDetails != null)
                {
                    var hostelRoom = _context.Hostels.Where(x => x.Id == roomDetails.HostelId && !x.Deleted).FirstOrDefault();
                    var checkExistingRoomName = _context.Rooms.Where(x => x.Name == roomDetails.Name && !x.Deleted && x.HostelId == hostelRoom.Id).Include(x => x.Hostel).FirstOrDefault();
                    if (checkExistingRoomName != null)
                    {
                        return Json(new { isError = true, msg = "Room Name Already Exist" });
                    }
                    var room = _userHelper.CreateRoom(roomDetails);
                    if (room != null)
                    {
                        return Json(new { isError = false, msg = "Room created Successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable To create Room" });
                }
               
            }
            return Json(new { isError = true, msg = "Please enter Hostel Name" });
        }

        [HttpGet]
        public IActionResult Rooms()
        {
            ViewBag.Hostel = _dropdownHelper.GetHostels().Result;
            var rooms  = _userHelper.ListOfRoom().Result;
            if (rooms.Count > 0)
            {
                return View(rooms);
            }
            return View(rooms);
        }

        [HttpPost]
        public JsonResult AllocateHostels(string hostelDetails)
        {
            if (hostelDetails != null)
            {
                var hostelDetail = JsonConvert.DeserializeObject<ApplicationUserViewModel>(hostelDetails);
                if (hostelDetail != null)
                {
                    var hostel = _userHelper.AllocateHostel(hostelDetail);
                    if (hostel != null)
                    {
                        return Json(new { isError = false, msg = "Hostel Allocated  Successfully" });
                    }
                    return Json(new { isError = true, msg = "Please fill all field" });
                }
            }
            return Json(new { isError = true, msg = "Please enter Hostel Name" });
        }

        [HttpGet]
        public JsonResult GetRoom(int hostelId)
        {
            var rooms = new List<Room>();
            var listOfRooms =  _context.Rooms.Where(a => a.Id != 0 && a.HostelId == hostelId && !a.Deleted && a.IsAvailable).Include(x => x.Hostel).OrderBy(m => m.Name).ToList();
            if (listOfRooms.Count > 0)
            {
                return Json(new SelectList(listOfRooms, "Id", "Name"));
            }
            return Json(rooms);
            
        }

        [HttpGet]
        public JsonResult EditRoom(int id)
        {
            if (id != 0)
            {
                var room = _context.Rooms.Where(x => x.Id == id && !x.Deleted && x.IsAvailable).FirstOrDefault();
                if (room != null)
                {
                    return Json(room);
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error Occured" });
        }

        [HttpPost]
        public JsonResult EditRoom(string roomDetails)
        {
            if (roomDetails != null)
            {
                var roomViewModel = JsonConvert.DeserializeObject<RoomViewModel>(roomDetails);
                if (roomViewModel != null)
                {
                    var room = _userHelper.EditRoom(roomViewModel);
                    if (room != null)
                    {
                        return Json(new { isError = false, msg = "Room Edited Successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable To Edit Room" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error Occured" });
        }

        [HttpPost]
        public JsonResult DeleteRoom(int id)
        {
            if (id != 0)
            {
                var room = _userHelper.DeleteRoom(id);
                if (room != null)
                {
                    return Json(new { isError = false, msg = "Room Deleted Successfully" });
                }
                return Json(new { isError = true, msg = "Unable To Delete Room" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult VacateHostel(string id)
        {
            if (id != null)
            {
                var user = _userHelper.VacateHostel(id);
                if (user != null)
                {
                    return Json(new { isError = false, msg = "User Vacated Successfully" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error Occured" });
        }

       

        [HttpPost]
        public JsonResult DeactivateUser(string id)
        {
            if (id != null)
            {
                var user = _userHelper.GetUserByUserId(id);
                if (user != null)
                {
                    var deactivateUser =  _userHelper.DeactivateUser(user.Email);
                    if (deactivateUser)
                    {
                        return Json(new { isError = false, msg = "User Deactivated  Successfully" });
                    }
                    else
                    {
                        return Json(new { isError = true, msg = "Unable To Deactivate User" });
                    }
                }
                return Json(new { isError = true, msg = "Unable To get User" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult ReactivateUser(string id)
        {
            if (id != null)
            {
                var user = _context.ApplicationUsers.Where(a => a.Id == id && a.Deactivated).FirstOrDefault();
                if (user != null)
                {
                    var reactivateUser = _userHelper.ReactivateUser(user.Email);
                    if (reactivateUser)
                    {
                        return Json(new { isError = false, msg = "User Reactivated  Successfully" });
                    }
                    else
                    {
                        return Json(new { isError = true, msg = "Unable To Reactivate User" });
                    }
                }
                return Json(new { isError = true, msg = "Unable To get User" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpGet]
        public IActionResult HostelSettings()
        {
            ViewBag.Hostels = _dropdownHelper.GetHostels().Result;
            var users = _userHelper.ListOfActiveUser();
            if (users.Count > 0)
            {
                return View(users);
            }
            return View(users);
        }

        [HttpGet]
        public JsonResult GetUserToBeReasignedHostel(string userId)
        {
            ViewBag.Hostels = _dropdownHelper.GetHostels().Result;
            if (userId != null)
            {
                var user = _userHelper.GetUserToBeReasignedHostel(userId);
                if (user != null)
                {
                    var hostels = ViewBag.Hostels;
                    var rooms = _dropdownHelper.GetRoomsByHostelId(user.Hostel?.Id).Result;
                    return Json(new { currentUser = user, allHostels = hostels, allRooms = rooms });
                }
                return Json(new { isError = true, msg = "User To Be Reasigned not through" });
            }
            return Json(new { isError = true, msg = "Please enter Hostel Name" });
        }

        [HttpPost]
        public JsonResult ReasignUser(string userDetails)
        {
            if (userDetails != null)
            {
                var userViewModel = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);
                if (userViewModel != null)
                {
                    var user = _userHelper.ReassignHostel(userViewModel);
                    if (user != null)
                    {
                        return Json(new { isError = false, msg = "Hostel Reasigned To User Successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable To Reasigned User To Hostel" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        public IActionResult AdminProfile(string userId)
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


        [HttpGet]
        public JsonResult EditAdminProfile(string userId)
        {
            if (userId != null)
            {
                var user = _userHelper.GetUserToBeReasignedHostel(userId);
                if (user != null)
                {
                    return Json(user);
                }
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult EditAdminProfile(string userProfileDetails, string id)
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
                        return Json(new { isError = false, msg = "Admin Profile Edited Successfully", userId = currentLoggedInUser.Id });
                    }
                    return Json(new { isError = true, msg = "Unable To Edit Admin Profile" });
                }
                return Json(new { isError = true, msg = "Error occured" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult EditAdminProfilePicture(string id, string base64)
        {
            if (base64 != null && id != null)
            {
                var uploadUserPicture = _userHelper.EditUserProfilePicture(base64, id);
                if (uploadUserPicture != null)
                {
                    return Json(new { isError = false, msg = "Admin Profile Picture Edited Successfully", UserId = id });
                }
                return Json(new { isError = true, msg = "Unable To Edit Admin Profile Picture" });
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpPost]
        public JsonResult ChangeAdminsPassword(string userProfileDetails, string id)
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
                            return Json(new { isError = true, msg = "Please Password and confirm password must match" });
                        }
                        var result = _userManager.ChangePasswordAsync(currentUser, userDetails.Password, userDetails.NewPassword).Result;
                        if (result.Succeeded)
                        {
                            return Json(new { isError = false, msg = "Admin Password change Successfully", userId = currentUser.Id });
                        }
                        return Json(new { isError = false, msg = "Unable to change Admin Password", userId = currentUser.Id });
                    }
                    return Json(new { isError = true, msg = "Error occured" });
                }
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

    }

    

}
