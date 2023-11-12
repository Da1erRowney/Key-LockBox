using Plugin.Fingerprint.Abstractions;

namespace MauiApp2;

public partial class ConfirmationPinCode : ContentPage
{
    private DatabaseServiceUser _databaseService;
    private readonly IFingerprint fingerprint;

    public ConfirmationPinCode(IFingerprint fingerprint)
    {
        InitializeComponent();
        this.fingerprint = fingerprint;
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
        base.OnAppearing();
        UpdateUserLabel();

    }


    private void UpdateUserLabel()
    {


        var userEmail = App.CurrentUserEmail;
        var labelText = $"������������ {userEmail}";
        UserLabel.Text = labelText;


    }
    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
        _databaseService = new DatabaseServiceUser(databasePath);
        User user = _databaseService.GetUserByEmail(App.CurrentUserEmail);
        user.StatusAccount = "Off";
        _databaseService.UpdateUser(user);
        await Navigation.PushModalAsync(new MainPage());

    }

    private async void Confirmation(object sender, EventArgs e)
    {
        string pincode = PinCodeBtn.Text;
        if (string.IsNullOrEmpty(pincode))
        {
            await DisplayAlert("������", "�� ��� ���� ���������", "OK");
            return;
        }
        if (pincode == App.CurrentUserPinCode)
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            _databaseService = new DatabaseServiceUser(databasePath);
            User user = _databaseService.GetUserByEmail(App.CurrentUserEmail);

            SingUp.CurrentUserEmail = App.CurrentUserEmail;
            SingUp.CurrentUserPassword = App.CurrentUserPassword;
            Setting.statusSort = user.StatusSort;
            await Navigation.PushModalAsync(new BasicsPage());

        }
        else
        {
            await DisplayAlert("������", "�� ��������� ������ PIN-���", "OK");
            return;
        }
    }

    private async void OnBiometricClicked(object sender, EventArgs e)
    {
        if (fingerprint != null)
        {
            var request = new AuthenticationRequestConfiguration("���������� ��������� ������", "��� ������� ��������� PIN-���.");
            var result = await fingerprint.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
                _databaseService = new DatabaseServiceUser(databasePath);
                User user = _databaseService.GetUserByEmail(App.CurrentUserEmail);

                SingUp.CurrentUserEmail = App.CurrentUserEmail;
                SingUp.CurrentUserPassword = App.CurrentUserPassword;
                Setting.statusSort = user.StatusSort;
                await Navigation.PushModalAsync(new BasicsPage());
            }
            else
            {
                await DisplayAlert("������", "��������� �� ���������", "OK");
            }
        }
        else
        {
            // ���������, ���� fingerprint ����� null
            await DisplayAlert("Error", "Fingerprint not initialized", "OK");
        }
    }
}