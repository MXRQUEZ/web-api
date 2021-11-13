using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Flags]
    public enum Platforms
    {
        PersonalComputer = 1,
        Mobile = 2,
        PlayStation = 4,
        Xbox = 8,
        Nintendo = 16
    }
}
