using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5780_02_2956_9500
{
    /// <summary>
    /// The class HostingUnit represents hosting unit
    /// </summary>
    public class HostingUnit : IComparable
    {
        /// <summary>
        /// Number of accommodation unit unique identifier for each object of the class 5 digit running code
        /// </summary>
        private static int stSerialKey = 0;

        /// <summary>
        ///  Number of current hosting unit גetermined in the constructor according to the static field
        /// </summary>
        private int hostingUnitKey;
        public int HostingUnitKey
        {
            get { return hostingUnitKey; }
        }
        /// <summary>
        /// The status of the accommodation unit for one year (12-month x 31-day matrix,
        /// For each day check if the unit is free / tonnage)
        /// </summary>
        bool[,] Diary = new bool[12, 31];

        /// <summary>
        /// This Ctor allocates an 8 digits hosting unit key for the object 
        /// </summary>
        public HostingUnit()
        {
            hostingUnitKey = stSerialKey++;

        }

        /// <summary>
        /// Displays for the unit its serial number, and 
        /// The list of periods in which it is occupied. ( First Date and End for each Period)
        /// </summary>
        /// <returns>the unit serial number and The list of periods in which it is occupied. ( First Date and End for each Period) </returns>
        public override string ToString()
        {
            return "Hosting unit key: " + hostingUnitKey + "\n" + printSchedule();
        }



        /// <summary>
        /// Accepts hosting requirement. If the matrix has a sequence of 
        /// Free days in accordance with the above requirement, the function indicates in the matrix that the unit was captured, 
        /// And signifies within the request itself that the request was approved(IsApproved = true)
        /// </summary>
        /// <param name="guestReq">It is a class that represents a guest request that contains 3 fields: 1- entry date, 2- relese date, 3- is approved(if the request approved bool type)</param>
        /// <returns> Returns true if the matrix has a sequence of Free days in accordance with the above requirement,
        /// If the requested dates are not available(even in part), returns "false". </returns>
        public bool ApprovedRequest(GuestRequest guestReq)
        {
            bool[] diaryArray = new bool[31 * 12 + 1];// 
            // coping the diary array to array of one dimension
            for (int i = 0, index = 1; i < 12; i++)
            {
                for (int j = 0; j < 31; j++, index++)
                {
                    diaryArray[index] = Diary[i, j];
                }
            }

            bool outOfRange = false, occupied = false; // out of range - indicates if the dates requests is not in that year, occupied - if the date requests is not available
            int month, day, length, tmpDay, tmpMonth;// month-entrydate.month, day - entrydate.day, length - releasedate - entrydate, tmpDay = day, tmpMonth = moth
            day = guestReq.EntryDate.Day;
            month = guestReq.EntryDate.Month;
            length = (guestReq.ReleaseDate - guestReq.EntryDate).Days;
            tmpDay = day;
            tmpMonth = month;

            // If length ==1
            if (1 == length)
            {
                // if it will be out of range
                if (((((tmpMonth - 1) * 31) + tmpDay) + 1) == (31 * 12 + 1))
                {
                    return false;
                }
                if (diaryArray.ElementAt((((month - 1) * 31) + day) + 1))// if dates not available
                {
                    occupied = true;
                }
            }
            else
            {
                for (int i = 0; i < length; i++, tmpDay++) // iterate on all days and check if available
                {
                    if (((((tmpMonth - 1) * 31) + tmpDay) + 1) == (31 * 12 + 1))// if out of range
                    {
                        outOfRange = true;
                        break;
                    }
                    if (diaryArray.ElementAt((((tmpMonth - 1) * 31) + tmpDay) + 1))// if date not available
                    {
                        occupied = true;
                    }
                }
            }
            tmpDay = day;// initialize tmpDay again to day beacouse we maby it has changed in the previous loop
            tmpMonth = month;// initialize tmpMonth again to day beacouse we maby it has changed in the previous loop

            if ((outOfRange) || (occupied))// if the dates indeed out of range, or if the dates not available
            {
                return false; // out of range - // the request was not accepted
            }
            else// Here it means that the dates are not out of range  and available and  it will mark as true all dated from the requests
            {
                if (1 == length) 
                {
                    diaryArray[((((month - 1) * 31) + day) + 1)] = true;
                }
                else
                {
                    for (int i = 0; i < length; i++, tmpDay++) 
                    {
                        diaryArray[((((tmpMonth - 1) * 31) + tmpDay) + 1)] = true;
                    }
                }

                for (int i = 0, index = 1; i < 12; i++)
                {
                    for (int j = 0; j < 31; j++, index++)
                    {
                        Diary[i, j] = diaryArray[index];
                    }
                }
                guestReq.IsApproved = true;// marks the bool field 'isApproved' ofguestRequest as 'true'
                return true; // the request accepted
            }
           
        }

        /// <summary>
        /// The function calulates out the number of occupied days out of the whole year.
        /// </summary>
        /// <returns>The number of occupied days out of the whole year.</returns>
        public int GetAnnualBusyDays()
        {
            int countOccupiedDays = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (Diary[i, j])
                    {
                        countOccupiedDays++;
                    }
                }
            }
            return countOccupiedDays;
        }

        /// <summary>
        ///  The function calculates the percentage of the occupied days out of the whole year.
        /// </summary>
        /// <returns>The percentage of the occupied days out of the whole year.</returns>
        public float GetAnnualBusyPercentage()
        {
            float countOccpiedDays = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (Diary[i, j])
                    {
                        countOccpiedDays++;
                    }
                }
            }
            return countOccpiedDays / 372;
        }

        /// <summary>
        /// To eneble comparing btween two hosting units by their amount of occupied days of the year.
        /// </summary>
        /// <param name="obj">The other hosting unit object</param>
        /// <returns>Less than zero	 -This instance precedes obj in the sort order ,Zero - This instance occurs in the same position in the sort order as obj,Greater than zero	- This instance follows obj in the sort order.</returns>
        public int CompareTo(object obj)
        {
            return GetAnnualBusyDays().CompareTo(((HostingUnit)obj).GetAnnualBusyDays());
        }


        /// <summary>
        /// The function returns string that contains The list of periods in which this hosting unit is occupied. (First and end date for each period)
        /// </summary>
        /// <returns>returns string that contains The list of periods in which this hosting unit is occupied. (First and end date for each period)</returns>
        private string printSchedule()
        {
            string toReturn = string.Empty; // the string to return from this function that will be contain the output 
            ArrayList tmpArr = new ArrayList();
            for (int i = 0; i < 12; i++)// iterate all the diary matrix and in case with some i and j if diary[i,j] is equal to true so it is  add to tmpArr the appropriate date ,otherwise it is add to tmpArr the string "empty"
            {
                for (int j = 0; j < 31; j++)
                {
                    if (Diary[i, j])
                    {
                        tmpArr.Add((j + 1) + "/" + (i + 1));
                    }
                    else
                    {
                        tmpArr.Add("empty");
                    }
                }
            }
            int state = 0;
            for (int i = 0; i < tmpArr.Count; i++)// iterate tmpArr and for each sequence of dates take the first date and the last and add them to 'toReturn' string with '-' betwwen the first date and the last of every sequence and after every first date + '-' + last date add "\n"
            {
                if ((i == 0) && (tmpArr[i].ToString() != "empty"))//if it is the first date and also the first date in the entire tmpArr
                {
                    toReturn += tmpArr[i].ToString() + " - ";
                    state++;
                }
                else if ((state % 2 == 0) && (tmpArr[i].ToString() != "empty"))//if it is  the first date but not the first date in the entire tmpArr
                {
                    toReturn += tmpArr[i].ToString() + " - ";
                    state++;
                }
                else if ((state % 2 == 1) && (tmpArr[i].ToString() == "empty"))//if the last  date was before one iteration in tmpArr
                {
                    state++;
                    toReturn += tmpArr[i - 1].ToString() + "\n";
                }
            }
            toReturn += "\n";
            return toReturn; 
        }
    }
}
