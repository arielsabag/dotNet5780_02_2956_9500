using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5780_02_2956_9500
{
    public class GuestRequest
    {
        public string EntryDate { get; set; }
        public string ReleaseDate { get; set; }
        public bool IsApproved { get; set; }

        public override string ToString()
        {
            return "Entry Date" + EntryDate + "Release Date" + ReleaseDate + "Is Approved" + IsApproved;
        }


    }
}
