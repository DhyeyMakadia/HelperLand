#pragma checksum "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "20afadbc21a43c474be99d16423c21f20e6f4b0b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_SRDashModal), @"mvc.1.0.view", @"/Views/Home/SRDashModal.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"20afadbc21a43c474be99d16423c21f20e6f4b0b", @"/Views/Home/SRDashModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"38e0af9faa8b6532b4ecb207049a7eed256a025b", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_SRDashModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Helperland.Models.CustomModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
  
    ViewData["Title"] = "SRDashModal";
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<!-- Modals -->\r\n\r\n<div class=\"row\">\r\n    <div class=\"col\">\r\n        <div>\r\n            <div><b>");
#nullable restore
#line 14 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
               Write(Model.ServiceStartDate.ToShortDateString());

#line default
#line hidden
#nullable disable
            WriteLiteral(" <span>08:00 - 11:30</span></b></div>\r\n            <div><b>Duration:</b> ");
#nullable restore
#line 15 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                             Write(Model.ServiceHours);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Hrs</div>\r\n        </div>\r\n        <hr>\r\n        <div>\r\n            <b>Service Id:</b>\r\n            <span>");
#nullable restore
#line 20 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
             Write(Model.ServiceId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n        </div>\r\n        <div>\r\n            <b>Extras:</b>");
#nullable restore
#line 23 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                     Write(Model.ServiceExtraId);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n        <div class=\"net-amount text-center\">\r\n            <b class=\"float-left\">Net Amount:</b>\r\n            <span>");
#nullable restore
#line 27 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
             Write(Model.TotalCost);

#line default
#line hidden
#nullable disable
            WriteLiteral(" €</span>\r\n        </div>\r\n        <hr>\r\n        <div>\r\n            <b>Cleaning Address:</b>\r\n            <span>");
#nullable restore
#line 32 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
             Write(Model.AddressLine1);

#line default
#line hidden
#nullable disable
            WriteLiteral(" , ");
#nullable restore
#line 32 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                                   Write(Model.AddressLine2);

#line default
#line hidden
#nullable disable
            WriteLiteral(" , ");
#nullable restore
#line 32 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                                                         Write(Model.City);

#line default
#line hidden
#nullable disable
            WriteLiteral(" , ");
#nullable restore
#line 32 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                                                                       Write(Model.PostalCode);

#line default
#line hidden
#nullable disable
            WriteLiteral(" , ");
#nullable restore
#line 32 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                                                                                           Write(Model.State);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n        </div>\r\n        <div>\r\n            <b>Service Address:</b>\r\n            <span>Same as Cleaning Address</span>\r\n        </div>\r\n        <div>\r\n            <b>Phone:</b>\r\n            <span>+91 ");
#nullable restore
#line 40 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                 Write(Model.Mobile);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n        </div>\r\n        <hr>\r\n        <div>\r\n            <b>Comments</b>:");
#nullable restore
#line 44 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
                       Write(Model.Comments);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 45 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
             if (@Model.HasPets)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div>I have pets at home.</div>\r\n");
#nullable restore
#line 48 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div>I Don\'t have pets at home.</div>\r\n");
#nullable restore
#line 52 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        </div>
        <hr>
        <div class=""action-history-modal"">
            <button class=""reschedule-upcoming"" data-dismiss=""modal""
                    data-target=""#ServiceHistoryRescheduleModal"" data-toggle=""modal"">
                Reschedule
            </button>
            <button class=""cancel-upcoming CancelServiceRequest"" id=""CancelServiceRequest"" data-target=""#ServiceHistoryCancelModal""");
            BeginWriteAttribute("SRid", " SRid=\"", 1852, "\"", 1882, 1);
#nullable restore
#line 60 "E:\TATVASOFT\Project-Helperland\Helperland\Helperland\Helperland\Views\Home\SRDashModal.cshtml"
WriteAttributeValue("", 1859, Model.ServiceRequestId, 1859, 23, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-toggle=\"modal\" data-dismiss=\"modal\">\r\n                Cancel\r\n            </button>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Helperland.Models.CustomModel> Html { get; private set; }
    }
}
#pragma warning restore 1591