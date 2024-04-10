using Minesweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
//
using System.Data.SQLite;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace lb8.Minesweeper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Tick;

            bdPath();
        }

        string bdNamePath;
        string PlayerName;

        int[,] Map;
        int Allmines;
        int countEmptyCells;

        DispatcherTimer timer = new DispatcherTimer();
        int Seconds;
        void Tick(object sender, EventArgs e)
        {
            Seconds++;

            TimeSpan curTime = new TimeSpan(0, 0, Seconds);

            time.Content = string.Format("{0:hh}:{0:mm}:{0:ss}", curTime);
        }

        //События игры
        void flag(object sender)
        {
            //создание и инициализация глобальной переменной для хранения изображения мины
            BitmapImage flag = new BitmapImage(new Uri(@"pack://application:,,,/pics/flag.png",
            UriKind.Absolute));

            //создание контейнера для хранения изображения
            Image img = new Image();
            //запись картинки с флагом в контейнер
            img.Source = flag;

            //создание компонента для отображения изображения
            StackPanel stackPnl = new StackPanel();
            //установка толщины границ компонента
            stackPnl.Margin = new Thickness(0);
            //добавление контейнера с картинкой в компонент
            stackPnl.Children.Add(img);

            //запись компонента в кнопку
            ((MyButton)sender).Content = stackPnl;
        }
        void playing(object sender, MouseButtonEventArgs e)
        {
            //установка фона нажатой кнопки, цвета и размера шрифта
            ((MyButton)sender).Background = Brushes.White;
            ((MyButton)sender).FontSize = 14;

            //запись в нажатую кнопку колличество мин
            int x = (int)((MyButton)sender).x;
            int y = (int)((MyButton)sender).y;

            //События возникающие если клетка содержит определенное значение
            if (Map[x, y] == 0)
            {
                //Рекурсивный алгоритм для открытия всех пустых клеток
                foreach (MyButton b in GameGrid.Children)
                {
                    //Проверка соседних клеток от выбранной
                    if(((Math.Abs(x - b.x) == 1) && (Math.Abs(y - b.y) == 0)) || ((Math.Abs(x - b.x) == 0) && (Math.Abs(y - b.y) == 1)) || ((Math.Abs(x - b.x) == 1) && (Math.Abs(y - b.y) == 1)))
                    {
                        //Если клетка еще не открыта
                        if(b.Background != Brushes.White)
                        {
                            //Если это не бомба - открыть
                            if (Map[b.x, b.y] != 9)
                                Btn_Click(b, e);
                        }
                    }
                }
            }
            if (Map[x, y] == 1)
            {
                ((MyButton)sender).Content = Map[x, y];
                ((MyButton)sender).Foreground = Brushes.Blue;
                ((MyButton)sender).FontWeight = FontWeights.Bold;

            }
            if (Map[x, y] == 2)
            {
                ((MyButton)sender).Content = Map[x, y];
                ((MyButton)sender).Foreground = Brushes.Green;
                ((MyButton)sender).FontWeight = FontWeights.Bold;

            }
            if (Map[x, y] == 3)
            {
                ((MyButton)sender).Content = Map[x, y];
                ((MyButton)sender).Foreground = Brushes.Red;
                ((MyButton)sender).FontWeight = FontWeights.Bold;

            }
            if (Map[x, y] == 4)
            {
                ((MyButton)sender).Content = Map[x, y];
                ((MyButton)sender).Foreground = Brushes.Purple;
                ((MyButton)sender).FontWeight = FontWeights.Bold;

            }
            if (Map[x, y] == 5)
            {
                ((MyButton)sender).Content = Map[x, y];
                ((MyButton)sender).Foreground = Brushes.DarkRed;
                ((MyButton)sender).FontWeight = FontWeights.Bold;

            }
            if (Map[x, y] == 6)
            {
                ((MyButton)sender).Content = Map[x, y];
                ((MyButton)sender).Foreground = Brushes.SkyBlue;
                ((MyButton)sender).FontWeight = FontWeights.Bold;

            }
            if (Map[x, y] == 7)
            {
                ((MyButton)sender).Content = Map[x, y];
                ((MyButton)sender).Foreground = Brushes.Black;
                ((MyButton)sender).FontWeight = FontWeights.Bold;

            }
            ((MyButton)sender).IsEnabled = false;
        }
        void loseGame()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\1\Desktop\вуз\1.2\pogi\done\lab8.Minesweeper\lb8.Minesweeper\lb8.Minesweeper\music\fart-with-reverb.wav");
            player.Play();

            timer.Stop();
            //создание и инициализация глобальной переменной для хранения изображения мины
            BitmapImage mine = new BitmapImage(new Uri(@"pack://application:,,,/pics/mine.ico",
            UriKind.Absolute));

            //создание контейнера для хранения изображения
            Image img = new Image();
            //запись картинки с миной в контейнер
            img.Source = mine;

            //создание компонента для отображения изображения
            StackPanel stackPnl = new StackPanel();
            //установка толщины границ компонента
            stackPnl.Margin = new Thickness(0);
            //добавление контейнера с картинкой в компонент
            stackPnl.Children.Add(img);

            //Показываем где еще были мины на поле
            foreach (MyButton btn in GameGrid.Children)
            {
                if (Map[btn.x, btn.y] == 9)
                {
                    btn.Background = Brushes.Red;

                    btn.Content = stackPnl;
                }
            }

            MessageBox.Show("Вы проиграли");
        }
        void restart()
        {
            //Перезапуск игры и установка начальных параметров

            timer.Stop();
            Seconds = 0;
            Allmines = 0;

            //Визуал
            time.Content = "00:00:00";
            lvl.IsEnabled = true;
            Confirm.IsEnabled = true;
            foreach (MyButton btn in GameGrid.Children)
            {
                btn.IsEnabled = false;
            }
            try
            {
                GameGrid.Children.Clear();
            }
            finally
            {
                int Lvl = (int)lvl.Value;
                FillUgrid(Lvl);
            }
            Restart.IsEnabled = false;
        }
        

        //Ввод игрока
        private void Btn_Click(object sender, MouseButtonEventArgs e)
        {
            //Собите на левую кнопку
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Проверка - помечена ли клетка флажком
                if (((MyButton)sender).isFlagged != true)
                {
                    //Если нет, то берутся координаты из кнопки и сверяются с координатами "Map" в массиве
                    int x = ((MyButton)sender).x;
                    int y = ((MyButton)sender).y;

                    //Если попали на бомбу, то проигрываем
                    if (Map[x, y] == 9)
                    {
                        loseGame();

                        restart();
                    }
                    else
                    {
                        //Если не попали на бомбу, то открываем значение клетки
                        playing(sender, e);

                        //Проверка открыли ли мы все клетки без мин. Пока не открыли будем вычитать их колличество 
                        if (countEmptyCells - 1 != 0)
                            countEmptyCells--;
                        //Если открыты все клетки без мин, то - победа!
                        else
                        {
                            //Остановка таймера и запись результата в БД
                            timer.Stop();
                            int score = Seconds;
                            BD_AddScore(score);

                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\1\Desktop\вуз\1.2\pogi\done\lab8.Minesweeper\lb8.Minesweeper\lb8.Minesweeper\music\vi ka.wav");
                            player.Play();

                            //Cообщение о победе+времени и рестарт игры
                            MessageBox.Show($" Вы выйграли! \n Ваше время: {score} c.");

                            restart();
                        }
                    }
                }
            }
            //Собите на правую кнопку
            if (e.RightButton == MouseButtonState.Pressed)
            {
                //Если флажка нет
                if (((MyButton)sender).isFlagged != true)
                {
                    //Cтавим флажок
                    ((MyButton)sender).isFlagged = true;
                    flag(sender);

                    //Вычитаем общее колличество мин на поле
                    Allmines--;
                    countMines.Content = "Мин: " + Allmines;
                }
                else//Cнимаем флажок
                {
                    
                    ((MyButton)sender).isFlagged = false;
                    ((MyButton)sender).Content = "";

                    //Добавляем колличество мин, т.к. убрали лишний флажок
                    Allmines++;
                    countMines.Content = "Мин: " + Allmines;
                }
            }
        }

        //Генерирование поля
        public void FillUgrid(int lvl)
        {
            //Функция заполнения поля с кнопками

            //указыается количество строк и столбцов в сетке
            GameGrid.Rows = lvl;
            GameGrid.Columns = lvl;

            //указываются размеры сетки (число ячеек * (размер кнопки в ячейки + толщина её границ))
            GameGrid.Width = lvl * (30 + 2);
            GameGrid.Height = lvl * (30 + 2);

            //толщина границ сетки
            GameGrid.Margin = new Thickness(5, 5, 5, 5);

            for (int x = 0; x < lvl; x++)
            {
                for (int y = 0; y < lvl; y++)
                {
                    //создание кнопки
                    MyButton btn = new MyButton();

                    //запись кординат кнопки
                    btn.x = x;
                    btn.y = y;

                    //установка размеров кнопки
                    btn.Width = 30;
                    btn.Height = 30;

                    //текст на кнопке
                    btn.Content = "";
                    btn.IsEnabled = false;

                    //толщина границ кнопки
                    btn.Margin = new Thickness(0);

                    //Вызываемое событие по нажатию кнопки
                    btn.PreviewMouseDown += Btn_Click;

                    //добавление кнопки в сетку
                    GameGrid.Children.Add(btn);
                }

            }
        }
        public bool checkEmpty()
        {
            //Функция для проверки не стоят ли бомбы вокруг одной пустой клетки

            //Перебор всего массива 
            for (int x = 0; x < Map.GetLength(0); x++)
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    //Если нашли бомбу
                    if (Map[x, y] == 9)
                    {
                        bool res = false;
                        
                        //Получение коорд границ поиска по x и y
                        int l = x - 1;
                        if (l < 0) l = 0;
                        int r = x + 1;
                        if (r >= Map.GetLength(0)) r = Map.GetLength(0) - 1;

                        int u = y - 1;
                        if (u < 0) u = 0;
                        int d = y + 1;
                        if (d >= Map.GetLength(1)) d = Map.GetLength(1) - 1;
                        
                        //Поиск пустой клетки
                        for (int x1 = l; x1 <= r; x1++)
                            for (int y1 = u; y1 <= d; y1++)
                            {
                                if (Map[x1, y1] == 0)
                                {
                                    res = true;
                                    break;
                                }
                            }
                        if (res == false) return false;
                    }

                }
            return true;
        }
        public void plantMines(int lvl)
        {
            /*Алгоритм работы функции будет выглядеть следующим образом:
        1. Выбрать в пределах поля случайную ячейку. 
        2. Проверить, что в этой ячейке ещё нет мины (если есть, вернуться к шагу 1)
        3. Проверить, что рядом с этой ячейкой есть хотя бы одна пустая (если нет, вернуться к шагу 1)
        4. Разместить в выбранной ячейке мину. (записать в массив 9)*/

            //Установка всего колличества мин
            
            int mines = lvl * 2;
            //int mines = 1;
            Allmines = mines;

            ///////////////ЧЕРЕЗ МАССИВ///////////////////////////////////
            //Создаем пустой массив поля
            Map = new int[lvl, lvl];

            for (int i = 0; i < lvl; i++)
                for (int d = 0; d < lvl; d++)
                {
                    Map[i, d] = 0;
                }

            //Расставляем мины в пустой массив
            Random randomArr = new Random();
            while (mines != 0)
            {
                //Выбираем случайные координаты клетки
                int x = randomArr.Next(lvl);
                int y = randomArr.Next(lvl);

                //Если она пуста
                if (Map[x, y] == 0)
                {
                    //Ставим мину
                    Map[x, y] = 9;

                    //Если та клетка в которую мы поставили мину, создает ситуацию - 8 мин вокруг пустой клетки
                    if (checkEmpty() == false)
                    {
                        //Убираем эту мину
                        Map[x, y] = 0;
                        continue;
                    }
                    mines--;
                }
            }

            ///////////////ЧЕРЕЗ СЛОВАРЬ///////////////////////////////////
            
            //int cells = lvl * lvl;

            /*for (int i = 0; i < cells; i++)
            {
                map.Add(i, 0);
            }

            Random randomMap = new Random();
            while (mines != 0)
            {
                int indexCell = randomMap.Next(0, cells);
                if (map[cell] == 0)
                {
                    map[cell] = 9;
                    mines--;
                }
            }*/

        }
        public void justNumbers(int lvl)
        {
            /*Функция подсчёта мин в соседних ячейках будет иметь следующую структуру:
            1. Для каждой ячейки поля. (перебор координат ячеек по i и по d)
            2. Если в ячейке нет мины. (значение массива не равно 9)
            3. Объявить переменную счётчик равную 0:
                * Перебрать 8 соседних ячеек:
                    *  1(i - 1, d - 1), 2(i, d - 1), 3(i + 1, d - 1), 4(i - 1, d), 5(i + 1, d), 6(i - 1, d + 1), 7(i, d + 1), 8 (i - 1, d + 1);
                * Если в ячейке встречается мина (значение массива 9), увеличить счётчик.
            4. Записать в ячейку значение счётчика.*/


            for (int x = 0; x < lvl; x++)
                for (int y = 0; y < lvl; y++)
                {
                    if (Map[x, y] == 0)
                    {
                        //Получение границ для перебора
                        int l = x - 1;
                        if (l < 0) l = 0;
                        int r = x + 1;
                        if (r >= lvl) r = lvl - 1;

                        int u = y - 1;
                        if (u < 0) u = 0;
                        int d = y + 1;
                        if (d >= lvl) d = lvl - 1;

                        //Колличество мин
                        int count = 0;

                        //Перебор соседних клеток
                        for (int x1 = l; x1 <= r; x1++)
                            for (int y1 = u; y1 <= d; y1++)
                            {
                                if (Map[x1, y1] == 9) count++;
                            }
                        //Колличество мин рядом с клеткой
                        Map[x, y] = count++;
                    }
                }
        }

        //Установка уровня
        private void lvl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                //Удаляем старое поле, которое выбрали ранее
                GameGrid.Children.Clear();
            }
            finally
            {
                //Выбор уровня сложности
                int Lvl = (int)lvl.Value;
                countMines.Content = "Мин: " + Lvl * 2;
                //Заполнение кнопками поля
                FillUgrid(Lvl);
            }
        }

        //Подтверждение выбранных параметров
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //Визуальные компоненты
            foreach(Button btn in GameGrid.Children)
            {
                btn.IsEnabled = true;
            }
            lvl.IsEnabled = false;
            Confirm.IsEnabled = false;
            Restart.IsEnabled = true;

            //Расставляем мины
            plantMines((int)lvl.Value);

            //Расставляем значения
            justNumbers((int)lvl.Value);

            //Считаем клетки без мин
            countEmptyCells = ((int)lvl.Value * (int)lvl.Value) - Allmines;

            //Запускаем таймер
            timer.Start();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            restart();
        }

        //Путь к бд
        void bdPath()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //имя базы данных
            bdNamePath = dlg.FileName;
        }
        //Добавление счета в бд
        public void BD_AddScore(int score)
        {
            //подключаемся к базе
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=" + bdNamePath + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();

            PlayerName = uName.Text;
            string PName = PlayerName;

            string sqlInsert = "INSERT INTO TabScores (Name, Score) VALUES ('" + PName + "', '" + score + "')";
            SQLiteCommand commandInsert = new SQLiteCommand(sqlInsert, m_dbConnection);
            commandInsert.ExecuteReader();

            m_dbConnection.Close();
        }
    }
}