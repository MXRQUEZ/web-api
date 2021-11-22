using System;

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
