namespace MauiApp2;
using PersonalsData;
using SQLite;

public partial class ChangeData : ContentPage
{
    private PersonalData selectedData;
    public ChangeData(PersonalData selectedData)
	{
		InitializeComponent();
        this.selectedData = selectedData;

        EntryName.Text =selectedData.Name;
        EntryLogin.Text =selectedData.Login;
        EntryEmail.Text =selectedData.Email;
        EntryPassword.Text =selectedData.Password;
        EntryOtherData.Text = selectedData.OtherData;
    }

    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
       await Navigation.PopModalAsync();
    }

    private void RenameData(object sender, EventArgs e)
    {
        DateTime currentDate = DateTime.UtcNow;
        DateTime newDate = currentDate.AddHours(+3);
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
        //string databasePath = @"C:\Users\����� ��������\source\repos\MauiApp2\MauiApp2\personalData.db";
        DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);

        // �������� ������ ���������� ��������
        selectedData.Name = EntryName.Text;
        selectedData.Login = EntryLogin.Text;
        selectedData.Email = EntryEmail.Text;
        selectedData.Password = EntryPassword.Text;
        selectedData.OtherData = EntryOtherData.Text;
        selectedData.LastModifiedDate = newDate.ToString("yyyy-MM-dd HH:mm:ss");

        // �������� ����� ���������� ������ � ���� ������
        databaseService.UpdatePersonalData(selectedData);

        // �������� ���������� � ����� ������
        databaseService.CloseConnection();

        // ��������� �� ���������� ��������
       
        Navigation.PushModalAsync(new BasicsPage());
    }

   
}