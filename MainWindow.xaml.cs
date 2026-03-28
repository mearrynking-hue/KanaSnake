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
    private int _score = 0;
    private string _lastRomaji = "";

    public MainWindow()
    {
        InitializeComponent();
        this.KeyDown += OnKeyDown;

        StartGame();
    }

    //starting game
    private void StartGame()
    {
        _score = 0;
        ScoreText.Text = "0";

        _snake.Clear();
        _snake.Add(new Snake(90, 90));

        _targetKana = _kanaManager.GetRandomKana();
        _lastRomaji = _targetKana.Romaji;
        TargetText.Text = _targetKana.Romaji;

        _gameTimer.Interval = TimeSpan.FromMilliseconds(150);
        
        _gameTimer.Stop();
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
                Width = 30,
                Height = 30,
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
                FontSize = 26,
                Foreground = Brushes.White,
                Width = 30,
                Height = 30,
                TextAlignment = TextAlignment.Center
            };

            Canvas.SetLeft(textBlock, kana.X);
            Canvas.SetTop(textBlock, kana.Y - 4);
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
            case "Up": newHead.Y -= 30; break;
            case "Down": newHead.Y += 30; break;
            case "Left": newHead.X -= 30; break;
            case "Right": newHead.X += 30; break;
        }

        KanaPair? eatenKana = _onScreenKanas.FirstOrDefault(k => k.X == newHead.X && k.Y == newHead.Y);
        
        //check if we crashed into the wall
        if(newHead.X < 0 || newHead.X > 390 || newHead.Y < 0 || newHead.Y > 390)
                {
                    GameOverWall();
                    return;            
                }

        //check if we crashed into snake tail
        if(_snake.Any(s => s.X == newHead.X && s.Y == newHead.Y))
        {
            GameOverSelf();
            return;
        }

        //check if we eaten right kana
        if(eatenKana != null)
        {
            if(eatenKana.Romaji == _targetKana?.Romaji)
            {
                _onScreenKanas.Remove(eatenKana);

                _score++;
                ScoreText.Text = _score.ToString();

                KanaPair nextKana;
                do
                {
                    nextKana = _kanaManager.GetRandomKana();
                }while(nextKana.Romaji == _lastRomaji);

                _targetKana = nextKana;
                _lastRomaji = _targetKana.Romaji;

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

        //spawning kana
        if(_targetKana != null)
        {
            SetRandomPosition(_targetKana);
            _onScreenKanas.Add(_targetKana);
        }

        int totalKanasCount = _random.Next(3, 5);

        //spawning fake kana
        while(_onScreenKanas.Count < totalKanasCount)
        {
            KanaPair fake = _kanaManager.GetRandomKana();

            bool isDublicateText = _onScreenKanas.Any(k => k.Romaji == fake.Romaji);

            if(!isDublicateText)
            {
                SetRandomPosition(fake);
                _onScreenKanas.Add(fake);
            }
        }
    }

    //method which helps to find free place for kana
    private void SetRandomPosition(KanaPair kana)
    {
        bool isPositionOccupied;
        int newX, newY;

        do
        {
            isPositionOccupied = false;

            newX = _random.Next(0, 14) * 30;
            newY = _random.Next(0, 14) * 30;

            if(_onScreenKanas.Any(k => k.X == newX && k.Y == newY))
            {
                isPositionOccupied = true;
            }

            if(_snake.Any(s => s.X == newX && s.Y == newY))
            {
                isPositionOccupied = true;
            }

        } while(isPositionOccupied);

        kana.X = newX;
        kana.Y = newY;
    }

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

    //gameover if you crashe into yourself
    private void GameOverSelf()
    {
        _gameTimer.Stop();
        MessageBox.Show("Oh! Don't try to eat your tail!");
        StartGame();
    }
}