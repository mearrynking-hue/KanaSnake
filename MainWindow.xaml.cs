using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KanaSnake;

public partial class MainWindow : Window
{
    //variables
    private KanaManager _kanaManager = new KanaManager();
    private List<Snake> _snake = new List<Snake>();
    private KanaPair _targetKana;
    private string _direction = "Right";

    public MainWindow()
    {
        InitializeComponent();

        StartGame();
    }

    //starting game
    private void StartGame()
    {
        _snake.Clear();
        _snake.Add(new Snake(100, 100));
        _targetKana = _kanaManager.GetRandomKana();
        TargetText.Text = _targetKana.Romaji;
        DrawSnake();
    }

    //drawing snake
    private void DrawSnake()
    {
        GameCanvas.Children.Clear();

        foreach(Snake part in _snake)
        {
            Rectangle rect = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Coral,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(rect, part.X);
            Canvas.SetRight(rect, part.Y);

            GameCanvas.Children.Add(rect);
        }
    }

    //moving snake

    
    //spawning kana


    //updating goal kana
}