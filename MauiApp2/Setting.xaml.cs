using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;



namespace MauiApp2
{
    public partial class Setting : ContentPage
    {
        public string CurrentUserEmail { get; set; }
        public string CurrentUserPassword { get; set; }
        //private ImageButton settingsButton; // ��������� ���� ��� �������� ������ �� SettingsBtn
        //public Setting(ImageButton settingsBtn)
        //{
        //    InitializeComponent();

        //    settingsButton = settingsBtn;
        //}

        public Setting()
        {
            InitializeComponent();
        }

        private async void OnSelectingThemeIndexChanged(object sender, EventArgs e)
        {
            if ((string)SelectingThemePicker.SelectedItem == "�������")
            {
                // ��������� ������� ����
                Application.Current.UserAppTheme = AppTheme.Light;

                // �������� �������� ����������� ��� ������� ����
               // settingsButton.Source = "setting8.png"; // ����������� ����������� ������ �� SettingsBtn
                await Navigation.PushModalAsync(new Setting());
            }
            else if ((string)SelectingThemePicker.SelectedItem == "������")
            {
                // ��������� ������ ����
                Application.Current.UserAppTheme = AppTheme.Dark;

                // �������� �������� ����������� ��� ������ ����
               // settingsButton.Source = "setting9.png"; // ����������� ����������� ������ �� SettingsBtn
                await Navigation.PushModalAsync(new Setting());
            }
            else
            {
                // ��������� ��������� ����
                Application.Current.UserAppTheme = AppTheme.Unspecified;
                await Navigation.PushModalAsync(new Setting());
            }
        }
        private async void ChangeAccountDetails(object sender, EventArgs e)
        {
            AccountConfirmation accountConfirmation = new AccountConfirmation();
            accountConfirmation.CurrentUserEmail = SingUp.CurrentUserEmail; // �������� �������� CurrentUserEmail
            accountConfirmation.CurrentUserPassword = SingUp.CurrentUserPassword; // �������� �������� CurrentUserPassword
            await Navigation.PushModalAsync(accountConfirmation);// ����������� changeAccountDetails ��� ���������

           // await Navigation.PushModalAsync(new AccountConfirmation());
        
        
        }

        private async void ExitAccount(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new MainPage());
        }

        private async void OnGoBackTapped(object sender, EventArgs e)
        {
            var basicsPage = new BasicsPage();
            await Navigation.PushModalAsync(basicsPage);
        }
    }
}