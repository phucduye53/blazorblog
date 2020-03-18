using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using blazorblog.Controllers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Radzen;
using Radzen.Blazor;

namespace blazorblog.Pages
{
    public class FileManagerModel : ComponentBase
    {
        [Inject] IWebHostEnvironment HostEnvironment { get; set; }
        [Parameter] public EventCallback<string> ImageSelected { get; set; }

        protected RadzenDataList<FileObject> FileDataList;
        protected bool ShowFileManager = false;
        protected bool ShowProgressBarPopup = false;
        protected bool ShowFolderPopup = false;
        protected int progress;
        protected string info;

        protected string CurrentDirectory = "";
        protected string CurrentRoot = "";
        protected string NewFolderName = "";
        protected FileObject SelectedFile = new FileObject();
        protected List<string> Directories = new List<string>();
        protected List<FileObject> Files = new List<FileObject>();
        protected Dictionary<DateTime, string> events =
            new Dictionary<DateTime, string>();

        protected override void OnInitialized()
        {
            CurrentRoot =
                Path.Combine(HostEnvironment.WebRootPath, "uploads");
            CurrentDirectory = CurrentRoot;
            Directories.Add(CurrentDirectory);
            LoadFiles();
        }

        public void SetShowFileManager(bool paramSetting)
        {
            ShowFileManager = paramSetting;
        }

        //SelectFile

        protected async Task SelectFile()
        {
            await ImageSelected.InvokeAsync(SelectedFile.FilePath);
        }

        // Files

        protected void SelectImage(FileObject file)
        {
            if (SelectedFile.FileName == file.FileName)
            {
                SelectedFile = new FileObject();
            }
            else
            {
                SelectedFile = file;
            }
        }

        protected void LogChange(TreeEventArgs args)
        {
            // Get the current directory from the
            // argument passed to the method
            CurrentDirectory = args.Value as string;

            // Set the RadzenDataList to page 1
            FileDataList.FirstPage();

            // Reload the FileDataList
            LoadFiles();
        }

        protected string GetTextForNode(object data)
        {
            return Path.GetFileName((string)data);
        }

        protected RenderFragment<RadzenTreeItem>
            FileOrFolderTemplate = (context) => builder =>
            {
                string path = context.Value as string;
                bool isDirectory = Directory.Exists(path);

                builder.OpenComponent<RadzenIcon>(0);
                builder.AddAttribute(1,
                    "Icon", isDirectory ? "folder" :
                    "insert_drive_file");

                if (!isDirectory)
                {
                    builder.AddAttribute(2,
                        "Style",
                        "margin-left: 24px");
                }
                builder.CloseComponent();
                builder.AddContent(3, context.Text);
            };

        protected void LoadDirectory(TreeExpandEventArgs args)
        {
            CurrentDirectory = args.Value as string;

            // Only get the folders not the files
            args.Children.Data =
                Directory.EnumerateFileSystemEntries(CurrentDirectory)
                .Where(x => !x.Contains("."));

            args.Children.Text = GetTextForNode;
            args.Children.HasChildren =
                (path) => Directory.Exists((string)path);
            args.Children.Template = FileOrFolderTemplate;
        }

        protected void LoadFiles()
        {
            Files = new List<FileObject>();
            var FileNames =
                Directory.EnumerateFileSystemEntries(CurrentDirectory)
                .Where(x => x.Contains("."));

            foreach (var item in FileNames)
            {
                using (var image = System.Drawing.Image.FromFile(item))
                {
                    // Calculate Thumbnail
                    int thumbnailHeight = 100;
                    int thumbnailWidth = 100;
                    double x = image.Height / 100;
                    if (x > 0)
                    {
                        thumbnailHeight = Convert.ToInt32(image.Height / x);
                        thumbnailWidth = Convert.ToInt32(image.Width / x);
                    }

                    Files.Add(new FileObject()
                    {
                        FileName =
                        Path.GetFileName(item),
                        FilePath =
                        item.Replace(HostEnvironment.WebRootPath, ""),
                        Height = image.Height,
                        Width = image.Width,
                        ThumbnailHeight = Convert.ToInt32(thumbnailHeight),
                        ThumbnailWidth = Convert.ToInt32(thumbnailWidth)
                    });
                }
            }

            // Reset selected file
            SelectedFile = new FileObject();

            // Update UI
            StateHasChanged();
        }

        // Uploading

        protected async void OnProgress(UploadProgressArgs args)
        {
            ShowProgressBarPopup = true;
            this.info = $"{args.Loaded} of {args.Total} bytes.";
            this.progress = args.Progress;
            StateHasChanged();
            if (args.Loaded == args.Total)
            {
                // Delay to give files time to be saved
                // before reloading file view
                await LoadFilesAsync();
            }
        }

        protected async Task LoadFilesAsync()
        {
            int Time = 1;
            while (Time > 0)
            {
                Time--;
                StateHasChanged();
                await Task.Delay(1000);
            }
            ShowProgressBarPopup = false;
            LoadFiles();
        }

        // Deleteing

        protected void DeleteSelectedFile()
        {
            string RequestedPath = SelectedFile.FilePath;

            RequestedPath =
                RequestedPath.Replace("\\uploads\\", "uploads\\");

            string path = Path.Combine(
                HostEnvironment.WebRootPath,
                RequestedPath);

            if (File.Exists(path))
            {
                File.Delete(path);

                LoadFiles();
            }
        }

        // Folders

        protected void AddFolder()
        {
            ShowFolderPopup = true;
        }

        protected void CloseFolderPopup()
        {
            ShowFolderPopup = false;
        }

        protected void AddFolderName()
        {
            string path = Path.Combine(
                HostEnvironment.WebRootPath,
                CurrentDirectory,
                NewFolderName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                LoadFiles();
            }

            NewFolderName = "";
            ShowFolderPopup = false;
        }

        protected void FolderAction(RadzenSplitButtonItem item)
        {
            if (item != null)
            {
                if (item.Value == "Add")
                {
                    ShowFolderPopup = true;
                }
                // Delete
                if (item.Value == "Delete")
                {
                    string path = Path.Combine(
                        HostEnvironment.WebRootPath,
                        CurrentDirectory);

                    if (path.ToLower() != CurrentRoot.ToLower())
                    {
                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);

                            CurrentDirectory = Path.Combine(
                                HostEnvironment.WebRootPath,
                                "uploads");

                            LoadFiles();
                        }
                    }
                }
            }
        }
    }
}