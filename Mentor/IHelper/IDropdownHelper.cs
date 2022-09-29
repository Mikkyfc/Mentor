using Mentor.Enum;
using Mentor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.IHelper
{
  public  interface IDropdownHelper
    {
        Task<List<Country>> GetCountries();
        Task<List<State>> GetStates();
        Task<List<CommonDropdown>> GetDropdownsByKey(DropdownEnums dropdownEnums);
        Task<List<Hostel>> GetHostels();
        Task<List<Room>> GetRoomsByHostelId(int? hostelId);
        Task<List<State>> GetStatesByCountryId(int? countryId);
       
    }
}
