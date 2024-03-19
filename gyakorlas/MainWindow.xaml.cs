using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
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
using Microsoft.Data.Sqlite;


namespace gyakorlas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Adatok> listaAdatok = File.ReadAllLines("adatok-utf8.txt").Skip(1).Select( (x,i) => new Adatok($"{i+1};{x}")).ToList();
        SqliteConnection? connection;
        public MainWindow()
        {
            InitializeComponent();
            createTableButton.Click += (s, e) =>
            {
                resultTable.ItemsSource = "";
                connection = new("Filename=adatok.db");
                connection.Open();
                string createTable = "CREATE TABLE IF NOT EXISTS orszagok(id INTEGER PRIMARY KEY AUTOINCREMENT,nev VARCHAR(100), terulet INTEGER, nepesseg INTEGER, fovaros VARCHAR(100), fovarosNepesseg INTEGER)";
                SqliteCommand command = new(createTable,connection);
                command.ExecuteNonQuery();
                listaAdatok.ForEach(x => 
                {
                    string parancs = $"INSERT INTO orszagok(nev,terulet,nepesseg,fovaros,fovarosNepesseg) VALUES('{x.Orszag}','{x.Terulet}','{x.Nepesseg}','{x.Fovaros}','{x.FovarosNepessege}')";
                    command = new(parancs,connection);
                    command.ExecuteNonQuery();
                });
                connection.Close();
            };


            readToTable.Click += (s, e) =>
            {
                resultTable.ItemsSource = "";
                listaAdatok.Clear();
                connection = new("Filename=adatok.db");
                connection.Open();
                SqliteCommand command = new("SELECT * FROM orszagok",connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    listaAdatok.Add(new Adatok($"{reader.GetInt32(0)};{reader.GetString(1)};{reader.GetInt32(2)};{reader.GetInt32(3)};{reader.GetString(4)};{reader.GetInt32(5)}"));
                connection.Close();
                resultTable.ItemsSource = listaAdatok;
            };

            deleteButton.Click += (s, e) =>
            {
                connection = new("Filename=adatok.db");
                connection.Open();
                foreach (Adatok item in resultTable.SelectedItems)
                {
                    SqliteCommand torles = new($"DELETE FROM orszagok WHERE id={item.Id}",connection);
                    torles.ExecuteNonQuery();
                    listaAdatok.Remove(item);
                }
                resultTable.Items.Refresh();
                connection.Close();
            };

            btnLekerdezes1.Click += (s, e) =>
            {
                connection = new("Filename=adatok.db");
                connection.Open();
                SqliteCommand elso = new($"SELECT nev, nepesseg FROM orszagok ORDER BY orszagok.nepesseg DESC LIMIT 1",connection);
                elso.ExecuteNonQuery();
                SqliteDataReader oke = elso.ExecuteReader();
                string valasz = "";
                while (oke.Read())
                    valasz = $"{oke.GetString(0)};{oke.GetString(1)}";
                MessageBox.Show(valasz);
                connection.Close();
            };

            btnLekerdezes2.Click += (s, e) =>
            {
                connection = new("Filename=adatok.db");
                connection.Open();
                SqliteCommand masodik = new($"SELECT nev,terulet FROM orszagok ORDER BY orszagok.terulet LIMIT 1",connection);
                masodik.ExecuteNonQuery();
                SqliteDataReader oke2 = masodik.ExecuteReader();
                while (oke2.Read())
                    MessageBox.Show($"{oke2.GetString(0)};{oke2.GetString(1)}");
                connection.Close();
            };

            btnLekerdezes3.Click += (s, e) =>
            {
                double atlag = Math.Round(listaAdatok.Average(x => x.Terulet));
                listaAdatok.Clear();
                connection = new("Filename=adatok.db");
                connection.Open();
                SqliteCommand harmadik = new($"SELECT * FROM orszagok WHERE orszagok.terulet > {atlag}", connection);
                harmadik.ExecuteNonQuery();
                SqliteDataReader oke3 = harmadik.ExecuteReader();
                while(oke3.Read())
                    listaAdatok.Add(new Adatok($"{oke3.GetInt32(0)};{oke3.GetString(1)};{oke3.GetInt32(2)};{oke3.GetInt32(3)};{oke3.GetString(4)};{oke3.GetInt32(5)}"));
                resultTable.ItemsSource = listaAdatok;
                resultTable.Items.Refresh();
                connection.Close();
            };

            btnLekerdezes4.Click += (s, e) =>
            {
                connection = new("Filename=adatok.db");
                connection.Open();
                SqliteCommand negyedik = new($"SELECT COUNT(*) FROM orszagok WHERE orszagok.nepesseg > 100000",connection);
                negyedik.ExecuteNonQuery();
                SqliteDataReader oke4 = negyedik.ExecuteReader();
                while (oke4.Read())
                    MessageBox.Show($"{oke4.GetString(0)}");
                connection.Close();
            };
        }

        
    }
}
