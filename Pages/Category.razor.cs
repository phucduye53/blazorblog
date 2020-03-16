using System;
using System.Threading;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Data.Dto;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace blazorblog.Pages
{
    public class CategoryModel : ComponentBase, IDisposable
    {
        [Inject]
        protected UserManager<User> _userManager { get; set; }
        [Inject]
        protected CategoryService _categoryService { get; set; }
        protected bool disposedValue = false; // To detect redundant calls
        protected readonly CancellationTokenSource cts = new CancellationTokenSource();
        [Parameter] public EventCallback<bool> CategoryUpdated { get; set; }

        public bool ShowManager = false;
        protected int CurrentPage = 1;


        protected IPagedEntities<CategoryDto> categories = new PagedEntities<CategoryDto>();

        protected CategoryDto SelectedCategory;
        protected bool ShowPopup = false;
        protected bool ShowPreviousButton { get { return (CurrentPage > 1); } }
        protected bool ShowNextButton { get { return ((categories.TotalSize > (CurrentPage * Constant.PageSize))); } }

        protected string strError;

        protected override async Task OnInitializedAsync()
        {
            categories = await _categoryService.GetCategoriesAsync(CurrentPage);
        }

        public void SetShowManager(bool paramSetting)
        {
            ShowManager = paramSetting;

            // Update UI
            StateHasChanged();
        }

        protected void CloseShowPopup()
        {
            // Close Popup
            ShowPopup = false;
        }

        protected void ClosePopup()
        {
            // Refresh collection on parent
            CategoryUpdated.InvokeAsync(true);

            // Close Popup
            ShowManager = false;
        }

        protected void AddNewCategory()
        {
            // Make new Category
            SelectedCategory = new CategoryDto();

            // Set Id to 0 so we know it is a new record
            SelectedCategory.CategoryId = 0;

            // Clear any error messages
            strError = "";
            // Open the Popup
            ShowPopup = true;
        }

        protected void EditCategory(CategoryDto Category)
        {
            // Set the selected Category
            // as the current Category
            SelectedCategory = Category;

            // Clear any error messages
            strError = "";

            // Open the Popup
            ShowPopup = true;
        }

        protected async Task SaveCategory()
        {
            try
            {
                // A new Category will have the Id set to 0
                if (SelectedCategory.CategoryId == 0)
                {
                    // Create new Category
                    CategoryDto objNewCategory = new CategoryDto();

                    objNewCategory.Name =
                        SelectedCategory.Name;


                    // Save the result
                    var result =
                    _categoryService.CreateCategoryAsync(objNewCategory);
                }
                else
                {
                    var result =
                    _categoryService.UpdateCategoryAsync(SelectedCategory);
                }

                // Get the Categorys
                categories = await _categoryService.GetCategoriesAsync(CurrentPage);

                // Close the Popup
                ShowPopup = false;
            }
            catch (Exception ex)
            {
                strError = ex.GetBaseException().Message;
            }
        }

        protected async Task DeleteCategory()
        {
            try
            {
                // Delete the Category
                var result = _categoryService.DeleteCategoryAsync(SelectedCategory);

                // If the current page has no records
                // and not on page one go back a page
                if ((CurrentPage > 1) &&
                    !((categories.PageSize - 1) > (CurrentPage * Constant.PageSize)))
                {
                    CurrentPage = CurrentPage - 1;
                }

                // Get the Categorys
                categories = await _categoryService.GetCategoriesAsync(CurrentPage);

                // Close the Popup
                ShowPopup = false;
            }
            catch (Exception ex)
            {
                strError = ex.GetBaseException().Message;
            }
        }

        protected async Task Previous()
        {
            if (CurrentPage > 1)
            {
                CurrentPage = CurrentPage - 1;
                categories = await _categoryService.GetCategoriesAsync(CurrentPage);
            }
        }

        protected async Task Next()
        {
            CurrentPage = CurrentPage + 1;
            categories = await _categoryService.GetCategoriesAsync(CurrentPage);
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