using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5780_02_2956_9500
{
    public class Host:IEnumerable
    {
        public int HostKey { get; set; }
        public List<HostingUnit> HostingUnitCollection;

        public Host(int id,int hostingUnitAmount)
        {
            HostKey = id;
            HostingUnitCollection = new List<HostingUnit>();
            for (int i = 0; i < hostingUnitAmount; i++)
            {
                HostingUnitCollection.Add(new HostingUnit());
            }
        }

        public override string ToString()
        {
            string toReturn = string.Empty;
            for (int i = 0; i < HostingUnitCollection.Count; i++)
            {
                toReturn += HostingUnitCollection.ElementAt(i).ToString();
            }
            return toReturn;
        }

        private long SubmitRequest(GuestRequest guestReq)
        {
            bool foundFreeHostingUnit = false;
            int index = 0;
          
            while ((!foundFreeHostingUnit)&& (index < HostingUnitCollection.Count))
            {
                foundFreeHostingUnit = HostingUnitCollection.ElementAt(index).ApprovedRequest(guestReq);
                if (foundFreeHostingUnit)
                {
                    return HostingUnitCollection.ElementAt(index).HostingUnitKey;
                }
                index++;

            }
            return -1;
        }
        /// <summary>
        ///  The function returns all of occupied days in all hosting units of the host using the function GetHostAnnualBussyDays of the HostingUnits class
        /// </summary>
        /// <returns>Return all of occupied days in all hosting units of the host</returns>
        public int GetHostAnnualBusyDays()
        {
            int sum = 0;
            for (int i = 0; i < HostingUnitCollection.Count; i++)
            {
                sum += HostingUnitCollection.ElementAt(i).GetAnnualBusyDays();
            }
            return sum;
        }

        /// <summary>
        ///  The function sorts out the HostingUnitsCollection
        /// </summary>
        public void SortUnits()
        {
            HostingUnitCollection.Sort();
        }
        
        /// <summary>
        /// Function for allocates requests to Hosting Units: The function gets unknown number of GuestRequest and will try to schedule the requests in the order they were accepted in the various Hosting Units order by using the SubmitRequest funtion of the host 
        /// </summary>
        /// <param name="list"></param>
        /// <returns>returns True if all requests were accepted , otherwise returns False  </returns>
        public bool AssignRequests(params GuestRequest[] list)
        {
            bool available = true;
            for (int i = 0; i < list.Length; i++)
            {
                if (SubmitRequest(list[i])==-1)
                {
                    available = false;
                }
            }
            if (available)
            {
                return true;
            }
            return false;            
        }

        /// <summary>
        /// Indexer thats recieves a serial number of Hosting Unit in the Host list (0 or 1 or 2) and returns its object, if such unit is not exists NULL will be returned. 
        /// </summary>
        /// <param name="serialNumber">The Hosting Unit key that the Indexer recieves</param>
        /// <returns>If the Hosting Unit exists it is return it's object, otherwise NULL will be returned</returns>
        public HostingUnit this[int serialNumber]
        {
            get
            {
                for (int i = 0; i < HostingUnitCollection.Count; i++)
                {
                    if (HostingUnitCollection.ElementAt(i).HostingUnitKey == serialNumber)
                    {
                        return HostingUnitCollection.ElementAt(i);
                    }
                }

                return null;
            }
        }


        public IEnumerator GetEnumerator()
        {
            foreach (HostingUnit val in HostingUnitCollection)
            {
                yield return val;
            }

        }
    }

   

}
