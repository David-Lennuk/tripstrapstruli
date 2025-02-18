namespace Lumememm;

public partial class RGB_mudeliPage : ContentPage
{
    private Label lblPunane, lblRoheline, lblSinine;
    private Slider slPunane, slRoheline, slSinine;
    private Stepper stPunane, stRoheline, stSinine;
    private Button btn;
    private AbsoluteLayout abs;
    private Frame frame;
    private BoxView colorProgress;
    private bool isRandomColor;

    public RGB_mudeliPage(int k)
    {
        InitializeComponents();
        ConfigureLayout();
    }

    private void InitializeComponents()
    {
        lblPunane = CreateLabel("Punane: 0");
        lblRoheline = CreateLabel("Roheline: 0");
        lblSinine = CreateLabel("Sinine: 0");

        slPunane = CreateSlider(Color.FromArgb("#ff0000"));
        slRoheline = CreateSlider(Color.FromArgb("#00ff00"));
        slSinine = CreateSlider(Color.FromArgb("#0000ff"));

        stPunane = CreateStepper();
        stRoheline = CreateStepper();
        stSinine = CreateStepper();

        btn = new Button { Text = "Juhuslik värv" };
        btn.Clicked += Btn_Clicked;

        frame = new Frame
        {
            BackgroundColor = Color.FromRgb(0, 0, 0),
            HeightRequest = 200,
            WidthRequest = 200,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            CornerRadius = 20,
            Padding = 5
        };

        colorProgress = new BoxView
        {
            Color = Color.FromArgb("#575757"),
            HeightRequest = 10,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        abs = new AbsoluteLayout { Children = { lblPunane, lblRoheline, lblSinine, slPunane, slRoheline, slSinine, btn, frame, colorProgress } };

        slPunane.ValueChanged += Varv_ValueChanged;
        slRoheline.ValueChanged += Varv_ValueChanged;
        slSinine.ValueChanged += Varv_ValueChanged;
        stPunane.ValueChanged += Varv_ValueChanged;
        stRoheline.ValueChanged += Varv_ValueChanged;
        stSinine.ValueChanged += Varv_ValueChanged;
    }

    private void ConfigureLayout()
    {
        // Позиционирование элементов
        AbsoluteLayout.SetLayoutBounds(lblPunane, new Rect(10, 20, 300, 30));
        AbsoluteLayout.SetLayoutBounds(lblRoheline, new Rect(10, 60, 300, 30));
        AbsoluteLayout.SetLayoutBounds(lblSinine, new Rect(10, 100, 300, 30));

        AbsoluteLayout.SetLayoutBounds(slPunane, new Rect(10, 140, 300, 30));
        AbsoluteLayout.SetLayoutBounds(slRoheline, new Rect(10, 180, 300, 30));
        AbsoluteLayout.SetLayoutBounds(slSinine, new Rect(10, 220, 300, 30));

        AbsoluteLayout.SetLayoutBounds(stPunane, new Rect(320, 140, 50, 30));
        AbsoluteLayout.SetLayoutBounds(stRoheline, new Rect(320, 180, 50, 30));
        AbsoluteLayout.SetLayoutBounds(stSinine, new Rect(320, 220, 50, 30));

        AbsoluteLayout.SetLayoutBounds(btn, new Rect(10, 260, 360, 40));
        AbsoluteLayout.SetLayoutBounds(frame, new Rect(10, 310, 360, 220));
        AbsoluteLayout.SetLayoutBounds(colorProgress, new Rect(10, 540, 360, 10));

        Content = abs;
    }

    private Label CreateLabel(string text)
    {
        return new Label
        {
            BackgroundColor = Color.FromRgb(120, 144, 133),
            Text = text
        };
    }

    private Slider CreateSlider(Color thumbColor)
    {
        return new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 0,
            ThumbColor = thumbColor
        };
    }

    private Stepper CreateStepper()
    {
        return new Stepper
        {
            Minimum = 0,
            Maximum = 255,
            Increment = 1,
            Value = 0
        };
    }

    private void Varv_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        int punane = Convert.ToInt32(slPunane.Value);
        int roheline = Convert.ToInt32(slRoheline.Value);
        int sinine = Convert.ToInt32(slSinine.Value);

        lblPunane.Text = $"Punane: {punane}";
        lblRoheline.Text = $"Roheline: {roheline}";
        lblSinine.Text = $"Sinine: {sinine}";

        // Обновляем цвет фрейма
        var newColor = Color.FromRgb(punane, roheline, sinine);
        frame.BackgroundColor = newColor;

        // Обновляем прогресс
        colorProgress.Color = newColor;
    }

    private async void Btn_Clicked(object sender, EventArgs e)
    {
        if (isRandomColor)
        {
            btn.Text = "Juhuslik värv";
            isRandomColor = false;
        }
        else
        {
            Random rnd = new Random();
            int rndPunane = rnd.Next(0, 256);
            int rndRoheline = rnd.Next(0, 256);
            int rndSinine = rnd.Next(0, 256);

            slPunane.Value = rndPunane;
            slRoheline.Value = rndRoheline;
            slSinine.Value = rndSinine;

            stPunane.Value = rndPunane;
            stRoheline.Value = rndRoheline;
            stSinine.Value = rndSinine;

            frame.BackgroundColor = Color.FromRgb(rndPunane, rndSinine, rndRoheline);
            colorProgress.Color = Color.FromRgb(rndPunane, rndSinine, rndRoheline);

            btn.Text = "Värv muudetud!";
            isRandomColor = true;

            // Восстановить текст кнопки через 2 секунды
            await Task.Delay(2000);
            btn.Text = "Juhuslik värv";
            isRandomColor = false;
        }
    }
}
