using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace CookingRecipes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string basePath = @".\Assets\Рецепты";
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                var availableCategories = Directory.EnumerateDirectories(basePath);
                var namesList = new List<String>();

                foreach (var item in availableCategories) namesList.Add(new DirectoryInfo(item).Name);

                categoriesList.ItemsSource = namesList;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RecipeViewer.Visibility = Visibility.Collapsed;
        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var currentPath = Path.Combine(basePath, (sender as ListView).SelectedItem.ToString());
                var availableCategories = Directory.EnumerateDirectories(currentPath);
                var namesList = new List<Model.RecipesListItem>();

                foreach (var item in availableCategories)
                {
                    namesList.Add(new Model.RecipesListItem() {
                        RecipeName = new FileInfo(item).Name,
                        PicturePath = new FileInfo(Path.Combine(Path.Combine(basePath, (sender as ListView).SelectedItem.ToString()), new FileInfo(item).Name, "image.jpg")).FullName
                    });
                }

                catRecipesList.ItemsSource = namesList;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var currentPath = Path.Combine(basePath, categoriesList.SelectedItem.ToString(), (((FrameworkElement)e.OriginalSource).DataContext as Model.RecipesListItem).RecipeName);

                RecipeViewer.Visibility = Visibility.Visible;
                RecipeName.Text = (((FrameworkElement)e.OriginalSource).DataContext as Model.RecipesListItem).RecipeName;

                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(File.ReadAllText(Path.Combine(currentPath, "recipe.rtf"))));
                RecipeRichTextBox.Selection.Load(stream, DataFormats.Rtf);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
