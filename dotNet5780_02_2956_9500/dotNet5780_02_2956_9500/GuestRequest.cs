using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5780_02_2956_9500
{
    /// <summary>
    /// The class GuestRequest represents customer hosting requirement
    /// </summary>
    public class GuestRequest
    {

        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Indicates if the request approved
        /// </summary>
        public bool IsApproved { get; set; }

        public override string ToString()
        {
            return "Entry Date" + EntryDate + "Release Date" + ReleaseDate + "Is Approved" + IsApproved;
        }


    }
}
