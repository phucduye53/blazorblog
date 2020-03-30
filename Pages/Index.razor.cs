using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Data.Dto;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace blazorblog.Pages
{
    public class IndexModel : ComponentBase, IDisposable
    {
        [Inject] protected UserManager<User> _userManager { get; set; }
        [Inject] protected CategoryService _categoryService { get; set; }
        [Inject] protected BlogService _blogService { get; set; }
        [Inject] protected RoleManager<IdentityRole> _roleManager { get; set; }
        [Inject] protected DisqusState DisqusState { get; set; }
        [Inject] protected BlogSearchState BlogSearchState { get; set; }

        [Inject] protected LogService LogService { get; set; }
        [Inject] IHttpContextAccessor httpContextAccessor { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected List<CategoryDto> categories = new List<CategoryDto>();

        protected IPagedEntities<BlogDto> blogs = new PagedEntities<BlogDto>();

        protected bool ShowPreviousButton
        {
            get
            {
                return (BlogSearchState.CurrentPage > 1);
            }
        }

        protected bool ShowNextButton
        {
            get
            {
                return (
                    (blogs.TotalSize > (BlogSearchState.CurrentPage * Constant.PageSize))
                    );
            }
        }
        protected bool UserIsAnAdmin = false;
        protected System.Security.Claims.ClaimsPrincipal CurrentUser;
        protected BlogAdministrationModel BlogAdministrationControl;
        protected bool disposedValue = false; // To detect redundant calls
        protected readonly CancellationTokenSource cts = new CancellationTokenSource();
        protected override async Task OnInitializedAsync()
        {
            if (BlogSearchState.CurrentPage == 0)
            {
                BlogSearchState.CurrentPage = 1;
            }

            if (BlogSearchState.CurrentCategoryID == 0)
            {
                BlogSearchState.CurrentCategoryID = 0;
            }

            // ensure there is a ADMINISTRATION_ROLE
            var RoleResult = await _roleManager.FindByNameAsync(Constant.ADMINISTRATION_ROLE);
            if (RoleResult == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(Constant.ADMINISTRATION_ROLE));
            }






            // Get the current user
            CurrentUser = (await authenticationStateTask).User;

            if (CurrentUser.Identity.IsAuthenticated)
            {
                var Current_User = await _userManager.FindByNameAsync(CurrentUser.Identity.Name);
                // Is user an administrator?
                UserIsAnAdmin = await _userManager.IsInRoleAsync(Current_User, Constant.ADMINISTRATION_ROLE);
            }

            categories = await _categoryService.GetCategorysAsync();

            // We access BlogsService using @Service
            await BlogUpdatedEvent();

            DisqusState.SetDisplayDisqus(false);
        }

        protected void NewBlog()
        {
            BlogAdministrationControl.AddNewBlog();
        }


        protected async Task BlogUpdatedEvent()
        {
            using (new PerfTimerLogger("GetBlogsAsync", (CurrentUser.Identity.Name != null) ? CurrentUser.Identity.Name : "", LogService, httpContextAccessor))
            {
                blogs = await _blogService.GetBlogsAsync(new BlogInputDto()
                {
                    page = BlogSearchState.CurrentPage,
                    CategoryId = Convert.ToInt32(BlogSearchState.CurrentCategoryID)
                });
            }
        }

        protected async Task ChangeCategory(object value, string name)
        {
            BlogSearchState.CurrentPage = 1;
            BlogSearchState.CurrentCategoryID = (value == null) ? 0 : Convert.ToInt32(value);
            await BlogUpdatedEvent();
        }



        protected async Task Previous()
        {
            if (BlogSearchState.CurrentPage > 1)
            {
                BlogSearchState.CurrentPage = BlogSearchState.CurrentPage - 1;
                await BlogUpdatedEvent();
            }
        }

        protected async Task Next()
        {
            BlogSearchState.CurrentPage = BlogSearchState.CurrentPage + 1;
            await BlogUpdatedEvent();
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