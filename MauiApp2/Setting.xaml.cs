using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;



namespace MauiApp2
{
    public partial class Setting : ContentPage
    {
        private DatabaseServiceUser _databaseService;
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        string CurrentUserPassword = SingUp.CurrentUserPassword;

        //private ImageButton settingsButton; // ��������� ���� ��� �������� ������ �� SettingsBtn
        //public Setting(ImageButton settingsBtn)
        //{
        //    InitializeComponent();

        //    settingsButton = settingsBtn;
        //}

        public Setting()
        {
            InitializeComponent();
            CheckHintsBasics();
        }
        private void CheckHintsBasics()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);

            // �������� ���������� � ������� ������������
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsSetting == "NoN")
            {
                // ����������� �����������
                DisplayAlert("���������", "������������� ��� ������ ������� ����������� PIN code. ��� ����� ������� �������� ������ ������� ������ -> ����������� ���� ������ � ������� ������. ", "OK");
                DisplayAlert("���������", "����� �� ������ �������� ������ ����� ������� ������, �������� PIN code ��� ����������� �����, ������� ���������������� ���� � ����� �� ��������.", "OK");
                // �������� �������� ���� HintsBasics � ���� ������
                currentUser.HintsSetting = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
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
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            _databaseService = new DatabaseServiceUser(databasePath);
            User user = _databaseService.GetUserByEmail(CurrentUserEmail);
            user.StatusAccount = "Off";
            _databaseService.UpdateUser(user);
            await Navigation.PushModalAsync(new MainPage());

        }

        private async void OnGoBackTapped(object sender, EventArgs e)
        {
            var basicsPage = new BasicsPage();
            await Navigation.PushModalAsync(basicsPage);
        }

       
    }
}