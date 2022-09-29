using Mentor.Db;
using Mentor.IHelper;
using Mentor.Models;
using Mentor.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Helper
{
    public class UserHelper : IUserHelper
    {
        private readonly AppDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public UserHelper(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<ApplicationUser> CreateUser(ApplicationUserViewModel applicationUserViewModel, string base64)
        {
            if (applicationUserViewModel != null)
            {

                var applicationUser = new ApplicationUser()
                {
                    UserName = applicationUserViewModel.Email,
                    FirstName = applicationUserViewModel.FirstName,
                    LastName = applicationUserViewModel.LastName,
                    Email = applicationUserViewModel.Email,
                    PhoneNumber = applicationUserViewModel.PhoneNumber,
                    GenderId = applicationUserViewModel.GenderId,
                    ProfilePicture = base64,
                    Deactivated = false,
                    isCompleted = false,
                    Password = applicationUserViewModel.Password,
                    ConfirmPassword = applicationUserViewModel.ConfirmPassword,
                };
                if (applicationUser.Email != null && applicationUser.Password != null)
                {
                    var createResult = _userManager.CreateAsync(applicationUser, applicationUserViewModel.Password).Result;
                    if (createResult.Succeeded)
                    {
                        return applicationUser;
                    }
                }
                return null;
            }
            return null;
        }
        
        public bool DeactivateUser(string email)
        {
            if (email != null)
            {
                var User = _context.ApplicationUsers.Where(x => x.Email == email && !x.Deactivated ).FirstOrDefault();
                if (User != null)
                {
                    User.Deactivated = true;
                    _context.Update(User);
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public bool ReactivateUser(string email)
        {
            if (email != null)
            {
                var User = _context.ApplicationUsers.Where(x => x.Email == email && x.Deactivated).FirstOrDefault();
                if (User != null)
                {
                    User.Deactivated = false;
                    _context.Update(User);
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }
        // method that check for the email if it's aleady existed
        public ApplicationUser FindByUser(string userName)
        {
            if (userName != null)
            {
                var user = _context.ApplicationUsers.Where(x => x.Email == userName).Include(x => x.Country).Include(x => x.State).Include(x => x.Hostel).Include(x => x.Room).Include(x => x.Department).Include(x => x.Gender).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }


        public ApplicationUser GetUserByUserId(string id)
        {
            if (id != null)
            {
                var user = _context.ApplicationUsers.Where(x => x.Id == id && !x.Deactivated).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }
         //method for creating the hostel 
        public string CreateHostel(string name)
        {
            if (name != null)
            {
                var hostel = new Hostel()
                {
                    Name = name,
                    Active = true,
                    Deleted = false,
                    DateCreated = DateTime.Now,
                };
                _context.Hostels.Add(hostel);
                _context.SaveChanges();
                return "Hostel Created Successfully";
            }
            return null;
        }
        // metthod for editing the hostel
        public string EditHostel(HostelViewModel hostelViewModel)
        {
            if (hostelViewModel != null)
            {
                var hostel = _context.Hostels.Where(x => x.Id == hostelViewModel.Id && !x.Deleted).FirstOrDefault();
                if (hostel != null)
                {
                    hostel.Name = hostelViewModel.Name;
                    _context.Hostels.Update(hostel);
                    _context.SaveChanges();
                    return "Hostel Successfully Updated";
                }
            }return "Hostel failed To Update";
        }
        public string DeleteHostel(int id)
        {
            if (id != 0)
            {
                var hostel = _context.Hostels.Where(x => x.Id == id && x.Active == true).FirstOrDefault();
                if (hostel != null)
                {
                    hostel.Active = false;
                    hostel.Deleted = true;
                    _context.Hostels.Update(hostel);
                    _context.SaveChanges();
                    return "Hostel Successfully Deleted";
                }
            }return "Hostel failed To Delete";
        }
        public List<HostelViewModel> ListOfHostel()
        {
            var listOfHostel = new List<HostelViewModel>();
            var hostels = _context.Hostels.Where(x => x.Id != 0 && !x.Deleted).ToList();
            if (hostels.Count > 0)
            {
                foreach (var hostel in hostels)
                {
                   var rooms = GetListOfRoomWithHostelId(hostel.Id).Result.Count();
                    var hostelViewModel = new HostelViewModel()
                    {
                        Rooms = rooms,
                        Name = hostel.Name,
                        Id = hostel.Id,
                        DateCreated = hostel.DateCreated.ToString("D"),
                    };
                    listOfHostel.Add(hostelViewModel);
                }
                return listOfHostel;
            }
            return listOfHostel;
        }

        public List<ApplicationUserViewModel> ListOfUser()
        {
            var listofUser = new List<ApplicationUserViewModel>();
            var users = _context.ApplicationUsers.Where(x => x.Id != null && x.Email != null).Include(x => x.Gender).Include(x => x.Department).Include(x => x.Country).Include(x => x.State).Include(x => x.Hostel).Include(x => x.Room).ToList();
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    var applicationUserViewModel = new ApplicationUserViewModel()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CountryId = user.CountryId,
                        StateId = user.StateId,
                        PhoneNumber = user.PhoneNumber,
                        ProfilePicture = user.ProfilePicture,
                        DepartmentId = user.DepartmentId,
                        HomeAddress = user.HomeAddress,
                        About = user.About,
                        Gender = user.Gender,
                        Id =  user.Id,
                        NextOfKin = user.NextOfKin,
                        Country = user.Country,
                        Department = user.Department,
                        NextOfKinPhoneNumber = user.NextOfKinPhoneNumber,
                        State = user.State,
                        Email = user.Email,
                        Hostel = user.Hostel,
                        Room = user.Room,
                        Deactivated = user.Deactivated,
                        isCompleted = user.isCompleted
                        
                    };
                    listofUser.Add(applicationUserViewModel);
                }
                return listofUser;
            }
            return listofUser;

            
        }

        public List<ApplicationUserViewModel> ListOfActiveUser()
        {
            var listofUser = new List<ApplicationUserViewModel>();
            var users = _context.ApplicationUsers.Where(x => x.Id != null && x.Email != null && !x.Deactivated).Include(x => x.Gender).Include(x => x.Department).Include(x => x.Country).Include(x => x.State).Include(x => x.Hostel).Include(x => x.Room).ToList();
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    var applicationUserViewModel = new ApplicationUserViewModel()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CountryId = user.CountryId,
                        StateId = user.StateId,
                        PhoneNumber = user.PhoneNumber,
                        ProfilePicture = user.ProfilePicture,
                        DepartmentId = user.DepartmentId,
                        HomeAddress = user.HomeAddress,
                        About = user.About,
                        Gender = user.Gender,
                        Id = user.Id,
                        NextOfKin = user.NextOfKin,
                        Country = user.Country,
                        Department = user.Department,
                        NextOfKinPhoneNumber = user.NextOfKinPhoneNumber,
                        State = user.State,
                        Email = user.Email,
                        Hostel = user.Hostel,
                        Room = user.Room,
                        Deactivated = user.Deactivated,
                        isCompleted = user.isCompleted
                    };
                    listofUser.Add(applicationUserViewModel);
                }
                return listofUser;
            }
            return listofUser;


        }

        public async Task<List<RoomViewModel>> ListOfRoom()
        {
            var listOfRoom = new List<RoomViewModel>();
            var rooms =await _context.Rooms.Where(x => x.Id != 0 && !x.Deleted && x.IsAvailable).Include(x => x.Hostel).ToListAsync();
            if (rooms.Count > 0)
            {
                foreach (var room in rooms)
                {
                    var roomViewModel = new RoomViewModel()
                    {
                        Name = room.Name,
                        Id = room.Id,
                        DateCreated = room.DateCreated.ToString("D"),
                        Hostel = room.Hostel,

                    };
                    listOfRoom.Add(roomViewModel);
                }
                return listOfRoom;
            }
            return listOfRoom;
        }

        public Room CreateRoom(RoomViewModel roomViewModel)
        {
            if (roomViewModel != null)
            {
                var hostel = _context.Hostels.Where(x => x.Id == roomViewModel.HostelId && x.Active).FirstOrDefault();
                if (hostel != null)
                {
                    var roomModel = new Room()
                    {
                        Name = roomViewModel.Name,
                        IsAvailable = true,
                        Deleted = false,
                        Active = true,
                        DateCreated = DateTime.Now,
                        HostelId = hostel.Id,
                        Hostel = hostel
                     };
                    _context.Rooms.Add(roomModel);
                    _context.SaveChanges();
                    return roomModel;
                }
            }
            return null;
        }

        public ApplicationUser AllocateHostel(ApplicationUserViewModel applicationUserViewModel)
        {
            if (applicationUserViewModel.RoomId != 0 && applicationUserViewModel.HostelId != 0)
            {
                var user = _context.ApplicationUsers.Where(x => x.Id == applicationUserViewModel.Id && !x.Deactivated).Include(x => x.Hostel).Include(x => x.Room).FirstOrDefault();
                if (user != null)
                {
                    user.HostelId = applicationUserViewModel.HostelId;
                    user.RoomId = applicationUserViewModel.RoomId;
                    _context.ApplicationUsers.Update(user);
                    var userRoom = _context.Rooms.Where(x => x.Id == applicationUserViewModel.RoomId && x.IsAvailable && x.Active).FirstOrDefault();
                    if (userRoom != null)
                    {
                        userRoom.IsAvailable = false;
                        userRoom.Active = true;
                        userRoom.Deleted = false;
                    }
                    _context.Rooms.Update(userRoom);
                    _context.SaveChanges();
                    return user;
                }
            }
            return null;
        }

        public ApplicationUser ReassignHostel(ApplicationUserViewModel applicationUserViewModel)
        {
            if (applicationUserViewModel.RoomId != 0 && applicationUserViewModel.HostelId != 0)
            {
                var user = _context.ApplicationUsers.Where(x => x.Id == applicationUserViewModel.Id && !x.Deactivated).Include(x => x.Hostel).Include(x => x.Room).FirstOrDefault();
                if (user != null)
                {
                    FreeUserRoom(user);
                    user.HostelId = applicationUserViewModel.HostelId;
                    user.RoomId = applicationUserViewModel.RoomId;
                    _context.ApplicationUsers.Update(user);
                    var userRoom = _context.Rooms.Where(x => x.Id == applicationUserViewModel.RoomId && x.Active).FirstOrDefault();
                    if (userRoom != null)
                    {
                        userRoom.IsAvailable = false;
                        userRoom.Active = true;
                        userRoom.Deleted = false;
                    }
                    _context.Rooms.Update(userRoom);
                    _context.SaveChanges();
                    return user;
                }
            }
            return null;
        }
        // metthod for editing the Room
        public string EditRoom(RoomViewModel roomViewModel)
        {
            if (roomViewModel != null)
            {
                var room = _context.Rooms.Where(x => x.Id == roomViewModel.Id && x.Deleted == false).FirstOrDefault();
                if (room != null)
                {
                    room.Name = roomViewModel.Name;
                    _context.Rooms.Update(room);
                    _context.SaveChanges();
                    return "Room Successfully Updated";
                }
            }
            return "Room failed To Update";
        }

        public string DeleteRoom(int id)
        {
            if (id != 0)
            {
                var room = _context.Rooms.Where(x => x.Id == id && x.Active == true && x.IsAvailable).FirstOrDefault();
                if (room != null)
                {
                    room.Active = false;
                    room.Deleted = true;
                    room.IsAvailable = false;
                    _context.Rooms.Update(room);
                    _context.SaveChanges();
                    return "Room Successfully Deleted";
                }
            }
            return "Room failed To Delete";
        }

        public ApplicationUser VacateHostel(string id)
        {
            var currrentUser = GetUserByUserId(id);
            if (currrentUser != null)
            {
                currrentUser.HostelId = null;
                currrentUser.RoomId = null;
                _context.ApplicationUsers.Update(currrentUser);
                _context.SaveChanges();
                return currrentUser;
            }
            return null;   
        }

        public ApplicationUserViewModel GetUserToBeReasignedHostel(string userId)
        {
            var user = _context.ApplicationUsers.Where(x => x.Id == userId && !x.Deactivated).Include(x => x.Hostel).Include(x => x.Room).Include(x => x.Country).Include(x => x.State).Include(x => x.Department).Include(x => x.Gender).FirstOrDefault();
            if (user != null)
            {
                var userDetails = new ApplicationUserViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CountryId = user.CountryId,
                    StateId = user.StateId,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture,
                    DepartmentId = user.DepartmentId,
                    HomeAddress = user.HomeAddress,
                    About = user.About,
                    Gender = user.Gender,
                    Id = user.Id,
                    NextOfKin = user.NextOfKin,
                    Country = user.Country,
                    Department = user.Department,
                    NextOfKinPhoneNumber = user.NextOfKinPhoneNumber,
                    State = user.State,
                    Email = user.Email,
                    HostelId = user.HostelId,
                    RoomId = user.RoomId,
                    Hostel = user.Hostel,
                    Room = user.Room,
                    Deactivated = user.Deactivated,
                    isCompleted = user.isCompleted
                };
                return userDetails;
            }
            return null;
        }

        public ApplicationUserViewModel GetCurrentLoginUser(ApplicationUser user)
        {
            if (user != null)
            {
                var currentLoginUser = new ApplicationUserViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CountryId = user.CountryId,
                    StateId = user.StateId,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture,
                    DepartmentId = user.DepartmentId,
                    HomeAddress = user.HomeAddress,
                    About = user.About,
                    Gender = user.Gender,
                    Id = user.Id,
                    NextOfKin = user.NextOfKin,
                    Country = user.Country,
                    Department = user.Department,
                    NextOfKinPhoneNumber = user.NextOfKinPhoneNumber,
                    State = user.State,
                    Email = user.Email,
                    Hostel = user.Hostel,
                    Room = user.Room,
                    Deactivated = user.Deactivated,
                    isCompleted = user.isCompleted
                };
                return currentLoginUser;
            }
            return null;
        }

        public string UpdateUser(ApplicationUserViewModel applicationUserViewModel, ApplicationUser currentUser)
        {
            if (applicationUserViewModel != null)
            {
                if (currentUser != null)
                {
                    currentUser.CountryId = applicationUserViewModel.CountryId;
                    currentUser.DepartmentId = applicationUserViewModel.DepartmentId;
                    currentUser.StateId = applicationUserViewModel.StateId;
                    currentUser.HomeAddress = applicationUserViewModel.HomeAddress;
                    currentUser.About = applicationUserViewModel.About;
                    currentUser.NextOfKin = applicationUserViewModel.NextOfKin;
                    currentUser.NextOfKinPhoneNumber = applicationUserViewModel.NextOfKinPhoneNumber;
                    currentUser.isCompleted = true;
                    _context.Update(currentUser);
                    _context.SaveChanges();
                    return "Registration Completed Successfully";
                }
            }
            return "Error Occured";
        }
        

        public string EditUserProfile( ApplicationUserViewModel applicationUserViewModel, ApplicationUser currentuser)
        {
            if (applicationUserViewModel != null)
            {
                if (currentuser != null)
                {
                    currentuser.FirstName = applicationUserViewModel.FirstName;
                    currentuser.LastName = applicationUserViewModel.LastName;
                    currentuser.PhoneNumber = applicationUserViewModel.PhoneNumber;
                    currentuser.Email = applicationUserViewModel.Email;
                    currentuser.About = applicationUserViewModel.About;
                    currentuser.CountryId = applicationUserViewModel.CountryId;
                    currentuser.StateId = applicationUserViewModel.StateId;
                    currentuser.isCompleted = true;
                    _context.Update(currentuser);
                    _context.SaveChanges();
                    return "User profile Edited Successfull";
                }
            }
            return "Error occured";
        }

        public string EditUserProfilePicture(string base64 , string id)
        {
            if (base64 != null && id != null)
            {
                var userPicture = GetUserByUserId(id);
                if (userPicture != null)
                {
                    userPicture.ProfilePicture = base64;
                    _context.Update(userPicture);
                    _context.SaveChanges();
                    return "User profile Picture Updated";
                }
               

            }
            return "Error occured";
        }

        public string GetValidateUrl(ApplicationUser user)
        {
            var userROle = _userManager.IsInRoleAsync(user, "Admin").Result;
            if (userROle)
            {
                return "/Admin/Index";
            }
            else
            {
                return "/User/Index";
            }
        }

        public GeneralViewModel GetAdminIndexDetails()
        {
            var generalViewModel = new GeneralViewModel();
            generalViewModel.ApplicationUserViewModels = ListOfUser();
            generalViewModel.HostelViewModels = ListOfHostel();
            generalViewModel.RoomViewModels = ListOfRoom().Result;
            return generalViewModel;
        }
        public async Task<List<RoomViewModel>> GetListOfRoomWithHostelId(int hostelId)
        {
            var listOfRoom = new List<RoomViewModel>();
            var rooms = await _context.Rooms.Where(x => x.Id != 0 && !x.Deleted && x.IsAvailable && x.HostelId == hostelId).Include(x => x.Hostel).ToListAsync();
            if (rooms.Count > 0)
            {
                foreach (var room in rooms)
                {
                    var roomViewModel = new RoomViewModel()
                    {
                        Name = room.Name,
                        Id = room.Id,
                        DateCreated = room.DateCreated.ToString("D"),
                        Hostel = room.Hostel,

                    };
                    listOfRoom.Add(roomViewModel);
                }
                return listOfRoom;
            }
            return listOfRoom;
        }
        public void FreeUserRoom(ApplicationUser user)
        {
            var userCurrentRoom = _context.Rooms.Where(x => x.Id == user.RoomId && x.Active).FirstOrDefault();
            userCurrentRoom.IsAvailable = true;
            userCurrentRoom.Active = true;
            userCurrentRoom.Deleted = false;
            _context.Rooms.Update(userCurrentRoom);
            _context.SaveChanges();
            
        }
    }
}
