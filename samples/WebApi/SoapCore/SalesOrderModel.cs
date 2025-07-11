using System.Xml.Serialization;

namespace WebApi.SoapCore;

[XmlRoot(ElementName = "body")]
public class SalesOrderModel
{
    [XmlElement(ElementName = "No")]
    public string? No { get; set; }
}
