using Microsoft.Win32;
using Migration.Core;
using Migration.Core.Template;
using Migration.Core.Template.Bjsc;
using Migration.Core.Template.Java;
using Migration.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Migration.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            ramdom = new Random();
        }

        private Random ramdom;
        private System.Windows.Forms.WebBrowser javaBrowser;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AssemblyTreeView.SelectedItemChanged += AssemblyTreeView_SelectedItemChanged;
            AssemblyTreeView.CheckedStatusChanged += AssemblyTreeView_SelectedItemChanged;

            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            javaBrowser = new System.Windows.Forms.WebBrowser();
            javaBrowser.IsWebBrowserContextMenuEnabled = false;
            host.Child = javaBrowser;
            JavaTab.Content = host;
        }

        #region Commands
        private void FilePathTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowOpenFileDialog(FilePathTextBox, "类库或可执行文件|*.dll;*.exe");
        }
        private void FilePathTextButton_Click(object sender, RoutedEventArgs e)
        {
            ShowOpenFileDialog(FilePathTextBox, "类库或可执行文件|*.dll;*.exe");
        }
        private void XmlPathTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowOpenFileDialog(XmlPathTextBox, "对应的xml注释文件|*.xml");
        }
        private void XmlPathTextButton_Click(object sender, RoutedEventArgs e)
        {
            ShowOpenFileDialog(XmlPathTextBox, "对应的xml注释文件|*.xml");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="filter">图像文件(*.bmp, *.jpg) | *.bmp; *.jpg | 所有文件(*.*) | *.*</param>
        private void ShowOpenFileDialog(TextBox textBox, string filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = filter;

            var showDialogResult = dialog.ShowDialog();
            if (showDialogResult.HasValue && showDialogResult.Value)
            {
                textBox.Text = dialog.FileName;
            }
        }
        private void FilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (File.Exists(FilePathTextBox.Text) && FilePathTextBox.Text.ToLower().EndsWith(".dll"))
                {
                    var path = FilePathTextBox.Text;
                    var xmlPath = path.Substring(0, path.Length - 4) + ".xml";
                    if (File.Exists(xmlPath))
                    {
                        XmlPathTextBox.Text = xmlPath;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GenerateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(FilePathTextBox.Text);
        }

        private void GenerateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsEnabled = false;

            try
            {
                TransConfig transConfig = (TransConfig)FindResource("transConfig");

                var mainModel = AssemblyLoader.LoadAssembly(FilePathTextBox.Text, XmlPathTextBox.Text, transConfig);
                BindRefreshCheckedEvent(AssemblyTreeView, mainModel.Root);

                AssemblyTreeView.ItemsSource = new ObservableCollection<Node>(new List<Node> { mainModel.Root });

                AssemblyTreeView.SetRootAsCurrent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.IsEnabled = true;
        }

        private void AssemblyTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var root = e.NewValue as Node;

            if (root != null)
            {
                var time = DateTime.Now;
                var folderPath = System.IO.Path.Combine(Environment.CurrentDirectory, string.Format("migration\\{0:yyyyMMdd}", time));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                //Java
                var csharpTemplate = new JavaTemplate();
                csharpTemplate.Root = root;
                var javaContent = csharpTemplate.TransformText();
                JavaTextBox.Text = javaContent;

                //Java(Pretty)
                HtmlTemplate htmlTemplate = new HtmlTemplate();
                htmlTemplate.Code = javaContent;
                var htmlContet = htmlTemplate.TransformText();
                var fileName = string.Format("migration_{0:HH}_{0:mm}_{0:ss}_{1}.html", time, ramdom.Next(100000, 999999));
                var htmlPath = System.IO.Path.Combine(folderPath, fileName);
                File.WriteAllText(htmlPath, htmlContet, Encoding.UTF8);
                javaBrowser.Url = new Uri(htmlPath);

                //Bjsc
                var bjscTemplate = new BjscTemplate();
                bjscTemplate.WithInclude = true;
                bjscTemplate.Root = root;
                var bjscContent = bjscTemplate.TransformText();
                BjscTextBox.Text = bjscContent;
            }
        }
        #endregion

        private void BindRefreshCheckedEvent(Controls.CTreeView cTreeView, Node root)
        {
            root.CheckedStatusChanged += (s, e) =>
            {
                cTreeView.OnRefreshCheckedStatus(cTreeView.SelectedItem);
            };
            if (root.Items != null && root.Items.Count > 0)
            {
                foreach (var item in root.Items)
                {
                    BindRefreshCheckedEvent(cTreeView, item);
                }
            }
        }
    }
}
