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
    public List<NetworkInfo> GetNetWorkCards()
    {
        var list = new List<NetworkInfo>();

        var modernInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        var searcher = new ManagementObjectSearcher(
            "SELECT NetConnectionID, NetEnabled, AdapterTypeId, Name FROM Win32_NetworkAdapter WHERE NetConnectionID IS NOT NULL"
        );

        foreach (ManagementObject obj in searcher.Get())
        {
            string name = obj["NetConnectionID"]?.ToString() ?? "unknown";
            bool isEnabled = (bool)(obj["NetEnabled"] ?? false);

            var modernMatch = modernInterfaces.FirstOrDefault(i => i.Name == name);

            bool isEth = true;
            bool isBT = false;

            if (modernMatch != null)
            {
                // 131 je interný kód Windowsu pre Bluetooth PAN (Personal Area Network)
                bool isBluetoothType = (int)modernMatch.NetworkInterfaceType == 131;

                // Niekedy sa Bluetooth hlási ako Ppp, ak ide o tethering
                bool isPppBluetooth = modernMatch.NetworkInterfaceType == NetworkInterfaceType.Ppp &&
                                      name.ToLower().Contains("bluetooth");

                if (isBluetoothType || isPppBluetooth || name.ToLower().Contains("bluetooth"))
                {
                    isBT = true;
                    isEth = false;
                }
                else if (modernMatch.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    isEth = false;
                    isBT = false;
                }
            }

            list.Add(new NetworkInfo
            {
                Name = name,
                isActive = isEnabled,
                isEthernet = isEth,
                isBluetooth = isBT,
                message = isEnabled ? "Aktivne" : "pripojiť",
                btnColor = isEnabled ? "" : "#FF2369B6"
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
              
                System.Diagnostics.Debug.WriteLine($"Error on '{name}': {ex.Message}");
            }
        }
    }




  
}