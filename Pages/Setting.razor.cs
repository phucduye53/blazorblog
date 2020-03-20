using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Entity;
using blazorblog.Helpers;
using Blazored.TextEditor;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace blazorblog.Pages
{
    public class SettingModel : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject] IHttpContextAccessor httpContextAccessor {get;set;}
        [Inject] UserManager<User> _UserManager {get;set;}
        [Inject] RoleManager<IdentityRole> _RoleManager{get;set;}
        [Inject] GlobalSettingService _globalSettingService {get;set;}
        [Inject] EmailService _emailService {get;set;}
        [Inject] IToastService toastService  {get;set;}
        [Inject] DisqusState disqusState {get;set;}
        public System.Security.Claims.ClaimsPrincipal CurrentUser;


        protected GlobalSetting objGlobalSettings = new GlobalSetting();
        protected FileManager FileManagerControl;
        protected FileManager FileManagerApplicationHeaderControl;

        protected BlazoredTextEditor QuillHtml;
        protected bool RichTextEditorMode = true;

        protected List<string> OptionsTrueFalse = new List<string>() { "True", "False" };
        protected List<string> OptionsSMTP = new List<string>() { "Anonymous", "Basic", "NTLM" };

        protected string SMTPSecure = "";
        protected string SMTPAuthentication = "";
        protected string DisqusEnabled = "";

        protected override async Task OnInitializedAsync()
        {
            // Get the current user
            CurrentUser = (await authenticationStateTask).User;

       

            await LoadGeneralSettingsAsync();
            disqusState.SetDisplayDisqus(false);
        }

        protected override async Task
            OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var GeneralSettings = await _globalSettingService.GetGeneralSettingsAsync();
                await this.QuillHtml.LoadHTMLContent(GeneralSettings.ApplicationHeader);
            }
        }

        protected async Task LoadGeneralSettingsAsync()
        {
            objGlobalSettings = await _globalSettingService.GetGeneralSettingsAsync();

            SMTPSecure = objGlobalSettings.SMTPSecure.ToString();
            DisqusEnabled = objGlobalSettings.DisqusEnabled.ToString();

            switch (objGlobalSettings.SMTPAuthendication.Trim())
            {
                case "":
                case "0":
                    SMTPAuthentication = "Anonymous";
                    break;
                case "1":
                    SMTPAuthentication = "Basic";
                    break;
                case "2":
                    SMTPAuthentication = "NTLM";
                    break;
            }
        }

        protected void SelectLogo()
        {
            FileManagerControl.SetShowFileManager(true);
        }

        protected void InsertImage(string paramImageURL)
        {
            objGlobalSettings.ApplicationLogo = paramImageURL;

            FileManagerControl.SetShowFileManager(false);
        }

        protected async Task InsertApplicationHeaderImage(string paramImageURL)
        {
            await this.QuillHtml.InsertImage(paramImageURL);

            FileManagerApplicationHeaderControl.SetShowFileManager(false);
        }

        protected void InsertApplicationHeaderImageClick()
        {
            FileManagerApplicationHeaderControl.SetShowFileManager(true);
        }

        protected async Task RichTextEditor()
        {
            RichTextEditorMode = true;
            await this.QuillHtml.LoadHTMLContent(objGlobalSettings.ApplicationHeader);
        }

        protected async Task RawHTMLEditor()
        {
            RichTextEditorMode = false;
            objGlobalSettings.ApplicationHeader =
                await this.QuillHtml.GetHTML();
        }

        public async Task Save()
        {
            try
            {
                await SaveSettings();
                toastService.ShowSuccess("", "Saved!");
            }
            catch (Exception ex)
            {
                toastService.ShowSuccess("Error", ex.GetBaseException().Message);
            }
        }

        public async Task SaveSettings()
        {

            var SMTPSecureResult = _globalSettingService.UpdateSMTPSecureAsync(Convert.ToBoolean(SMTPSecure));

            var UpdateApplicationNameResult = _globalSettingService.UpdateApplicationNameAsync(objGlobalSettings.ApplicationName);
            var UpdateApplicationLogoResult = _globalSettingService.UpdateApplicationLogoAsync(objGlobalSettings.ApplicationLogo);
            var UpdateSMTPServerResult = _globalSettingService.UpdateSMTPServerAsync(objGlobalSettings.SMTPServer);
            var UpdateSMTPUserNameResult = _globalSettingService.UpdateSMTPUserNameAsync(objGlobalSettings.SMTPUserName);
            var UpdateSMTPPasswordResult = _globalSettingService.UpdateSMTPPasswordAsync(objGlobalSettings.SMTPPassword);
            var UpdateSMTPFromEmailResult = _globalSettingService.UpdateSMTPFromEmailAsync(objGlobalSettings.SMTPFromEmail);

            var UpdateDisqusEnabledResult = _globalSettingService.UpdateDisqusEnabledAsync(Convert.ToBoolean(DisqusEnabled));
            var UpdateDisqusShortNameResult = _globalSettingService.UpdateDisqusShortNameAsync(objGlobalSettings.DisqusShortName);

            if (RichTextEditorMode)
            {
                objGlobalSettings.ApplicationHeader =
                    await this.QuillHtml.GetHTML();
            }

            // If ApplicationHeader is essentially blank - - clear it
            if (objGlobalSettings.ApplicationHeader == @"<p><br></p>")
            {
                objGlobalSettings.ApplicationHeader = "";
            }

            var UpdateApplicationHeaderResult =
                _globalSettingService.UpdateApplicationHeaderAsync(objGlobalSettings.ApplicationHeader);

            switch (SMTPAuthentication)
            {
                case "Anonymous":
                    objGlobalSettings.SMTPAuthendication = "0";
                    break;
                case "Basic":
                    objGlobalSettings.SMTPAuthendication = "1";
                    break;
                case "NTLM":
                    objGlobalSettings.SMTPAuthendication = "2";
                    break;
            }

            var UpdateSMTPAuthendicationResult = _globalSettingService.UpdateSMTPAuthenticationAsync(objGlobalSettings.SMTPAuthendication);

            objGlobalSettings = await _globalSettingService.GetGeneralSettingsAsync();
        }

        public async Task SendTestEmail()
        {
            try
            {
                await SaveSettings();

                // Send Test Email
                string strError = await _emailService.SendMailAsync(
                     false,
                     objGlobalSettings.SMTPFromEmail,
                     "Blazor-Blogs Administrator",
                     "", "",
                     objGlobalSettings.SMTPFromEmail,
                     "Blazor-Blogs SMTP Test Email",
                     $"This is a Blazor-Blogs SMTP Test Email from: { httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}");

                if (strError == "")
                {
                    toastService.ShowSuccess("", "Test Email Sent");
                }
                else
                {
                    toastService.ShowError("", strError);
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError("", ex.GetBaseException().Message);
            }
        }
    }
}