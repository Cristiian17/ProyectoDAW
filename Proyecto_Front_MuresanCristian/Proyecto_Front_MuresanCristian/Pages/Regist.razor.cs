using Microsoft.AspNetCore.Components;
using Proyecto_Front_MuresanCristian.Entities;
using Proyecto_Front_MuresanCristian.Services;
using Proyecto_Front_MuresanCristian.Services.Interfaces;

namespace Proyecto_Front_MuresanCristian.Pages
{
    public partial class Regist
    {
        [Inject]
        protected IRestService MyRestService { get; set; }
        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public bool ShowModal { get; set; } = false;
        public async Task Register()
        {
            try
            {
                if(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
                {
                    await DialogService.AlertAsync("", "No puedes dejar campos vacios");
                    return;
                }

                var user = new User { Email = Email, Name = UserName, Password = Password };
                if (await MyRestService.AddUsser(user))
                {
                    Navigation.NavigateTo("/");
                }
            }
            catch(Exception ex)
            {
                await DialogService.AlertAsync("", ex.Message);
            }
        }

    }
}
