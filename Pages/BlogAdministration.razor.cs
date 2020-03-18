using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Data.Dto;
using blazorblog.Entity;
using Blazored.TextEditor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace blazorblog.Pages
{
    public class BlogAdministrationModel : ComponentBase, IDisposable
    {
        [Inject]
        protected UserManager<User> _userManager { get; set; }
        [Inject]
        protected CategoryService _categoryService { get; set; }
        [Inject]
        protected BlogService _blogService { get; set; }
        [Inject]
        protected RoleManager<IdentityRole> _roleManager { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Parameter] public EventCallback<bool> BlogUpdated { get; set; }
        protected bool disposedValue = false; // To detect redundant calls
        protected readonly CancellationTokenSource cts = new CancellationTokenSource();

        protected BlogDto SelectedBlog;
        public bool ShowAdmin = false;
        public bool ConFirmDeletePopup = false;

        protected System.Security.Claims.ClaimsPrincipal CurrentUser;
        protected CategoryModel CategoryManagerControl;
        protected FileManagerModel FileManagerControl;
        protected BlazoredTextEditor QuillHtmlSummary;
        protected BlazoredTextEditor QuillHtml;
        protected bool RichTextEditorMode = true;
        protected bool RichTextEditorModeSummary = true;

        protected List<CategoryDto> categories = new List<CategoryDto>();
        protected IEnumerable<int> selectedBlogCategories = new int[] { };
        protected string strError;

        protected override async Task OnInitializedAsync()
        {
            // Get the current user
            CurrentUser = (await authenticationStateTask).User;

            categories = await _categoryService.GetCategorysAsync();
        }

        //CategoryManagerControl

        protected void OpenCategoryManagerControl()
        {
            // Open CategoryManagerControl
            CategoryManagerControl.SetShowManager(true);
        }

        protected void ClosePopup()
        {
            // Close the Popup
            ShowAdmin = false;

            // Refresh collection on parent
            BlogUpdated.InvokeAsync(true);
        }

        public void AddNewBlog()
        {
            // Make new Blog
            SelectedBlog = new BlogDto();
            // Set the default date
            SelectedBlog.CreatedDate = DateTime.Now;
            // Set Id to 0 so we know it is a new record
            SelectedBlog.Id = 0;
            // Set the selected Blog Categorys
            selectedBlogCategories = new int[] { };
            // Clear any error messages
            strError = "";
            // Open the Popup
            ShowAdmin = true;
        }

        public void EditBlog(BlogDto Blog)
        {
            // Set the selected Blog
            // as the current Blog
            SelectedBlog = Blog;

            // Set the selected Blog Categorys
            List<int> BlogCatagories = SelectedBlog.Categories.Select(x => x.CategoryId).ToList();
            selectedBlogCategories = BlogCatagories.ToArray<int>();

            // Clear any error messages
            strError = "";

            // Open the Popup
            ShowAdmin = true;
        }

        protected async Task SaveBlog()
        {
            try
            {
                // A new Blog will have the Id set to 0
                if (SelectedBlog.Id == 0)
                {
                    // Create new Blog
                    BlogDto objNewBlog = new BlogDto();

                    objNewBlog.CreatedDate =
                        SelectedBlog.CreatedDate;

                    objNewBlog.Title =
                        SelectedBlog.Title;

                    if (RichTextEditorModeSummary)
                    {
                        objNewBlog.Summary =
                            await this.QuillHtmlSummary.GetHTML();
                    }
                    else
                    {
                        objNewBlog.Summary =
                            SelectedBlog.Summary;
                    }

                    if (RichTextEditorMode)
                    {
                        objNewBlog.Content =
                            await this.QuillHtml.GetHTML();
                    }
                    else
                    {
                        objNewBlog.Content =
                            SelectedBlog.Content;
                    }

                    objNewBlog.UserName =
                        CurrentUser.Identity.Name;

                    // Save the result
                    var result =
                    _blogService.CreateBlogAsync(objNewBlog, selectedBlogCategories);

                    // Log
                    // await LogAction($"Create Blog #{objNewBlog.Id}");
                }
                else
                {
                    // Get HTML Content

                    if (RichTextEditorModeSummary)
                    {
                        SelectedBlog.Summary =
                            await this.QuillHtmlSummary.GetHTML();
                    }

                    if (RichTextEditorMode)
                    {
                        SelectedBlog.Content =
                            await this.QuillHtml.GetHTML();
                    }

                    var result =
                    _blogService.UpdateBlogAsync(SelectedBlog, selectedBlogCategories);

                    // Log
                    // await LogAction($"Update Blog #{SelectedBlog.Id}");
                }

                ClosePopup();
            }
            catch (Exception ex)
            {
                strError = ex.GetBaseException().Message;
            }
        }

        protected void DeleteBlog()
        {
            ConFirmDeletePopup = true;
        }

        protected void DeleteNo()
        {
            ConFirmDeletePopup = false;
        }

        protected async Task DeleteYes()
        {
            try
            {
                int BlogId = SelectedBlog.Id;

                // Delete the Blog
                var result = await _blogService.DeleteBlogAsync(BlogId);

                // Log
                // await LogAction($"Delete Blog #{BlogId}");

                ClosePopup();
            }
            catch (Exception ex)
            {
                strError = ex.GetBaseException().Message;
            }
        }

        //Summary

        protected void RichTextEditorSummary()
        {
            RichTextEditorModeSummary = true;
            StateHasChanged();
        }

        protected async Task RawHTMLEditorSummary()
        {
            RichTextEditorModeSummary = false;
            SelectedBlog.Summary =
                await this.QuillHtmlSummary.GetHTML();
        }

        protected void RichTextEditor()
        {
            RichTextEditorMode = true;
            StateHasChanged();
        }

        protected async Task RawHTMLEditor()
        {
            RichTextEditorMode = false;
            SelectedBlog.Content =
                await this.QuillHtml.GetHTML();
        }

        // Inserting Images


        protected async Task UpdateCategories()
        {
            categories = await _categoryService.GetCategorysAsync();
        }
        protected async Task InsertImage(string paramImageURL)
        {
            await this.QuillHtml.InsertImage(paramImageURL);
            FileManagerControl.SetShowFileManager(false);
        }

        protected void InsertImageClick()
        {
            FileManagerControl.SetShowFileManager(true);
        }
        // private async Task LogAction(string strAction)
        // {
        //     // Get the current user
        //     var CurrentUser = (await authenticationStateTask).User;

        //     BlazorBlogs.Data.Models.Logs objLog = new Data.Models.Logs();
        //     objLog.LogDate = DateTime.Now;
        //     objLog.LogAction = strAction;
        //     objLog.LogUserName = (CurrentUser.Identity != null) ? CurrentUser.Identity.Name : "";
        //     objLog.LogIpaddress = httpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();

        //     var result = await @Service.CreateLogAsync(objLog);
        // }
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