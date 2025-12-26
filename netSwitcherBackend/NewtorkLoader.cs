using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Xml.Linq;
namespace netSwitcherBackend;

[SupportedOSPlatform("windows")]

public class NewtorkLoader
    {
   

    //method for loading all avaliable network cards
    public List<NetworkInfo> getAvaliableNets()
    {
       var list = new List<NetworkInfo>();
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;

            bool isEth = false;
            bool isWifi = false;

            string description = ni.Description.ToLower();
            string name = ni.Name.ToLower();

            if (name.Contains("*") ||
                description.Contains("virtual") ||
                description.Contains("miniport") ||
                description.Contains("pseudo") ||
                description.Contains("vmware") ||
                description.Contains("vbox"))
            {
                continue; 
            }


            if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                isEth = true;
            }
            else if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                isWifi = true;
            }

            list.Add(new NetworkInfo
            {
                Name = ni.Name,
                isActive = ni.OperationalStatus == OperationalStatus.Up,
                isEthernet = isEth,
             
            });
        }
        return list;
    

}

       
}
