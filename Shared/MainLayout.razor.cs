using System;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace blazorblog.Shared
{
    public class MainLayoutModel : LayoutComponentBase, IDisposable
    {

        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected DisqusState DisqusState { get; set; }
        [Inject] GlobalSettingService GlobalSettingService { get; set; }
        protected GlobalSetting objglobalSettings = new GlobalSetting();
        protected private ElementReference disqusBody;

        protected override async Task OnInitializedAsync()
        {
            // Subscribe to the StateChanged EventHandler
            DisqusState.StateChanged +=
            OnDisqusStateStateChanged;

            await LoadGeneralSettingsAsync();
        }

        protected override async Task
             OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Convert.ToBoolean(objglobalSettings.DisqusEnabled))
                {
                    await DisqusInterop.CreateDisqus(
                        JSRuntime,
                        disqusBody,
                        @$"https://{objglobalSettings.DisqusShortName.Trim()}.disqus.com/embed.js");
                }
            }
        }

        protected async Task LoadGeneralSettingsAsync()
        {
            objglobalSettings = await GlobalSettingService.GetGeneralSettingsAsync();
        }

        // This method is fired when the DisqusState object
        // invokes its StateHasChanged() method
        // This will cause this control to invoke its own
        // StateHasChanged() method refreshing the page
        // and displaying the updated counter value
        void OnDisqusStateStateChanged(
            object sender, EventArgs e) => StateHasChanged();

        void IDisposable.Dispose()
        {
            // When this control is disposed of
            // unsubscribe from the StateChanged EventHandler
            DisqusState.StateChanged -=
            OnDisqusStateStateChanged;
        }
    }
}