using PersonalsData;
using System.ComponentModel;
using System.Text.RegularExpressions;



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
        private async void InitializePersonalDataList()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
            DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);


            List<PersonalData> allPersonalData = databaseService.GetAllPersonalData();

            foreach (var data in allPersonalData)
            {
                string nameicon = data.Name;
                nameicon = nameicon.ToLower();
                nameicon = Regex.Replace(nameicon, @"[\p{P}-[.]]+", "");
                nameicon = Regex.Replace(nameicon, " ", "");
                string nameicontranc = string.Empty;
                // Replace "��" with "x"
                if (nameicon == "����������")
                {
                    nameicon = "microsoft";
                }

                if (nameicon == "����")
                {
                    nameicon = "chrome";
                }

                if (nameicon == "tictok")
                {
                    nameicon = "tiktok";
                }

                if (nameicon == "����")
                {
                    nameicon = "steam";
                }

                if (nameicon == "�������")
                {
                    nameicon = "ubisoft";
                }

                if (nameicon == "��" || nameicon == "���������")
                {
                    nameicon = "vk";
                }

                if (nameicon == "����")
                {
                    nameicon = "youtube";
                }

                if (nameicon == "���" || nameicon == "����������" || nameicon == "����������")
                {
                    nameicon = "aliexpress";
                }


                nameicon = Regex.Replace(nameicon, @"��", "x");

                // Replace "��" in the middle with "i"
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})��(?=\p{IsCyrillic})", "i");

                // Replace "��" at the end with "y"
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})��$", "y");
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})��$", "ay");
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})��$", "ay");

                foreach (char c in nameicon)
                {
                    nameicontranc += TransliterateCharacter(c);
                }

                string TransliterateCharacter(char c)
                {
                    switch (c)
                    {
                        case '�': return "a";
                        case '�': return "b";
                        case '�': return "v";
                        case '�': return "g";
                        case '�': return "d";
                        case '�': return "e";
                        case '�': return "yo";
                        case '�': return "zh";
                        case '�': return "z";
                        case '�': return "i";
                        case '�': return "y";
                        case '�': return "k";
                        case '�': return "l";
                        case '�': return "m";
                        case '�': return "n";
                        case '�': return "o";
                        case '�': return "p";
                        case '�': return "r";
                        case '�': return "s";
                        case '�': return "t";
                        case '�': return "u";
                        case '�': return "f";
                        case '�': return "kh";
                        case '�': return "ts";
                        case '�': return "ch";
                        case '�': return "sh";
                        case '�': return "sch";
                        case '�': return "'";
                        case '�': return "y";
                        case '�': return "'";
                        case '�': return "e";
                        case '�': return "yu";
                        case '�': return "ya";
                        default: return c.ToString();
                    }
                }


                var icon = new string[]
                {
                    "dota2",
                    "google",
                    "googleplay",
                    "instagram",
                    "microsoft",
                    "nvidia",
                    "odnoklassniki",
                    "opera",
                    "pinterest",
                    "spotify",
                    "steam",
                    "telegram",
                    "tiktok",
                    "viber",
                    "visa",
                    "vk",
                    "whatsapp",
                    "youtube",
                    "discord",
                    "twitter",
                    "pornhub",
                    "mobilelegends",
                    "genshinimpact",
                    "github",
                    "drweb",
                    "delphi",
                    "chrome",
                    "blender",
                    "css",
                    "javascript",
                    "ea",
                    "onedrive",
                    "edge",
                    "roblox",
                    "totalcommander",
                    "shazam",
                    "appleappstore",
                    "snapchat",
                    "facebook",
                    "kfc",
                    "lamoda",
                    "oz",
                    "onliner",
                    "kufar",
                    "burgerking",
                    "epicgames",
                    "faceit",
                    "blizzard",
                    "soundcloud",
                    "ubisoft",
                    "rockstargames",
                    "aliexpress",
                    "yandex",
                    "oplati"
                };

                if (icon.Contains(nameicontranc))
                {
                    data.IconUrl = $"{nameicontranc}.png";
                }
                else
                {


                    data.IconUrl = "noticon.png";
                }
            }

            if (Setting.statusSort == "�� ��������")
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.Name)
                    .ToList();
            }
            else if (Setting.statusSort == "�� ������")
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.Login)
                    .ToList();
            }
            else if (Setting.statusSort == "�� ���� ��������")
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.DateCreation)
                    .ToList();
            }
            else
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.Name)
                    .ToList();
            }
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


            PersonalDataListView.SelectedItem = null;
        }

        private void OnSettingsClicked(object sender, TappedEventArgs e)
        {

        }
    }


}