namespace MauiApp2;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MauiApp2;
using SQLite;
using static System.Security.Cryptography.SHA256;


public partial class ChangeAccountDetails : ContentPage
{

    private DatabaseServiceUser _databaseService;
    public string CurrentUserEmail { get; set; }
    public string CurrentUserPassword { get; set; }

    public ChangeAccountDetails()
    {
        InitializeComponent();
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
        //string databasePath = @"C:\Users\����� ��������\source\repos\MauiApp2\MauiApp2\user.db";

        _databaseService = new DatabaseServiceUser(databasePath);

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
        UpdateUserEntry();
    }
   
    private void UpdateUserEntry()
    {
        User user = _databaseService.GetUserByEmail(CurrentUserEmail);
        PasswordBtn.Text = CurrentUserPassword;
        EmailBtn.Text = CurrentUserEmail;
        PinCodeBtn.Text = user.PinCode;
    }


    private async void RenameData(object sender, EventArgs e)
    {
        string password = PasswordBtn.Text;
        string email = EmailBtn.Text;
        string pincode = PinCodeBtn.Text;

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pincode))
        {
            await DisplayAlert("������", "�� ��� ���� ���������", "OK");
            return;
        }

        Regex regex = new Regex("^[0-9]{4}$"); // ���������, ��� ���-��� ������� �� 4 ����

        if (!regex.IsMatch(pincode))
        {
            await DisplayAlert("������", "PIN-��� ������ ��������� ����� 4 �����.", "��");
            return;
        }
        if (password.Length < 8 || !HasLetterAndDigit(password))
        {
            await DisplayAlert("������", "������ ������ ��������� �� ����� 8 �������� � �������� � �����, � �����", "��");
            return;
        }
        else
        {
            User currentUser = _databaseService.GetUserByEmail(CurrentUserEmail);
            string salt = CurrentUserEmail.Split('@')[0];
            string hashedPassword = HashPassword(password, salt);

            // ���������� ������� � ������ ����������
            currentUser.Email = EmailBtn.Text;
            currentUser.Password = hashedPassword;
            currentUser.PinCode = PinCodeBtn.Text;
            // ���������� ��������� � ���� ������
            _databaseService.UpdateUser(currentUser);
            await DisplayAlert("�����", "������ ������� ���������, ����������� � ������� ��� �������� ��������", "��");
            var settingsPage = new Setting();
            await Navigation.PushModalAsync(settingsPage);
        }

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

    private bool HasLetterAndDigit(string input)
    {
        return Regex.IsMatch(input, @"[a-zA-Z]") && Regex.IsMatch(input, @"\d");
    }
    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        var settingsPage = new Setting();
        await Navigation.PushModalAsync(settingsPage);
    }

}