namespace Lumememm;

public partial class StarPage : ContentPage
{
    public List<ContentPage> lehed = new List<ContentPage>() { new TicTacToePage() };
    public List<string> tekstid = new List<string> { "Trips" };

    ScrollView sv;
    VerticalStackLayout vsl;

    public StarPage()
    {
        Title = "Avaleht";
        vsl = new VerticalStackLayout { BackgroundColor = Color.FromArgb("#006600") };

        for (int i = 0; i < tekstid.Count; i++)
        {
            Button nupp = new Button
            {
                Text = tekstid[i],
                BackgroundColor = Color.FromArgb("#ffa31a"),
                TextColor = Color.FromArgb("#006600"),
                BorderWidth = 10,
                ZIndex = i,
                FontFamily = "Lovesive 400",
                FontSize = 30
            };

            vsl.Add(nupp);

            nupp.Clicked += Lehte_avamine;
        }

        sv = new ScrollView { Content = vsl };
        Content = sv;
    }

    private async void Lehte_avamine(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;

        await Navigation.PushAsync(lehed[btn.ZIndex]);
    }

    private async void Tagasi_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
