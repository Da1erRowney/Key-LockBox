using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PersonalsData;
using SQLite;

namespace MauiApp2
{
    public partial class BasicsPage : ContentPage, INotifyPropertyChanged
    {
        private List<PersonalData> _personalDataList;
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        public List<PersonalData> PersonalDataList
        {
            get { return _personalDataList; }
            set
            {
                _personalDataList = value;
                OnPropertyChanged(nameof(PersonalDataList));
            }
        }

        public BasicsPage()
        {
            InitializeComponent();
            InitializePersonalDataList();
            BindingContext = this;

            // �������� �������� ���� HintsBasics � ���� ������
            CheckHintsBasics();
        }

        private void InitializePersonalDataList()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
            DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);

            // �������� ��� ������������ ������ �� ���� ������
            List<PersonalData> allPersonalData = databaseService.GetAllPersonalData();

            // ������������ ������, ��������������� �������� ������������
            PersonalDataList = allPersonalData
                .Where(data => data.EmailUser == CurrentUserEmail)
                .OrderBy(data => data.Name)
                .ToList();

            databaseService.CloseConnection();
        }
        private void CheckHintsBasics()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);

            // �������� ���������� � ������� ������������
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsBasics == "NoN")
            {
                // ����������� �����������
                DisplayAlert("���������", "�� ������ �������� �� ������ ������� ����� ��� �������� ����� ������ � �� ����������� ��������. ��� �� ������ ����� ���� ������� ���������.", "OK");

                // �������� �������� ���� HintsBasics � ���� ������
                currentUser.HintsBasics = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
        }
        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddPunct());
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            //var settingPage = new Setting(SettingsButton);
            await Navigation.PushModalAsync(new Setting());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is PersonalData tappedData)
            {
                await Navigation.PushModalAsync(new ViewData(tappedData));
            }

            // �������� ��������� ��������
            PersonalDataListView.SelectedItem = null;
        }
    }
}