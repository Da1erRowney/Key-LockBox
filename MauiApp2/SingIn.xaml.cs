using System.Security.Cryptography;
using System.Text.RegularExpressions;
using MauiApp2;
using Org.W3c.Dom;
using SQLite;



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

        private async void OnCreateClicked(object sender, EventArgs e)
        {
            //string databasePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db");
            //await DisplayAlert("������", databasePath, "OK");

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

            var user = new User
            {
                Email = email,
                Password = password1,
                HintsBasics = "NoN",
                HintsSetting = "NoN",
                HintsData= "NoN",
                PinCode="NoN",
                StatusAccount = "Off"
            };
            
             _databaseService.InsertUser(user);

            await DisplayAlert("�����", "������������ ������� ������", "OK");
            await Navigation.PopModalAsync();
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
    }
}