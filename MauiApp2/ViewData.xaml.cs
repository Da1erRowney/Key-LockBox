using System.Xml;
using PersonalsData;

namespace MauiApp2
{
    public partial class ViewData : ContentPage
    {
        private PersonalData selectedData; // ���������� ���������� ������

        public ViewData(PersonalData selectedData)
        {
            InitializeComponent();

            // ��������� ��������� ������ � ���������� ������
            this.selectedData = selectedData;

            Name.Text = "��������: " + selectedData.Name;
            Login.Text = "�����: " + selectedData.Login;
            Email.Text = "�����: " + selectedData.Email;
            Password.Text = "������: " + selectedData.Password;
            OtherData.Text = "������ ������: " + selectedData.OtherData;
        }

        private async void OnGoBackTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void DeleteData(object sender, EventArgs e)
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
           // string databasePath = @"C:\Users\����� ��������\source\repos\MauiApp2\MauiApp2\personalData.db";
            DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);

            // ������� ��������� ������ �� ���� ������
            databaseService.DeletePersonalData(selectedData);

            // �������� ���������� � ����� ������
            databaseService.CloseConnection();

            // ��������� �� ���������� ��������
            Navigation.PushModalAsync(new BasicsPage());
        }



        private void RenamesData(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ChangeData(selectedData));
        }
    }
}