using System.IO;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Xml.Linq;

namespace Pentamic.SSBI.Models
{
    public class PivotDataSourceRequestModelBinder : IModelBinder
    {
        private static readonly XNamespace nx = "urn:schemas-microsoft-com:xml-analysis";

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var request = new PivotDataSourceRequest();
            var content = actionContext.Request.Content.ReadAsStreamAsync().Result;
            var document = XElement.Load(new StreamReader(content));
            request.Discover = document.Descendants(nx + "Discover").Any();

            request.Statement = (string)document.Descendants(nx + "Statement").FirstOrDefault();

            if (!string.IsNullOrEmpty(request.Statement))
            {
                request.Statement = request.Statement.Replace("&amp;", "&");
            }
            request.Command = (string)document.Descendants(nx + "RequestType").FirstOrDefault();
            request.Restrictions = document.Descendants(nx + "RestrictionList").Elements().ToDictionary(n => n.Name.LocalName, n => (string)n.Value);
            bindingContext.Model = request;
            return true;
        }
    }
}