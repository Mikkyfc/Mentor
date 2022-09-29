using Mentor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.ViewModel
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
         
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string About { get; set; }
        public string NextOfKin { get; set; }
        [Phone]
        public string NextOfKinPhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool Deactivated { get; set; }
        public string NewPassword { get; set; }
        public int? GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual CommonDropdown Gender { get; set; }
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual CommonDropdown Department { get; set; }
        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        public int? StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        public int? HostelId { get; set; }
        [ForeignKey("HostelId")]
        public virtual Hostel Hostel { get; set; }
        public int? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public DateTime DateCreated { get; set; }
        public bool isCompleted { get; set; }
    }
}
