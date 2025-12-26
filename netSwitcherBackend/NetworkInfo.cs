using System.Runtime.Versioning;
namespace netSwitcherBackend;

  
    public class NetworkInfo
    {

    public String? Name { get; set; }
    public bool isActive { get; set; }
    public bool isEthernet { get; set; }

    public String iconKey => isEthernet ?  "/ETH.png" : "/WIFI.png" ;




}
