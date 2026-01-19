using System.Runtime.Versioning;
namespace netSwitcherBackend;

  
    public class NetworkInfo
    {
    public String? Name { get; set; }
    public bool isActive { get; set; }
    public bool isEthernet { get; set; }
    public bool isBluetooth { get; set; }

    public String? btnWriter { get; set; }
    public int speed { get; set; } = 150;

    public String btnColor { get; set; } 

    public String? message { get; set; }
    public String iconKey
    {
        get
        {
            if (isBluetooth) return "/BTH.png"; 
            return isEthernet ? "/ETH.png" : "/WIFI.png";
        }
    }


}
