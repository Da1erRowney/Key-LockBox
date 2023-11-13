namespace MauiApp2
{
    public partial class Setting : ContentPage
    {
        private DatabaseServiceUser _databaseService;
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        string CurrentUserPassword = SingUp.CurrentUserPassword;
        public static string statusSort;

        private DatabaseServiceUser databaseService; // ���������� ���������� ��� ���� ������

        public Setting()
        {
            InitializeComponent();
            sort.SelectedItem = statusSort;
            CheckHintsBasics();


        }

        [Obsolete]
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // �������� ��������� �������� IsAnimationPlaying ����� 3 �������
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    // �������� �������� IsAnimationPlaying �� True
                    gif.IsAnimationPlaying = true;
                });

                return false; // ���������� ������ ����� ������ ����������
            });
        }
        private void CheckHintsBasics()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            databaseService = new DatabaseServiceUser(databasePath); // ������������� ����������

            // �������� ���������� � ������� ������������
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsSetting == "NoN")
            {
                // ����������� �����������
                DisplayAlert("���������", "������������� ��� ������ ������� ����������� PIN-���. ��� ����� ������� �������� ������ ������� ������ -> ����������� ���� ������ � ������� ������. ", "OK");
                DisplayAlert("���������", "����� �� ������ �������� ������ ����� ������� ������, �������� PIN-��� ��� ����������� �����, ������� ���������������� ���� � ����� �� ��������.", "OK");
                // �������� �������� ���� HintsBasics � ���� ������
                currentUser.HintsSetting = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
        }
        private async void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            bool isDarkTheme = e.Value;
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (isDarkTheme)
            {
                currentUser.ThemeApplication = "Dark";

                databaseService.UpdateUser(currentUser);
                // ��������� ������ ����
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                // ��������� ������� ����
                currentUser.ThemeApplication = "Light";

                databaseService.UpdateUser(currentUser);
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

        private async void DeleteAccount(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("��������", "�� ������� ��� ������ ������� ��� �������?", "��", "���");

            if (result)
            {
                string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
                _databaseService = new DatabaseServiceUser(databasePath);
                _databaseService.DeleteUserByEmail(CurrentUserEmail);
                await Navigation.PushModalAsync(new MainPage());
            }
        }

        private async Task<bool> alert()
        {
            return await DisplayAlert("��������", "�� ������� ��� ������ ������� ��� �������?", "��", "���");
        }


        private async void informationPage(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync(new Information());
        }

        private void OnSortPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            statusSort = (string)sort.SelectedItem;
            currentUser.StatusSort = statusSort;
            databaseService.UpdateUser(currentUser);

        }

        
    }
}