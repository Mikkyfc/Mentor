using Mentor.Models;
using Mentor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.IHelper
{
    public interface IUserHelper
    {
        Task<ApplicationUser> CreateUser(ApplicationUserViewModel applicationUserViewModel, string base64);
        bool DeactivateUser(string email);
        bool ReactivateUser(string email);
        ApplicationUser FindByUser(string userName);
        List<HostelViewModel> ListOfHostel();
        string CreateHostel(string name);
        string EditHostel(HostelViewModel hostelViewModel);
        string DeleteHostel(int id);
        List<ApplicationUserViewModel> ListOfUser();
        List<ApplicationUserViewModel> ListOfActiveUser();
        Room CreateRoom(RoomViewModel roomViewModel);
        Task<List<RoomViewModel>> ListOfRoom();
        ApplicationUser AllocateHostel(ApplicationUserViewModel applicationUserViewModel);
        string EditRoom(RoomViewModel roomViewModel);
        string DeleteRoom(int id);
        ApplicationUser GetUserByUserId(string id);
        ApplicationUser VacateHostel(string id);
        ApplicationUserViewModel GetUserToBeReasignedHostel(string userId);
        ApplicationUserViewModel GetCurrentLoginUser(ApplicationUser user);
        string UpdateUser(ApplicationUserViewModel applicationUserViewModel, ApplicationUser currentUser);
        string EditUserProfile(ApplicationUserViewModel applicationUserViewModel, ApplicationUser currentuser);
        string EditUserProfilePicture(string base64, string id);
        string GetValidateUrl(ApplicationUser user);
        GeneralViewModel GetAdminIndexDetails();
        Task<List<RoomViewModel>> GetListOfRoomWithHostelId(int hostelId);
        ApplicationUser ReassignHostel(ApplicationUserViewModel applicationUserViewModel);
        void FreeUserRoom(ApplicationUser user);
    }
}
