using System.Xml;
using PersonalsData;

namespace MauiApp2
{
    public partial class ViewData : ContentPage
    {
        private PersonalData selectedData; // ���������� ���������� ������
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        public ViewData(PersonalData selectedData)
        {
            InitializeComponent();
            CheckHintsBasics();
            // ��������� ��������� ������ � ���������� ������
            this.selectedData = selectedData;

            Name.Text = "��������: " + selectedData.Name;
            Login.Text = "�����: " + selectedData.Login;
            Email.Text = "�����: " + selectedData.Email;
            Password.Text = "������: " + selectedData.Password;
            OtherData.Text = "������ ������: " + selectedData.OtherData;
            DataCreation.Text = "���� ��������: " + selectedData.DateCreation;
            DataModification.Text = "���� ���������� ���������: " + selectedData.LastModifiedDate;
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
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);

            // �������� ���������� � ������� ������������
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsData == "NoN")
            {
                // ����������� �����������
                DisplayAlert("���������", "��� ��� ���� � ������ �������. �� ���������� ���� �� ����� ����������� ���������� ��� ������ ����� ������. �� ������ �������� ���� ������ ��� �� ���������� �� ������� ��� ����������� ��������������.", "OK");

                // �������� �������� ���� HintsBasics � ���� ������
                currentUser.HintsData = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
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