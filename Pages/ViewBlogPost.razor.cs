using System;
using System.Threading;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Data.Dto;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbelt.Blazor.HeadElement;
using static Toolbelt.Blazor.HeadElement.MetaElement;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Http;

namespace blazorblog.Pages
{
    public class ViewBlogPostModel : ComponentBase, IDisposable
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IHeadElementHelper HeadElementHelper { get; set; }
        [Inject] GlobalSettingService GlobalSettingService { get; set; }
        [Inject] BlogService _blogService { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] DisqusState DisqusState { get; set; }
        [Inject] LogService LogService { get; set; }
        [Inject] IHttpContextAccessor httpContextAccessor {get;set;}
        [Parameter] public string NormalizeTitle { get; set; }

        protected BlogDto SelectedBlog = new BlogDto() { Id = 0 };

        protected GlobalSetting objGeneralSettings = new GlobalSetting();
        protected bool disposedValue = false; // To detect redundant calls
        protected readonly CancellationTokenSource cts = new CancellationTokenSource();
        protected BlogAdministration BlogAdministrationControl;
        protected System.Security.Claims.ClaimsPrincipal CurrentUser;
        protected bool UserIsAdminOfBlogPost = false;
        protected string AbsoluteUrlOfThisPage => NavigationManager.ToAbsoluteUri($"/ViewBlogPost/{NormalizeTitle}").AbsoluteUri;

        protected override async Task OnInitializedAsync()
        {
            try
            {


                // await LogAction($"View Blog #{SelectedBlog.BlogId}");
                objGeneralSettings = await GlobalSettingService.GetGeneralSettingsAsync();
                // Get the current user
                CurrentUser = (await authenticationStateTask).User;

                if (CurrentUser.Identity.IsAuthenticated)
                {
                    if (CurrentUser.Identity.Name.ToLower() == SelectedBlog.UserName.ToLower())
                    {
                        UserIsAdminOfBlogPost = true;
                    }
                }
                using (new PerfTimerLogger("GetBlogAsync",(CurrentUser.Identity.Name != null) ? CurrentUser.Identity.Name : "",LogService,httpContextAccessor))
                {
                    SelectedBlog = await _blogService.GetBlogAsync(NormalizeTitle);
                }
            
                await HeadElementHelper.SetMetaElementsAsync(
                ByProp("og:description", SelectedBlog.Title),
                ByName("description", SelectedBlog.Title)
                );
            }
            catch
            {
                // await LogAction($"Error Viewing Blog #{BlogPostId}");
                SelectedBlog = new BlogDto() { CreatedDate = DateTime.Now, Title = "ERROR - Page Not Found" };
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Convert.ToBoolean(objGeneralSettings.DisqusEnabled))
                {
                    string url = NavigationManager.ToAbsoluteUri($"/ViewBlogPost/{NormalizeTitle}").AbsoluteUri;

                    await DisqusInterop.ResetDisqus(
                        JSRuntime,
                        NormalizeTitle.ToString(),
                        url,
                        SelectedBlog.Title);

                    DisqusState.SetDisplayDisqus(true);
                }
            }
        }

        protected void EditBlog()
        {
            BlogAdministrationControl.EditBlog(SelectedBlog);
        }

        protected void Back()
        {
            NavigationManager.NavigateTo("/");
        }

        protected async Task BlogUpdatedEvent()
        {
            try
            {
                SelectedBlog = await _blogService.GetBlogAsync(NormalizeTitle);
            }
            catch
            {
                // Blog was deleted
                Back();
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cts.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}