// Check if network is available
using System.Text;

if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
{
    var sb = new StringBuilder();

    string hostname = System.Net.Dns.GetHostName();
    System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(hostname);

    foreach(System.Net.IPAddress address in host.AddressList)
    {
        sb.Append(" " + address);
    }

    Console.WriteLine(sb.ToString());
 }
 else
 {
    Console.WriteLine("No Network Connection");
 }
