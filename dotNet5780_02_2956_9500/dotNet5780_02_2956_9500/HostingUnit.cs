﻿using System;
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
            return "Hosting unit key: " + hostingUnitKey + "\n" + printSchedule(Diary);
        }


        /// <param name="Calendar">Calendar is the data base in which the hosting scedule saved inside</param>

        /// <summary>
        /// Accepts hosting requirement. If the matrix has a sequence of 
        /// Free days in accordance with the above requirement, the function indicates in the matrix that the unit was captured, 
        /// And signifies within the request itself that the request was approved(IsApproved = true)
        /// </summary>
        /// <param name="guestReq"></param>
        /// <returns> Returns true if the matrix has a sequence of Free days in accordance with the above requirement,
        /// If the requested dates are not available(even in part), returns "false". </returns>
        public bool ApprovedRequest(GuestRequest guestReq)
        {
            bool free = false;

            int month, tmpMonth, day, tmpDay, length;
            bool occupied;


            occupied = false;
            Console.WriteLine("enter day");
            day = guestReq.EntryDate.Day;
            tmpDay = day;

            Console.WriteLine("enter month");
            month = guestReq.EntryDate.Month;
            tmpMonth = month;

            Console.WriteLine("enter length");
            length = (guestReq.ReleaseDate - guestReq.EntryDate).Days;

            // If length = 1 and the first day is occiupied
            if ((1 == length) && (Diary[month - 1, day - 1]))
            {
                if (32 == day + length)
                {
                    if (Diary[month, 0])
                    {
                        occupied = true;
                    }
                }
                else if (Diary[month - 1, day])
                {
                    occupied = true;
                }
            }
            else
            {
                for (int i = 0; i < length - 1; i++) // iterate on all days and check if available
                {
                    if (tmpDay + i > 30 && tmpMonth < 12)
                    {
                        tmpMonth++;
                        tmpDay = (tmpDay + i) % 31;
                    }
                    if (Diary[tmpMonth - 1, tmpDay])
                    {
                        occupied = true;
                    }
                }
            }
            if (occupied)
            {
                free = false;
                // Console.WriteLine("the request was denied");
            }
            else
            {
                free = true;

                //Console.WriteLine("your request has been approved");
                tmpDay = day;
                tmpMonth = month;
                Diary[tmpMonth - 1, day - 1] = true;
                for (int i = 0, j = 0; i < length - 1; i++, j++) // iterate on all days and check if available
                {
                    if (tmpDay + j > 30)
                    {
                        if (tmpMonth < 12)
                            tmpMonth++;
                        tmpDay = 0;
                        j = 0;
                    }
                    Diary[tmpMonth - 1, tmpDay + j] = true;
                }
            }
            if (free)
            {
                guestReq.IsApproved = true;
                return true;
            }
            else
            {
                return false;
            }
        }




        /// <summary>
        ///  The function prints out for each hosting period it's first and last dayof hosting.
        /// </summary>
        /// <param name="Calendar">Calendaris the data base in which the hosting scedule saved inside</param>
        private static string printSchedule(bool[,] Calendar)
        {
            string toReturn = string.Empty;
            ArrayList tmpArr = new ArrayList();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (Calendar[i, j])
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
            for (int i = 0; i < tmpArr.Count; i++)
            {
                if ((i == 0) && (tmpArr[i].ToString() != "empty"))
                {
                    toReturn += tmpArr[i].ToString() + " - ";
                    // Console.Write(tmpArr[i].ToString() + " - ");
                    state++;
                }
                else if ((tmpArr[i].ToString() != "empty") && (state % 2 == 0))
                {
                    toReturn += tmpArr[i].ToString() + " - ";
                    //Console.Write(tmpArr[i].ToString() + " - ");
                    state++;
                }
                else if ((tmpArr[i].ToString() == "empty") && (state % 2 == 1))
                {
                    state++;
                    toReturn += tmpArr[i - 1].ToString();
                    //Console.WriteLine(tmpArr[i - 1].ToString());
                }
            }
            return toReturn;
        }



        /// <summary>
        /// The function prints out the number of occupied days out of the whole year, and it's percentage.
        /// </summary>
        /// <param name="Calendar">Calendaris the data base in which the hosting scedule saved inside</param>
        public int GetAnnualBusyDays()
        {
            int countOccpiedDays = 0;
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
            return countOccpiedDays;

        }

        /// <summary>
        /// The function prints out the number of occupied days out of the whole year, and it's percentage.
        /// </summary>
        /// <param name="Calendar">Calendaris the data base in which the hosting scedule saved inside</param>
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

        public int CompareTo(object obj)
        {
            return GetAnnualBusyDays().CompareTo(((HostingUnit)obj).GetAnnualBusyDays());
        }
    }
}
