using Microsoft.AspNetCore.Components;
using Proyecto_Front_MuresanCristian.Entities;
using Proyecto_Front_MuresanCristian.Services;
using Proyecto_Front_MuresanCristian.Services.Interfaces;
using Blazored.LocalStorage;

namespace Proyecto_Front_MuresanCristian.Pages
{
    public partial class Login
    {
        [Inject]
        private IRestService MyRestService { get; set; }
        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        private string Email { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        public async Task CheckLogin()
        {

            try
            {
                if(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Email))
                {
                    await DialogService.AlertAsync("", "Email o contraseña incorrectos");
                    return;
                }

                var user = new User() { Email = Email, Password = Password, Name = Email };

                var userInfo = await MyRestService.Login(user);
                await LocalStorage.SetItemAsync("user", userInfo);
                Navigation.NavigateTo("/Notes");
            }
            catch (Exception ex)
            {
                await DialogService.AlertAsync("", ex.Message);
            }
        }
    }
}
