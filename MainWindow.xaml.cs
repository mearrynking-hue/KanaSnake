using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KanaSnake;

public partial class MainWindow : Window
{
    //variables
    private KanaManager _kanaManager = new KanaManager();
    private List<Snake> _snake = new List<Snake>();
    private KanaPair? _targetKana;
    private string _direction = "Right";

    public MainWindow()
    {
        InitializeComponent();
        this.KeyDown += OnKeyDown;
        StartGame();
    }

    //starting game
    private void StartGame()
    {
        _snake.Clear();
        _snake.Add(new Snake(100, 100));

        _targetKana = _kanaManager.GetRandomKana();
        TargetText.Text = _targetKana.Romaji;

        _gameTimer.Interval = TimeSpan.FromMilliseconds(150);
        _gameTimer.Tick -= GameTick;
        _gameTimer.Tick += GameTick;
        _gameTimer.Start(); 

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
            Canvas.SetTop(rect, part.Y);

            GameCanvas.Children.Add(rect);
        }
    }

    //timer
    private DispatcherTimer _gameTimer = new DispatcherTimer();

    private void GameTick(object? sender, EventArgs e)
    {
        Move();
        DrawSnake();
    }

    //moving snake
    private void Move()
    {
        Snake head = _snake[0];
        Snake newHead = new Snake(head.X, head.Y);

        switch(_direction)
        {
            case "Up": newHead.Y -= 20; break;
            case "Down": newHead.Y += 20; break;
            case "Left": newHead.X -= 20; break;
            case "Right": newHead.X += 20; break;
        }

        if(newHead.X < 0 || newHead.X >= 400 || newHead.Y < 0 || newHead.Y >= 400)
        {
            GameOver();
            return;            
        }

        _snake.Insert(0, newHead);
        _snake.RemoveAt(_snake.Count - 1);
    }

    //control(WASD)
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch(e.Key)
        {
            case Key.W: if (_direction != "Down") _direction = "Up"; break;
            case Key.S: if (_direction != "Up") _direction = "Down"; break;
            case Key.A: if (_direction != "Right") _direction = "Left"; break;
            case Key.D: if (_direction != "Left") _direction = "Right"; break;
        }
    }
    
    //spawning kana


    //updating goal kana


    //gameover
    private void GameOver()
    {
        _gameTimer.Stop();
        MessageBox.Show("Oh! You crashed!");
        StartGame();
    }
}