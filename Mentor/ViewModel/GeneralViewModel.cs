using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.ViewModel
{
    public class GeneralViewModel
    {
        public List<ApplicationUserViewModel> ApplicationUserViewModels { get; set; }
        public List<HostelViewModel> HostelViewModels { get; set; }
        public List<RoomViewModel> RoomViewModels { get; set; }
    }
}
