using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Viewmodels
{
    public class HomeIndexVM
    {
        public HomeIndexLoginVM DigiMaster { get; set; }
        public HomeIndexFindClassroomVM DigiStudent { get; set; }
        public HomeIndexCreateClassroomVM CreateClassroom { get; set; }
        public bool LoggedIn { get; set; }
        public AccountRegisterVM Register { get; set; }
    }
}
