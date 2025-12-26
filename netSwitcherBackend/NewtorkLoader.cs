using Microsoft.Win32;
using System.Drawing;
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
    public List<NetworkInfo> GetNetWorkCards()
    {
        var list = new List<NetworkInfo>();

        var searcher = new ManagementObjectSearcher(
                  "SELECT NetConnectionID, NetEnabled FROM Win32_NetworkAdapter WHERE NetConnectionID IS NOT NULL"
                   );

      
        
        foreach (ManagementObject obj in searcher.Get())
        {
            string name = obj["NetConnectionID"]?.ToString() ?? "unknown";
            bool? enabled = obj["NetEnabled"] as bool?;
            string message =  "Aktivne";
            String btnColor = "";


            if (enabled == false)
            {
                message ="pripojiť";
                btnColor = "#FF2369B6";
            }

            list.Add(new NetworkInfo
            {
                Name = name,
                message = message,
                btnColor = btnColor,
            });

        }
        return list;

    }



    public void ToggleAdapterStatus(string adapterName)
    {
        var searcher = new ManagementObjectSearcher(
            $"SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID = '{adapterName}'");

        foreach (ManagementObject obj in searcher.Get())
        {
            string name = obj["NetConnectionID"]?.ToString() ?? "???";
            var enabled = obj["NetEnabled"] as bool?;

            try
            {
                uint errorCode = (uint)obj["ConfigManagerErrorCode"];
                bool isCurrentlyDisabled = (errorCode == 22);

                if (!isCurrentlyDisabled)
                {
                    obj.InvokeMethod("Disable", null);
                }
                else
                {
                    obj.InvokeMethod("Enable", null);
                }
            }
            catch (Exception ex)
            {
                // Log pre debugging
                System.Diagnostics.Debug.WriteLine($"Error on '{name}': {ex.Message}");
            }
        }
    }


    public bool IsScriptAllowed()
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", false);

        if (key != null)
        {
            var value = key.GetValue("AutoConfigURL");
            return value != null && !string.IsNullOrWhiteSpace(value.ToString());
        }

        return false;
    }

    public string? FirstBoot()
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", false);

        if (key != null)
        {
            var value = key.GetValue("AutoConfigURL");
            return value as string; // bezpečný pretyp
        }

        return null; // v prípade že key je null alebo hodnota neexistuje
    }



}
    //public List<NetworkInfo> getAvaliableNets()
    //{
    //    var list = new List<NetworkInfo>();
    //    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
    //    {
    //        if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;

    //        bool isEth = false;
    //        bool isWifi = false;

    //        string description = ni.Description.ToLower();
    //        string name = ni.Name.ToLower();
    //        string message = "pripojiť";
    //        String btnColor = "#FF2369B6";

    //        if (name.Contains("*") ||
    //            description.Contains("virtual") ||
    //            description.Contains("miniport") ||
    //            description.Contains("pseudo") ||
    //            description.Contains("vmware") ||
    //            description.Contains("vbox"))
    //        {
    //            continue;
    //        }


    //        if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
    //        {
    //            isEth = true;
    //        }
    //        else if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
    //        {
    //            isWifi = true;
    //        }
    //        if (ni.OperationalStatus == OperationalStatus.Up)
    //        {
    //            message= "odpojiť";
    //            btnColor="";

    //        }


    //        list.Add(new NetworkInfo
    //        {
    //            Name = ni.Name,
    //            isActive = ni.OperationalStatus == OperationalStatus.Up,
    //            isEthernet = isEth,
    //            message = message,
    //            btnColor=btnColor

    //        });
    //    }
    //    return list;

    //}