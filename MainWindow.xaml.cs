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
    private List<KanaPair> _onScreenKanas = new List<KanaPair>();
    private Random _random = new Random();

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

        _targetKana = _kanaManager.GetRandomKana();
        TargetText.Text = _targetKana.Romaji;

        _gameTimer.Interval = TimeSpan.FromMilliseconds(150);
        _gameTimer.Tick -= GameTick;
        _gameTimer.Tick += GameTick;
        _gameTimer.Start(); 

        SpawnKanas();
        DrawSnake();
    }

    //drawing snake and kanas
    private void DrawSnake()
    {
        GameCanvas.Children.Clear();

        //drawing snake
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

        //drawing kana
        foreach(var kana in _onScreenKanas)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = kana.Kana,
                FontSize = 18,
                Foreground = Brushes.White
            };

            Canvas.SetLeft(textBlock, kana.X + 2);
            Canvas.SetTop(textBlock, kana.Y);
            GameCanvas.Children.Add(textBlock);
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

        KanaPair? eatenKana = _onScreenKanas.FirstOrDefault(k => k.X == newHead.X && k.Y == newHead.Y);

        if(eatenKana != null)
        {
            if(eatenKana.Romaji == _targetKana?.Romaji)
            {
                _onScreenKanas.Remove(eatenKana);

                _targetKana = _kanaManager.GetRandomKana();
                TargetText.Text = _targetKana.Romaji;
                SpawnKanas();

                _snake.Insert(0, newHead);
                return;
            }
            else
            {
                GameOverKana();
                return;
            }
        }

        if(newHead.X < 0 || newHead.X >= 400 || newHead.Y < 0 || newHead.Y >= 400)
        {
            GameOverWall();
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
    private void SpawnKanas()
    {
        _onScreenKanas.Clear();

        if(_targetKana != null)
        {
            _targetKana.X = _random.Next(0,20)*20;
            _targetKana.Y = _random.Next(0,20)*20;
            _onScreenKanas.Add(_targetKana);
        }

        for(int i=0; i<3; i++)
        {
            KanaPair fake = _kanaManager.GetRandomKana();

            if(fake.Romaji != _targetKana?.Romaji)
            {
                fake.X = _random.Next(0,20)*20;
                fake.Y = _random.Next(0,20)*20;
                _onScreenKanas.Add(fake);
            }
        }
    }

    //updating goal kana


    //gameover if you crashed in the wall
    private void GameOverWall()
    {
        _gameTimer.Stop();
        MessageBox.Show("Oh! You crashed!");
        StartGame();
    }

    //gameover if you eaten wrong kana
    private void GameOverKana()
    {
        _gameTimer.Stop();
        MessageBox.Show("Wrong kana!!");
        StartGame();
    }
}