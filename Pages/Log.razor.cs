using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace blazorblog.Pages
{
    public class LogModel : ComponentBase
    {
        [Inject] IHttpContextAccessor httpContextAccessor { get; set; }

        [Inject] UserManager<User> _UserManager { get; set; }
        [Inject] RoleManager<IdentityRole> _RoleManager { get; set; }
        [Inject] LogService LogService { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] IConfiguration _configuration { get; set; }
        [Inject] DisqusState DisqusState { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        public System.Security.Claims.ClaimsPrincipal CurrentUser;

        protected string strError;
        public bool ConFirmDeletePopup = false;
        int CurrentPage = 1;
        protected IPagedEntities<blazorblog.Entity.Log> logs = new PagedEntities<blazorblog.Entity.Log>();

        protected bool ShowNextButton
        {
            get
            {
                return (
                    (logs.TotalSize > (CurrentPage * Constant.PageSize))
                    );
            }
        }

        protected override async Task OnInitializedAsync()
        {
            // Get the current user
            CurrentUser = (await authenticationStateTask).User;

            logs = await LogService.GetLogsAsync(CurrentPage);
            DisqusState.SetDisplayDisqus(false);
        }

        protected async Task Previous()
        {
            if (CurrentPage > 1)
            {
                CurrentPage = CurrentPage - 1;
                logs = await LogService.GetLogsAsync(CurrentPage);
            }
        }

        protected async Task Next()
        {
            CurrentPage = CurrentPage + 1;
            logs = await LogService.GetLogsAsync(CurrentPage);
        }

        protected void DeleteLogs()
        {
            ConFirmDeletePopup = true;
        }

        protected void DeleteNo()
        {
            ConFirmDeletePopup = false;
        }

        protected async Task DeleteYes()
        {
            // try
            // {
            //     // Delete the Logs
            //     var result = await LogService.DelteLogsAsync(CurrentUser.Identity.Name);

            //     CurrentPage = 1;
            //     LogService = await LogService.GetLogsAsync(CurrentPage);

            //     ConFirmDeletePopup = false;
            // }
            // catch (Exception ex)
            // {
            //     strError = ex.GetBaseException().Message;
            // }
        }
    }
}