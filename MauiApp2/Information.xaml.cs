namespace MauiApp2;

public partial class Information : ContentPage
{
    public Information()
    {
        InitializeComponent();
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
    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Setting());
    }
}