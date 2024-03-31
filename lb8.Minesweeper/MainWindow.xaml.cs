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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void Btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //получение значения лежащего в Tag
                int n = (int)((Button)sender).Tag;

                //установка фона нажатой кнопки, цвета и размера шрифта
                ((Button)sender).Background = Brushes.White;
                ((Button)sender).Foreground = Brushes.Red;
                ((Button)sender).FontSize = 12;

                //запись в нажатую кнопку её номера
                ((Button)sender).Content = n.ToString();
                ((Button)sender).IsEnabled = false;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                //создание и инициализация глобальной переменной для хранения изображения мины
                BitmapImage flag = new BitmapImage(new Uri(@"pack://application:,,,/pics/flag.png",
                UriKind.Absolute));

                //создание контейнера для хранения изображения
                Image img = new Image();
                //запись картинки с миной в контейнер
                img.Source = flag;

                //создание компонента для отображения изображения
                StackPanel stackPnl = new StackPanel();
                //установка толщины границ компонента
                stackPnl.Margin = new Thickness(0);
                //добавление контейнера с картинкой в компонент
                stackPnl.Children.Add(img);

                //запись компонента в кнопку
                ((Button)sender).Content = stackPnl;
            }
        }
        
        void FillUgrid(int lvl)
        {
            //указыается количество строк и столбцов в сетке
            GameGrid.Rows = lvl;
            GameGrid.Columns = lvl;

            //указываются размеры сетки (число ячеек * (размер кнопки в ячейки + толщина её границ))
            GameGrid.Width = lvl * (30 + 2);
            GameGrid.Height = lvl * (30 + 2);

            //толщина границ сетки
            GameGrid.Margin = new Thickness(5, 5, 5, 5);

            for (int i = 0; i < lvl * lvl; i++)
            {
                //создание кнопки
                Button btn = new Button();

                //запись номера кнопки
                btn.Tag = i;

                //установка размеров кнопки
                btn.Width = 30;
                btn.Height = 30;

                //текст на кнопке
                btn.Content = "";
                btn.IsEnabled = false;

                //толщина границ кнопки
                btn.Margin = new Thickness(0);

                btn.PreviewMouseDown += Btn_MouseDown;

                //добавление кнопки в сетку
                GameGrid.Children.Add(btn);
            }
        }
        void plantMines(int lvl)
        {
            int cell = lvl * lvl;
            int mines = lvl;
        }

        private void lvl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GameGrid.Children.Clear();

            int Lvl = (int)lvl.Value;
            FillUgrid(Lvl);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            foreach(Button btn in GameGrid.Children)
            {
                btn.IsEnabled = true;
            }

            lvl.IsEnabled = false;
            Confirm.IsEnabled = false;

            plantMines((int)lvl.Value);
        }
    }
}
