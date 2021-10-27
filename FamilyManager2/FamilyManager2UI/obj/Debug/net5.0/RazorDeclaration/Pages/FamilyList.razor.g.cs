// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace FamilyManager2UI.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using FamilyManager2UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\_Imports.razor"
using FamilyManager2UI.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\Pages\FamilyList.razor"
using Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\Pages\FamilyList.razor"
using Models;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/FamilyList")]
    public partial class FamilyList : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 47 "C:\Users\adria\DNP\DNP-AS-2\FamilyManager2\FamilyManager2UI\Pages\FamilyList.razor"
       

    private IList<Family> families;
    private IList<Family> filteredFamilies;

    private bool wasChecked;

    protected override async Task OnInitializedAsync() {
        families = FamilyData.GetFamilies();
        filteredFamilies = families;
        wasChecked = false;
    }


    private void FilterByAddress(ChangeEventArgs changeEventArgs) {
        List<Family> filter = new List<Family>();
        foreach (var family in families) {
            if (family.GetUniqueKey().ToLower().Contains(changeEventArgs.Value.ToString().ToLower()))
                filter.Add(family);
        }
        filteredFamilies = filter;
    }

    private void View(string streetName, int houseNumber) {
        navigationManager.NavigateTo($"Family/{streetName}/{houseNumber}");
    }

    private void SortByStreetName() {
        var sort = families.OrderBy(name => name.StreetName);
        filteredFamilies = sort.ToList();
    }

    private void FilterByPets() {
        List<Family> filter = new List<Family>();
        foreach (var family in families) {
            if (family.HasPets()) {
                filter.Add(family);
            }
        }
        if (!wasChecked) {
            filteredFamilies = families;
            wasChecked = true;
        }
        else {
            filteredFamilies = filter;
            wasChecked = false;
        }
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFamilyData FamilyData { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager navigationManager { get; set; }
    }
}
#pragma warning restore 1591
