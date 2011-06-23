using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
using Steam4NET;

namespace SQCN
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dataFileName = "data.txt";

        private List<Person> textFile2PersonList(string fileName, string encoding="UTF-8")
        {
            StreamReader reader = null;
            try
            {
                List<Person> personList = new List<Person>();
                reader = new StreamReader(fileName, Encoding.GetEncoding(encoding));

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    Person person = new Person();
                    person.Name = line;
                    personList.Add(person);
                }

                reader.Close();
                return personList;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private bool personList2TextFile(List<Person> personList, string fileName)
        {
            StreamWriter writer = null;
            try
            {
                // 入力データ保存処理
                writer = new StreamWriter(fileName);

                personList.ForEach(delegate(Person p)
                {
                    writer.WriteLine(p.Name);
                });

                writer.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SteamContext.Initialize();
            
            List<Person> personList = new List<Person>();
            if (personList == null)
                MessageBox.Show("設定ファイル読み込みエラー: ");
            else
                DataContext = textFile2PersonList(dataFileName);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            List<Person> personList = (List<Person>)DataContext;
            if (!personList2TextFile(personList, dataFileName))
                MessageBox.Show("設定ファイル書き込みエラー");
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null || dataGrid.SelectedItem == CollectionView.NewItemPlaceholder)
            {
                MessageBox.Show("変更後の名前を選択してください");
                return;
            }

            Person person = (Person)dataGrid.SelectedItem;
            ChangePersonaName(person.Name, false);
        }
        
        private void allOk_Click(object sender, RoutedEventArgs e)
        {
            // すべて適用します
            List<Person> list = (List<Person>)DataContext;
            list.ForEach(delegate(Person p)
            {
                ChangePersonaName(p.Name);
            }
            );
        }

        private void ChangePersonaName(String newName, bool noticeBox=true)
        {
            SteamContext.ClientFriends.SetPersonaName(newName);
            System.Media.SystemSounds.Asterisk.Play();
            if(noticeBox)
                MessageBox.Show(newName + "に名前を変更しました");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // 履歴を消去します
            for (int i = 1; i <= 10; i++)
            {
                ChangePersonaName(i.ToString());
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}

