
using System.Text.Json;

namespace ConsoleApp;

public class NIC
{
    public enum NICType
    {
        Ethernet,
        TokenRing
    }
    
    public string SerialNumber { get; init; }
    public string Manufacturer { get; set; }
    public string MacAddress { get; set; }
    public NICType Type { get; set; }

    private static NIC _instance;
    private static readonly string _path = @"../../../nic.json";
    private NIC()
    {
        using (StreamReader sr = new(_path))
        {
            string json = sr.ReadToEnd();
            
            json = json.Trim().Trim('[', ']');
            string[] properties = json.Split(',');

            SerialNumber = properties[0].Trim().Trim('"');
            Manufacturer = properties[1].Trim().Trim('"');
            MacAddress = properties[2].Trim().Trim('"');
            Type = (NICType)Enum.Parse(typeof(NICType), properties[3].Trim().Trim('"'));
        }
    }

    public static NIC GetInstance()
    {
        return _instance ??= new NIC();
    }

    public override string ToString()
    {
        return $"NIC: {SerialNumber} - {Manufacturer} - {MacAddress} - {Type}";
    }
}
