using PersonalsData;
using SQLite;
namespace MauiApp2;

public partial class AddPunct : ContentPage
{
    private DatabaseServicePersonalData _databaseService;
    public string CurrentUserEmail { get; set; }
    public SQLiteConnection CreateDatabase(string databasePath)
    {
        SQLiteConnection connection = new SQLiteConnection(databasePath);
        connection.CreateTable<PersonalData>(); // �������� ������� PersonalData
        return connection;
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
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
        //string databasePath = @"C:\Users\����� ��������\source\repos\MauiApp2\MauiApp2\personalData.db";
        _databaseService = new DatabaseServicePersonalData(databasePath);
        SQLiteConnection connection = CreateDatabase(databasePath);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _databaseService?.CloseConnection();
    }
    public AddPunct()
    {
        InitializeComponent();
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
        // string databasePath = @"C:\Users\����� ��������\source\repos\MauiApp2\MauiApp2\personalData.db";
        _databaseService = new DatabaseServicePersonalData(databasePath);
        SQLiteConnection connection = CreateDatabase(databasePath);


    }

    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new BasicsPage());
    }

    private async void AddData(object sender, EventArgs e)
    {

        string emailUser = SingUp.CurrentUserEmail;
        string name = EntryName.Text;
        string login = EntryLogin.Text;
        string email = EntryEmail.Text;
        string password = EntryPassword.Text;
        string otherData = EntryOtherData.Text;

        DateTime currentDate = DateTime.UtcNow;
        DateTime newDate = currentDate.AddHours(+3);
        if (name != null)
        {


            // �������� ���������� PersonalData �� ������ ��������� ������
            PersonalData personalData = new PersonalData
            {
                EmailUser = emailUser,
                Name = name,
                Email = email,
                Login = login,
                Password = password,
                OtherData = otherData,
                DateCreation = newDate.ToString("yyyy-MM-dd HH:mm:ss"),
                LastModifiedDate = "�� ���������"
            };

            // ���������� ������ � ���� ������
            _databaseService.InsertPersonalData(personalData);

            await DisplayAlert("�����", "������ ���������", "OK");
            await Navigation.PushModalAsync(new BasicsPage());

        }
        else
            await DisplayAlert("������", "��������� ���� ��������", "OK");
    }
}