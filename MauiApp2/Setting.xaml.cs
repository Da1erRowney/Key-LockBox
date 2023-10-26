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

        private async void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            bool isDarkTheme = e.Value;

            if (isDarkTheme)
            {
                // ��������� ������ ����
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                // ��������� ������� ����
                Application.Current.UserAppTheme = AppTheme.Light;
            }

            await Navigation.PushAsync(new Setting());
            Navigation.RemovePage(this);
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