using Mentor.Db;
using Mentor.Enum;
using Mentor.IHelper;
using Mentor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Helper
{
    public class DropdownHelper : IDropdownHelper
    {
        private readonly AppDbContext _context;

        public DropdownHelper(AppDbContext context)
        {
            _context = context;
        }

        

        public async Task<List<Country>> GetCountries()
        {
            try
            {
                var common = new Country()
                {
                    Id = 0,
                    Name = "Select Country"
                };
                var listOfCountries = await _context.Countries.Where(x => x.Id != 0 && !x.Deleted).OrderBy(p => p.Id).ToListAsync();
                listOfCountries.Insert(0, common);
                return listOfCountries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<State>> GetStates()
        {
            try
            {
                var common = new State()
                {
                    Id = 0,
                    Name = "Select State"
                };
                var listOfStates = await _context.States.Where(a => a.Id != 0 && !a.Deleted).OrderBy(m => m.Id).ToListAsync();
                listOfStates.Insert(0, common);
                return listOfStates;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class DropdownEnum
        {
            public int Id { get; set; }
            public string Name { get; set; }
           
        }
        public async Task<List<CommonDropdown>> GetDropdownsByKey(DropdownEnums dropdownEnums)
        {
            try
            {
                var common = new CommonDropdown()
                {
                    Id = 0,
                    Name = "Select",
                };
                var listOfDropdownEnums = await _context.CommonDropdowns.Where(a => !a.Deleted && a.DropdownKey == (int)dropdownEnums).OrderBy(a => a.Name).ToListAsync();
                listOfDropdownEnums.Insert(0, common);
                return listOfDropdownEnums;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<List<Hostel>> GetHostels()
        {
            try
            {
                var common = new Hostel()
                {
                    Id = 0,
                    Name = "Select Hostel"
                };
                var listOfHostels = await _context.Hostels.Where(a => a.Id != 0 && !a.Deleted).OrderBy(m => m.Id).ToListAsync();

                listOfHostels.Insert(0, common);
                return listOfHostels;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Room>> GetRoomsByHostelId(int? hostelId)
        {
            try
            {
                var rooms = new List<Room>();
                var listOfRooms = await _context.Rooms.Where(a => a.Id != 0 && a.HostelId == hostelId && !a.Deleted).Include(x => x.Hostel).OrderBy(m => m.Name).ToListAsync();
                if (listOfRooms.Any())
                {
                    return listOfRooms;
                }
                return rooms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<State>> GetStatesByCountryId(int? countryId)
        {
            try
            {
                var states = new List<State>();
                var listOfStates = await _context.States.Where(a => a.Id != 0 && a.CountryId == countryId && !a.Deleted).Include(a => a.Country).OrderBy(m => m.Name).ToListAsync();
                if (listOfStates != null)
                {
                    return listOfStates;
                }
                return states;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
