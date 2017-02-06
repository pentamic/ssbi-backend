using System.Data;
using System.Xml;

namespace Pentamic.SSBI.Models
{
    public static class PivotDataSourceExtensions
    {
        private const string XmlaWrap = @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><ExecuteResponse><return>{0}</return></ExecuteResponse></soap:Body></soap:Envelope>";
        private const string XmlaDiscoverWrap = @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><DiscoverResponse xmlns=""urn:schemas-microsoft-com:xml-analysis""><return>{0}</return></DiscoverResponse></soap:Body></soap:Envelope>";

        public static string ToXmlaResult(this XmlReader reader)
        {
            return string.Format(XmlaWrap, reader.ReadOuterXml());
        }

        public static string ToXmlaDiscoverResult(this DataSet dataSet)
        {
            var str = dataSet.GetXml();
            str = str.Replace("NewDataSet", "root")
                .Replace("rowsetTable", "row");
            return string.Format(XmlaDiscoverWrap, str);
        }
    }
}