using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//
using System.Data.SQLite;
using Microsoft.Win32;


namespace tableScore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //имя базы данных
            bdNamePath = dlg.FileName;

            LogPlayers(Tab.Items);
        }

        public class PlayerInGrid
        {
            public string Name { get; set; }
            public int Score { get; set; }
        }

        string bdNamePath = "";
        
        public void LogPlayers(ItemCollection items)
        {
            {
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=" + bdNamePath + ";Version=3;");

                //открытие соединения с базой данных
                m_dbConnection.Open();

                //вывод учеников в лог
                string sqlShow = "SELECT Name, Score FROM TabScores";
                SQLiteCommand command = new SQLiteCommand(sqlShow, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //чтение строки из data
                    var data = new PlayerInGrid
                    {
                        Name = reader["Name"].ToString(),
                        Score = int.Parse(reader["Score"].ToString())
                    };
                    //добавление студента в DataGrid
                    items.Add(data);
                }
                //закрытие соединения с базой данных
                m_dbConnection.Close();
            }
        }

        private void revive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tab.Items.Clear();
            }
            finally
            {
                //Вывод списка студентов в лог
                LogPlayers(Tab.Items);
            }
        }
    }
}
