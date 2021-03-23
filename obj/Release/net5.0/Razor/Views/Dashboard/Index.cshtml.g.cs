#pragma checksum "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5170ea97d434b19bb2e713188e68cd57c84a506b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_Index), @"mvc.1.0.view", @"/Views/Dashboard/Index.cshtml")]
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
#line 1 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/_ViewImports.cshtml"
using SmartOffice;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/_ViewImports.cshtml"
using SmartOffice.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5170ea97d434b19bb2e713188e68cd57c84a506b", @"/Views/Dashboard/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8d5a01f9e15c3aae7144ab8a5e5c54e868a74dc1", @"/Views/_ViewImports.cshtml")]
    public class Views_Dashboard_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
  
    ClaimsPrincipal currentUser = this.User;
    string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;
    string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
    ViewData["Title"] = "Dashboard";

#line default
#line hidden
#nullable disable
            WriteLiteral("<h4>");
#nullable restore
#line 8 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
<div id=dashboard-container class=""cflex"">
    <div id=""guages"" class=""cflex"">
        <div class=""guage-meter"" class=""cflex"">
            <div class=""guage-value"">04</div>
            <div class=""guage-name"">Assigned</div>
        </div>
        <div class=""guage-meter"" class=""cflex"">
            <div class=""guage-value"">01</div>
            <div class=""guage-name"">Pending</div>
        </div>
        <div class=""guage-meter"" class=""cflex"">
            <div class=""guage-value"">17</div>
            <div class=""guage-name"">Executed</div>
        </div>
        <div class=""guage-meter"" class=""cflex"">
            <div class=""guage-value"">16</div>
            <div class=""guage-name"">Completed</div>
        </div>
        <div class=""guage-meter"" class=""cflex"">
            <div class=""guage-value"">38</div>
            <div class=""guage-name"">Total</div>
        </div>
    </div>
");
#nullable restore
#line 32 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
     if (roleOfCurrentUser == "manager"
        || roleOfCurrentUser == "admin"
        || roleOfCurrentUser == "superuser")
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div id=\"listOfUsers\">\n");
#nullable restore
#line 37 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
             foreach (var user in ViewBag.AllUsers)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div");
            BeginWriteAttribute("id", " id=\"", 1442, "\"", 1501, 1);
#nullable restore
#line 39 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
WriteAttributeValue("", 1447, user.GetType().GetProperty("Id").GetValue(user, null), 1447, 54, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"dash-user\">\n                    ");
#nullable restore
#line 40 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
               Write(user.GetType().GetProperty("Names").GetValue(user, null));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                </div>\n");
#nullable restore
#line 42 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\n");
#nullable restore
#line 44 "/home/ben/Documents/ProjectsMe/SmartOffice/Views/Dashboard/Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
