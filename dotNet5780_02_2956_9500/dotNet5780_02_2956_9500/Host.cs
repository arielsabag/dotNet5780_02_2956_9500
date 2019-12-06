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
        public int HostKey { get; set; } // Id number of the Host
        public List<HostingUnit> HostingUnitCollection; // list of hosting units of the Host

        /// <summary>
        /// A constructor function that accepts the host's ID and the amount of hosting units that the Host have, and initializes the HostingUnitCollection list so that all units are free.
        /// </summary>
        /// <param name="id">Id number of the Host</param>
        /// <param name="hostingUnitAmount">The amount of hosting units that the host have</param>
        public Host(int id,int hostingUnitAmount)
        {
            HostKey = id; 
            HostingUnitCollection = new List<HostingUnit>();
            for (int i = 0; i < hostingUnitAmount; i++) // Initialize the List so that all the units are free
            {
                HostingUnitCollection.Add(new HostingUnit());
            }
        }

        /// <summary>
        /// Displays a list of hosting units of the same host. For each unit: a serial number and the date list in which it is occupied. (the hosting unit's ToString must be used )
        /// </summary>
        /// <returns>List of hosting units of the same host. For each unit: a serial number and the date list in which it is occupied.</returns>
        public override string ToString()
        {
            string toReturn = string.Empty;
            for (int i = 0; i < HostingUnitCollection.Count; i++)
            {
                toReturn += HostingUnitCollection.ElementAt(i).ToString();
            }
            return toReturn;
        }

        /// <summary>
        /// A function that receives an order request and returns the unit serial number of The hosting unit
        /// that available at the same host to accept the order. If no such unit exists, the function will return 1.
        /// - The function will use the ApproveRequest function of a hosting unit.
        /// </summary>
        /// <param name="guestReq">The request order</param>
        /// <returns> If no such unit exists, the function will return 1. otherwise it will return the hosting unit serial key</returns>
        private long SubmitRequest(GuestRequest guestReq)
        {
            bool foundFreeHostingUnit = false;
            int index = 0;
          
            while ((!foundFreeHostingUnit) && (index < HostingUnitCollection.Count))
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
        ///  A function that returns the total number of nights in all of the accommodation units of the owner. 
        ///  Using the GetAnnualBusyDays hosting unit function.
        /// </summary>
        /// <returns>the total number of nights in all of the accommodation units of the owner.</returns>
        public int GetHostAnnualBusyDays()
        {
            int sum = 0;
            for (int i = 0; i < HostingUnitCollection.Count; i++)
            {
                sum += HostingUnitCollection.ElementAt(i).GetAnnualBusyDays();// GetAnnualBusyDays - The function calulates out the number of occupied days out of the whole year. and return it
            }
            return sum;
        }

        /// <summary>
        ///  The function sorts out the HostingUnitsCollection according of it's Occupancy.
        /// </summary>
        public void SortUnits()
        {
            HostingUnitCollection.Sort();
        }

        ///// <summary>
        ///// Function for allocates requests to Hosting Units: The function gets unknown number of GuestRequest and will try to schedule the requests in the order they were accepted in the various Hosting Units order by using the SubmitRequest funtion of the host 
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns>returns True if all requests were accepted , otherwise returns False  </returns>
        ///// 




        /// <summary>
        /// Request allocation function for hosting units. The function will get an unknown number of Hosting requirements,
        /// and try to fix in the requirements in the order in which they were received, In the various hosting units, using the SubmitRequest function of the Host. The function returns true if all requests are possible and false otherwise.
        /// </summary>
        /// <param name="list"> All the guest requests</param>
        /// <returns>The function returns true if all requests are possible and false otherwise</returns>
        public bool AssignRequests(params GuestRequest[] list)
        {
            bool notAvailable = false;
            for (int i = 0; i < list.Length; i++)
            {
                if (SubmitRequest(list[i])==-1)
                {
                    notAvailable = true;
                }
            }
            if (notAvailable)
            {
                return false;
            }
            return true;            
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

        /// <summary>
        /// To make it possible to cross the department with a foreach loop, that in any  Iteration  another host unit of the host will return.
        /// </summary>
        /// <returns>hosting unit</returns>
        public IEnumerator GetEnumerator()
        {
            foreach (HostingUnit val in HostingUnitCollection)
            {
                yield return val;
            }
        }
    }

   

}
