using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MauiApp2;
using SQLite;
using static System.Security.Cryptography.SHA256;


namespace MauiApp2
{
    public partial class SingIn : ContentPage
    {
        private DatabaseServiceUser _databaseService;

        public SQLiteConnection CreateDatabase(string databasePath)
        {
            SQLiteConnection connection = new SQLiteConnection(databasePath);
            connection.CreateTable<User>();
            return connection;
        }

        public SingIn()
        {
            InitializeComponent();
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            //string databasePath = @"C:\Users\����� ��������\source\repos\MauiApp2\MauiApp2\user.db";
            _databaseService = new DatabaseServiceUser(databasePath);
            SQLiteConnection connection = CreateDatabase(databasePath);


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
        private async void OnCreateClicked(object sender, EventArgs e)
        {
            string password1 = EntryPassword1.Text;
            string password2 = EntryPassword2.Text;
            string email = EntryMail.Text;
            email = email.ToLower();

            if (string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(password2) || string.IsNullOrEmpty(email))
            {
                await DisplayAlert("������", "�� ��� ���� ���������", "OK");
                return;
            }
          
            if (password1 != password2)
            {
                await DisplayAlert("������", "������ �� ���������", "OK");
                return;
            }
            
            if (password1.Length < 8 || !HasLetterAndDigit(password1))
            {
                await DisplayAlert("������", "������ ������ ��������� �� ����� 8 �������� � �������� � �����, � �����", "��");
                return;
            }
            
            if (!IsValidEmail(email))
            {
                await DisplayAlert("������", "������������ ������ ��������� ������", "OK");
                return;
            }
            
            if (_databaseService.GetUserByEmail(email) != null)
            {
                await DisplayAlert("������", "������������ � ����� ������ ��� ����������", "OK");
                return;
            }

            string salt = email.Split('@')[0];
            string hashedPassword = HashPassword(password1, salt);

            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                HintsBasics = "NoN",
                HintsSetting = "NoN",
                HintsData= "NoN",
                PinCode="NoN",
                StatusAccount = "Off",
                ThemeApplication = "Light"
            };
            
             _databaseService.InsertUser(user);

            await DisplayAlert("�����", "������������ ������� ������", "OK");
            await Navigation.PopModalAsync();
        }
        private string HashPassword(string password, string salt)
        {
            string saltedPassword = password + salt;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);

                byte[] hashedBytes = sha256.ComputeHash(bytes);

                string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                
                return hashedPassword;
            }
        }
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        private bool HasLetterAndDigit(string input)
        {
            return Regex.IsMatch(input, @"[a-zA-Z]") && Regex.IsMatch(input, @"\d");
        }

        private async void OnGoBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void OnGoBackTapped(object sender, TappedEventArgs e)
        {

        }
    }
}