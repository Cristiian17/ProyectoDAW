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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await CheckUserInfo();
            }
        }
        private async Task CheckUserInfo()
        {
            var userInfo = await LocalStorage.GetItemAsync<UserInfo>("user");
            if (userInfo != null)
            {
                var newUserInfo = await MyRestService.RegenerateToken(userInfo);
                await LocalStorage.SetItemAsync("user", newUserInfo);
                Navigation.NavigateTo("/Notes");
            }
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
