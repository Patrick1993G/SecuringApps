#pragma checksum "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\Shared\ViewFeedbackPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ff0da51164cf3de6cc49d0a76cadf3abfcfa112"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_ViewFeedbackPartial), @"mvc.1.0.view", @"/Views/Shared/ViewFeedbackPartial.cshtml")]
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
#line 1 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\_ViewImports.cshtml"
using SecuringApps_WebApplication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\_ViewImports.cshtml"
using SecuringApps_WebApplication.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8ff0da51164cf3de6cc49d0a76cadf3abfcfa112", @"/Views/Shared/ViewFeedbackPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5bf0f7a85bb1f1c43d29e2c4b4b3fd7ee786227d", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_ViewFeedbackPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\Shared\ViewFeedbackPartial.cshtml"
 if (TempData["feedback"] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-primary\" role=\"alert\">\r\n        ");
#nullable restore
#line 5 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\Shared\ViewFeedbackPartial.cshtml"
   Write(TempData["feedback"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n");
#nullable restore
#line 7 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\Shared\ViewFeedbackPartial.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 9 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\Shared\ViewFeedbackPartial.cshtml"
 if (TempData["warning"] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-warning\" role=\"alert\">\r\n        ");
#nullable restore
#line 12 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\Shared\ViewFeedbackPartial.cshtml"
   Write(TempData["warning"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n");
#nullable restore
#line 14 "C:\Users\patri\source\repos\SecuringApps\Securing\SecuringApps_WebApplication\Views\Shared\ViewFeedbackPartial.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
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
