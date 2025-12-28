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
            string message = "Aktivne";
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




  
}