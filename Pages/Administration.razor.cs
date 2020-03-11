using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Data.Dto;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace blazorblog.Pages
{

    public class AdministrationModel : ComponentBase, IDisposable
    {


        [Inject]
        protected UserManager<User> _userManager { get; set; }
        [Inject]
        protected UserService _userService { get; set; }

        [Inject]
        protected RoleManager<IdentityRole> _roleManager { get; set; }

        [CascadingParameter]

        protected Task<AuthenticationState> authenticationStateTask { get; set; }
        protected bool disposedValue = false; // To detect redundant calls
        protected readonly CancellationTokenSource cts = new CancellationTokenSource();
   
        protected UserDto objUser = new UserDto();
        protected IPagedEntities<UserDto> ColUsers = new PagedEntities<UserDto>();

        public List<string> Options = new List<string>() { "Users", "Administrators" };
        public List<string> OptionsTrueFalse = new List<string>() { "True", "False" };
        System.Security.Claims.ClaimsPrincipal CurrentUser;
        protected string EmailConfirmed { get; set; } = "False";
        protected string NewsletterSubscriber { get; set; } = "False";
        protected string CurrentUserRole { get; set; } = "Users";
        protected string strError = "";
        protected string strSearch = "";
        protected bool ShowPopup = false;

        int CurrentPage = 1;
        protected bool ShowPreviousButton { get { return (CurrentPage > 1); } }
        protected bool ShowNextButton { get { return ((ColUsers.TotalSize > (CurrentPage * Constant.PageSize))); } }

        protected override async Task OnInitializedAsync()
        {
            // Get the current logged in user
            CurrentUser = (await authenticationStateTask).User;

            // Get the users
            await GetUsers();

            // DisqusState.SetDisplayDisqus(false);
        }

        public async Task GetUsers()
        {
            // clear any error messages
            strError = "";

            ColUsers = await _userService.GetUsersAsync(strSearch, CurrentPage);
        }

        protected void ClosePopup()
        {
            // Close the Popup
            ShowPopup = false;
        }

        protected void AddNewUser()
        {
            // Make new user
            objUser = new UserDto();
            objUser.PasswordHash = "*****";

            // Set Id to blank so we know it is a new record
            objUser.Id = "";

            // Open the Popup
            ShowPopup = true;
        }

        public async Task SaveUser()
        {
            try
            {
                // Is this an existing user?
                if (objUser.Id != "")
                {
                    // Get the user
                    var user = await _userManager.FindByIdAsync(objUser.Id);

                    // Update Email
                    user.Email = objUser.Email;

                    // Update DisplayName
                    user.UserName = objUser.UserName;

                    user.PhoneNumber = objUser.PhoneNumber;
                    // Update the user
                    await _userManager.UpdateAsync(user);


                     IdentityResult roleResult;
                    bool adminRoleExists = await _roleManager.RoleExistsAsync(Constant.ADMINISTRATION_ROLE);
                    if (!adminRoleExists)
                    {
                        roleResult = await _roleManager.CreateAsync(new IdentityRole(Constant.ADMINISTRATION_ROLE));
                    }


                    // Is user in administrator role?
                    var UserResult = await _userManager.IsInRoleAsync(user, Constant.ADMINISTRATION_ROLE);

                    // Is Administrator role selected but user is not an Administrator?
                    if ((CurrentUserRole == Constant.ADMINISTRATION_ROLE) & (!UserResult))
                    {
                        // Put admin in Administrator role
                         await _userManager.AddToRoleAsync(user, Constant.ADMINISTRATION_ROLE);
                    }
                    else
                    {
                        // Is Administrator role not selected but user is an Administrator?
                        if ((CurrentUserRole != Constant.ADMINISTRATION_ROLE) & (UserResult))
                        {
                            // Remove user from Administrator role
                            await _userManager.RemoveFromRoleAsync(user, Constant.ADMINISTRATION_ROLE);
                        }
                    }
                }

                // Close the Popup
                ShowPopup = false;

                // Refresh Users
                await GetUsers();
            }
            catch (Exception ex)
            {
                strError = ex.GetBaseException().Message;
            }
        }

        protected async Task EditUser(UserDto _IdentityUser)
        {
            // Set the selected user
            // as the current user
            objUser = _IdentityUser;

            // Get the user
            var user = await _userManager.FindByIdAsync(objUser.Id);

            if (user != null)
            {
                // Is user in administrator role?
                var UserResult = await _userManager.IsInRoleAsync(user, Constant.ADMINISTRATION_ROLE);

                if (UserResult)
                {
                    CurrentUserRole = Constant.ADMINISTRATION_ROLE;
                }
                else
                {
                    CurrentUserRole = Constant.USER_ROLE;
                }

            }

            // Open the Popup
            ShowPopup = true;
        }

        protected async Task DeleteUser()
        {
            // Close the Popup
            ShowPopup = false;

            // Get the user
            var user = await _userManager.FindByIdAsync(objUser.Id);
            if (user != null)
            {
                // Delete the user
                await _userManager.DeleteAsync(user);
            }

            // Refresh Users
            await GetUsers();
        }

        // Search

        protected async Task Search(string value, string name)
        {
            // Update search value
            strSearch = value;

            // Switch to page 1
            CurrentPage = 1;

            // Refresh Users
            await GetUsers();
        }

        // Paging

        protected async Task Previous()
        {
            if (CurrentPage > 1)
            {
                CurrentPage = CurrentPage - 1;
                // Refresh Users
                await GetUsers();
            }
        }

        protected async Task Next()
        {
            CurrentPage = CurrentPage + 1;
            // Refresh Users
            await GetUsers();
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