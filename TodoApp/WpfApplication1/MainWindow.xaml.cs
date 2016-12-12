using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace WpfApplication1
{
    public class TodoItem
    {
        public string Text { get; set; }
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public bool IsCompleted { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var todoJson = await client.GetStringAsync("https://todo-app-fer.azurewebsites.net/todoapi/get");
                var todos = JsonConvert.DeserializeObject<List<TodoItem>>(todoJson);

                TodoListView.ItemsSource = todos.OrderByDescending(t => t.DateCreated);
            }
        }
    }
}
