using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5780_02_2956_9500
{
    class Host
    {
        public int HostKey { get; set; }

        List<HostingUnit> HostingUnitCollection;
        Host(int id,int hostingUnitAmount)
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
                index++;
                if (foundFreeHostingUnit)
                {
                    return HostingUnitCollection.ElementAt(index).HostingUnitKey;
                }
            }
            return -1;
        }
        public int GetHostAnnualBusyDays()
        {
            int sum = 0;
            for (int i = 0; i < HostingUnitCollection.Count; i++)
            {
                sum += HostingUnitCollection.ElementAt(i).GetAnnualBusyDays();
            }
            return sum;
        }

    }
}
