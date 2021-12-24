using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Models;

namespace CarService.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperfactory)
        {
            urlHelperFactory = helperfactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; } //referans sayfa
        public string PageAction { get; set; } //hangi action calistirilacak
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext); //urlmizi döndürür
            TagBuilder result = new TagBuilder("div");

            for (int i = 1; i <= PageModel.TotalPage; i++) //1 den total pagede tutulan sayiya kadar sayfa gosterilmesi icin
            {
                TagBuilder tag = new TagBuilder("a"); //anchor tag listesi
                string url = PageModel.UrlParam.Replace(":", i.ToString()); //linke numaraları ekliyoruz
                tag.Attributes["href"] = url;
                tag.AddCssClass(PageClass);
                tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag); //bu resultu taghelperoutpu olarak kullanıyoruz
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
