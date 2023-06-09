using Microsoft.AspNetCore.Components;
using Proyecto_Front_MuresanCristian.Services.Interfaces;
using Proyecto_Front_MuresanCristian.Services;
using Proyecto_Front_MuresanCristian.Entities;
using Blazored.LocalStorage;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Proyecto_Front_MuresanCristian.Pages
{
    public partial class NotePage
    {
        [Inject]
        private IRestService MyRestService { get; set; }
        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        private UserInfo user { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

        protected override Task OnInitializedAsync()
        {
            Notes = new ObservableCollection<Note>();
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                user = await LocalStorage.GetItemAsync<UserInfo>("user");

                var notes = await MyRestService.GetUserNotes(user);

                if (notes != null)
                {
                    Notes = new ObservableCollection<Note>(notes);
                    StateHasChanged();
                }
            }

            await base.OnInitializedAsync();
        }
    }
    
}
