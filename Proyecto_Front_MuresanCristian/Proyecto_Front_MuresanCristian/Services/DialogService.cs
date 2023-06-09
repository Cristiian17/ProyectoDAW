using Microsoft.JSInterop;

namespace Proyecto_Front_MuresanCristian.Services
{
    public class DialogService
    {
        public IJSRuntime JsRuntime { get; set; }
        public DialogService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async Task<bool> ConfirmAsync(string title, string description, string yes = "Si", string no = "No")
        {
            return await JsRuntime.InvokeAsync<bool>("SWConfirm", title, description, yes, no);
        }
        public async Task<bool> AlertAsync(string title, string description, string ok = "Ok", string okIcon = "fa fa-thumbs-up")
        {
            return await JsRuntime.InvokeAsync<bool>("SWAlert", title, description, ok, okIcon);
        }
        public async Task<bool> OpenWindow(string url)
        {
            return await JsRuntime.InvokeAsync<bool>("OpenWindow", url);
        }

    }
}

