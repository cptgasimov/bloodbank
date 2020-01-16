using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Window window2, adminPanel;
        public bool isWindow2Closed = true, isEditable = false;
        public OleDbConnection conn;
        public TextBox usrBox, searchBox, name, surname, address, phoneNumber, bloodType;
        public TextBlock listBoxHeader;
        public PasswordBox oldPassBox, nwPassBox, ConfirmPassBox;
        public StackPanel mainStackPanel;
        public ListBox listBox;
        public static int counter = 0;
        public DataTable donorTable;
        public ComboBox comboBox;
        public TabControl tabControl;
        public string query = "";
        public List<string> initDonor = new List<string>();

        public MainWindow()
        {
            InitializeComponent();          
            
            string ConnStr = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source=" + System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() +"\\BloodBank.mdb";

            conn = new OleDbConnection(ConnStr);    
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string senderName = ((Window)sender).Name;

            if (senderName.Equals("about"))
            {
                if (e.ChangedButton == MouseButton.Left)
                    window2.DragMove();
            }
            else if (senderName.Equals("adminPanel"))
            {
                if (e.ChangedButton == MouseButton.Left)
                    adminPanel.DragMove();

                mainStackPanel.Focus();
            }
            else
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();

                //if we click on main window, the value of topMost property of window2 will be false
                if (window2 != null)
                    window2.Topmost = false;

                //when we click anywhere on the window, and if the TextBox or PasswordBox had a focus, they will lose it
                mainWindow.Focus();
            }                                  
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            string senderName = ((Button)sender).Name;

            if (senderName.Equals("minimizeAdminPanel"))
            {
                adminPanel.WindowState = WindowState.Minimized;
            }
            else
            {
                this.WindowState = WindowState.Minimized;
            }            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            string senderName = ((Button)sender).Name;

            if (senderName.Equals("closeAdminPanel"))
            {
                    if ((oldPassBox.Password != "" || nwPassBox.Password != "" || ConfirmPassBox.Password != "") || (name.Text != "" || surname.Text != "" || address.Text != "" || phoneNumber.Text != "" || bloodType.Text != ""))
                    {
                        MessageBoxResult result = MessageBox.Show("All unsaved changes will be lost. Do you want to continue?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                            adminPanel.Close();
                    }
                    else
                    {
                        adminPanel.Close();
                    }                   
            }
            else if (senderName.Equals("closeAbout"))
            {
                isWindow2Closed = true;

                window2.Close();
            }
            else
            {
                this.Close();

                if (window2 != null)
                    window2.Close();
            }

            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }        

        private void About_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {                  
            if (isWindow2Closed)
            {
                //shows info about the program
                window2 = new Window();

                window2.Width = 500;
                window2.Height = 400;

                //makes the window2 be on the top of other windows
                window2.Topmost = true;

                //window2 properties
                window2.WindowStyle = WindowStyle.None;
                window2.AllowsTransparency = true;
                window2.ResizeMode = ResizeMode.NoResize;
                window2.Background = new SolidColorBrush(Colors.Transparent);
                window2.Name = "about";
                window2.MouseDown += (MouseButtonEventHandler)Window_MouseDown;

                //create the border of a window2
                Border window2Border = new Border();

                window2Border.BorderBrush = Brushes.Black;
                window2Border.BorderThickness = new Thickness(1.5);
                window2Border.CornerRadius = new CornerRadius(15);
                window2Border.Background = Brushes.WhiteSmoke;

                //create StackPanel
                StackPanel stackPanel = new StackPanel();

                //create image and add it to stack panel
                Image image = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/pics/donation.png");
                bitmapImage.EndInit();
                image.Source = bitmapImage;
                image.Width = 115;
                image.Height = 115;
                image.Margin = new Thickness(15);

                stackPanel.Children.Add(image);

                //create button and textblock, and add it to stack panel
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Blood Bank version 1.0";
                
                textBlock.Text += "\n\n\nThis app keeps the data about the blood gathered as a result of blood donation, about donors and their contact information.";
                
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.FontSize = 18;
                textBlock.FontFamily = new FontFamily("Courier");
                textBlock.Margin = new Thickness(15, 10, 10, 10);
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;

                Button button = new Button();
                button.Content = "Close";
                button.FontSize = 18;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Red;
                button.Cursor = Cursors.Hand;
                button.BorderThickness = new Thickness(1.5);
                button.BorderBrush = Brushes.Red;
                button.Padding = new Thickness(4);
                button.Width = 100;
                button.Height = 35;
                button.Margin = new Thickness(0, 40, 0, 0);
                button.Name = "closeAbout";
                button.Click += Close_Click;

                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(button);

                //add stack panel to border element
                window2Border.Child = stackPanel;

                //finally, add all elements to window2 element
                window2.Content = window2Border;

                //this part of the code defines window2's startup location
                Application curApp = Application.Current;
                Window window1 = curApp.MainWindow;
                window2.Left = window1.Left - (window1.Width - window2.ActualWidth) / 2;
                window2.Top = window1.Top + (window2.Height - window1.ActualHeight) / 2;

                window2.Show();
                isWindow2Closed = false;
            }
            
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            LoginCheck(e);
        }
        
        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            LoginCheck(e);
        }

        private void LogIn()
        {
            bool bRead = false;

            string selectQuery = @"SELECT * FROM Users WHERE [User] = @name AND [Pass] = @pass";

            conn.Open();            

            OleDbCommand oleDbCmd = new OleDbCommand(selectQuery, conn);
            oleDbCmd.Parameters.AddWithValue("@name", Txt.Text);
            oleDbCmd.Parameters.AddWithValue("@pass", Pass.Password.ToString());

            OleDbDataReader dataReader = oleDbCmd.ExecuteReader();
            bRead = dataReader.Read();
           
            if (bRead)
            {
                openAdminPanel();
            }
            else
            {
                MessageBox.Show("Invalid Username/Password combination! Please, try again.", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
                Txt.Text = "";
                Pass.Password = "";
                Txt.Focus();
            }

            dataReader.Close();
            conn.Close();
        }

        private void LoginCheck(EventArgs eventArgs)
        {
            if (eventArgs is KeyEventArgs)
            {
                KeyEventArgs e = eventArgs as KeyEventArgs;

                if (Txt.Text != "" && Pass.Password.ToString() != "")
                {
                    if (Txt.Text.All(char.IsLetterOrDigit))
                    {
                        if (e.Key.ToString().Equals("Return"))
                            LogIn();
                    }
                    else
                    {
                        if (e.Key.ToString().Equals("Return"))
                        {
                            MessageBox.Show("Invalid Username Format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            
                            Txt.Text = "";
                            Pass.Password = "";
                        }
                    }
                }
                else
                {
                    if (e.Key.ToString().Equals("Return"))
                        MessageBox.Show("Write Username/Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else if (eventArgs is RoutedEventArgs)
            {                

                if (Txt.Text != "" && Pass.Password.ToString() != "")
                {
                    if (Txt.Text.All(char.IsLetterOrDigit))
                    {                        
                            LogIn();
                    }
                    else
                    {
                            MessageBox.Show("Invalid Username Format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            Txt.Text = "";
                            Pass.Password = "";                      
                    }
                }
                else
                {                    
                    MessageBox.Show("Write Username/Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }            
        }

        private void openAdminPanel()
        {
            //adminPanel and its properties
            #region

            //create adminPanel window
            adminPanel = new Window();

            adminPanel.Width = 900;
            adminPanel.Height = 600;
            adminPanel.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            adminPanel.WindowStyle = WindowStyle.None;
            adminPanel.AllowsTransparency = true;
            adminPanel.ResizeMode = ResizeMode.NoResize;
            adminPanel.Background = new SolidColorBrush(Colors.Transparent);
            adminPanel.Name = "adminPanel";
            adminPanel.MouseDown += (MouseButtonEventHandler)Window_MouseDown;
            #endregion

            //border and its properties
            #region

            //create border for adminPanel window
            Border adminPanelBorder = new Border();

            adminPanelBorder.BorderBrush = Brushes.Black;
            adminPanelBorder.BorderThickness = new Thickness(1.5);
            adminPanelBorder.CornerRadius = new CornerRadius(15);
            adminPanelBorder.Background = Brushes.White;
            #endregion

            //stackPanels and their properties
            #region
            
            mainStackPanel = new StackPanel();
            StackPanel minAndClosePanel = new StackPanel();
            StackPanel windowStack = new StackPanel();
            StackPanel footer = new StackPanel();

            StackPanel adminWindowStack = new StackPanel();
            StackPanel donorInfo = new StackPanel();
            StackPanel donorInfo2 = new StackPanel();
            StackPanel donorInfo3 = new StackPanel();
            StackPanel donorInfo4 = new StackPanel();
            StackPanel donorInfo5 = new StackPanel();
            StackPanel donorInfo6 = new StackPanel();

            StackPanel settingsPanel = new StackPanel();
            StackPanel usrInfo = new StackPanel();
            StackPanel usrInfo2 = new StackPanel();
            StackPanel usrInfo3 = new StackPanel();
            StackPanel usrInfo4 = new StackPanel();
            StackPanel usrInfo5 = new StackPanel();

            StackPanel listBoxStack = new StackPanel();

            StackPanel addButtonStack = new StackPanel();
            StackPanel deleteButtonStack = new StackPanel();
                        
            mainStackPanel.Focusable = true;
            //child stacks of mainStackPanel and their properties
            #region

            minAndClosePanel.Orientation = Orientation.Horizontal;
            minAndClosePanel.HorizontalAlignment = HorizontalAlignment.Right;

            windowStack.Orientation = Orientation.Horizontal;

            //child stacks of adminWindowStack and their properties
            #region
            donorInfo.Orientation = Orientation.Horizontal;
            donorInfo2.Orientation = Orientation.Horizontal;
            donorInfo3.Orientation = Orientation.Horizontal;
            donorInfo4.Orientation = Orientation.Horizontal;
            donorInfo6.Orientation = Orientation.Horizontal;

            donorInfo5.HorizontalAlignment = HorizontalAlignment.Left;
            donorInfo6.HorizontalAlignment = HorizontalAlignment.Right;

            donorInfo.Margin = new Thickness(0, 20, 0, 0);
            donorInfo2.Margin = new Thickness(0, 10, 0, 0);
            donorInfo3.Margin = new Thickness(0, 10, 0, 0);
            donorInfo4.Margin = new Thickness(0, 10, 0, 0);
            donorInfo5.Margin = new Thickness(0, 10, 0, 0);
            donorInfo6.Margin = new Thickness(0, 50, 0, 0);
            #endregion

            //child stacks of settingsPanel and their properties
            #region
            usrInfo.Orientation = Orientation.Horizontal;
            usrInfo2.Orientation = Orientation.Horizontal;
            usrInfo3.Orientation = Orientation.Horizontal;
            usrInfo4.Orientation = Orientation.Horizontal;
            usrInfo5.Orientation = Orientation.Horizontal;

            usrInfo.Margin = new Thickness(0, 20, 0, 0);
            usrInfo2.Margin = new Thickness(0, 10, 0, 0);
            usrInfo3.Margin = new Thickness(0, 10, 0, 0);
            usrInfo4.Margin = new Thickness(0, 10, 0, 0);
            usrInfo5.Margin = new Thickness(0, 124.5, 0, 0);

            usrInfo5.HorizontalAlignment = HorizontalAlignment.Right;
            #endregion

            footer.Orientation = Orientation.Horizontal;

            //child stacks of footer and their properties
            #region
            addButtonStack.Orientation = Orientation.Horizontal;
            deleteButtonStack.Orientation = Orientation.Horizontal;
            #endregion

            #endregion

            #endregion

            //buttons and their properties
            #region
            Button minimizeButton = new Button();
            Button closeButton = new Button();
            Button edit = new Button();
            Button saveChanges = new Button();
            Button cancel = new Button();
            Button add = new Button();
            Button delete = new Button();

            //minimize button
            minimizeButton.Width = 35;
            minimizeButton.Height = 35;
            minimizeButton.Background = Brushes.Transparent;
            minimizeButton.HorizontalAlignment = HorizontalAlignment.Right;
            minimizeButton.Margin = new Thickness(0, 5, 15, 0);
            minimizeButton.BorderThickness = new Thickness(0);
            minimizeButton.Name = "minimizeAdminPanel";

            Image minimizeImage = new Image();
            BitmapImage minBitmapImage = new BitmapImage();
            minBitmapImage.BeginInit();
            minBitmapImage.UriSource = new Uri("pack://application:,,,/pics/minimize.png");
            minBitmapImage.EndInit();
            minimizeImage.Source = minBitmapImage;

            minimizeButton.Content = minimizeImage;
            minimizeButton.Click += Minimize_Click;

            //close button
            closeButton.Width = 35;
            closeButton.Height = 35;
            closeButton.Background = Brushes.Transparent;
            closeButton.HorizontalAlignment = HorizontalAlignment.Right;
            closeButton.Margin = new Thickness(0, 5, 15, 0);
            closeButton.BorderThickness = new Thickness(0);
            closeButton.Name = "closeAdminPanel";

            Image closeImage = new Image();
            BitmapImage closeBitmapImage = new BitmapImage();
            closeBitmapImage.BeginInit();
            closeBitmapImage.UriSource = new Uri("pack://application:,,,/pics/close.png");
            closeBitmapImage.EndInit();
            closeImage.Source = closeBitmapImage;

            closeButton.Content = closeImage;
            closeButton.Click += Close_Click;

            //edit button
            edit.Name = "edit";
            edit.Width = 80;
            edit.Height = 30;
            edit.Content = "Edit";
            edit.FontSize = 18;
            edit.FontWeight = FontWeights.SemiBold;
            edit.Background = Brushes.White;
            edit.Foreground = Brushes.LightSkyBlue;
            edit.Margin = new Thickness(0, 0, 15, 0);
            edit.BorderThickness = new Thickness(2.5);
            edit.BorderBrush = Brushes.LightSkyBlue;
            edit.Click += Settings_Click;

            //save changes button
            saveChanges.Name = "saveChanges";
            saveChanges.Height = 30;
            saveChanges.Content = "Save Changes";
            saveChanges.FontSize = 18;
            saveChanges.FontWeight = FontWeights.SemiBold;
            saveChanges.Background = Brushes.White;
            saveChanges.Foreground = Brushes.LightGreen;
            saveChanges.Margin = new Thickness(0, 0, 15, 0);
            saveChanges.BorderThickness = new Thickness(2.5);
            saveChanges.BorderBrush = Brushes.LightGreen;
            saveChanges.Click += Settings_Click;

            //cancel button
            cancel.Name = "cancel";
            cancel.Width = 80;
            cancel.Height = 30;
            cancel.Content = "Cancel";
            cancel.FontSize = 18;
            cancel.FontWeight = FontWeights.SemiBold;
            cancel.Background = Brushes.White;
            cancel.Foreground = Brushes.Red;
            cancel.Margin = new Thickness(0, 0, 15, 0);
            cancel.BorderThickness = new Thickness(2.5);
            cancel.BorderBrush = Brushes.Red;
            cancel.Click += Settings_Click;

            //add button
            add.Name = "add";
            add.Background = Brushes.White;
            add.Padding = new Thickness(3);
            add.Margin = new Thickness(220, 20, 0, 0);
            add.BorderThickness = new Thickness(2.5);
            add.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#85be65");
            add.Click += Add_Click;

            Image addImage = new Image();
            addImage.Width = 32;
            addImage.Height = 32;
            BitmapImage addBitmapImage = new BitmapImage();
            addBitmapImage.BeginInit();
            addBitmapImage.UriSource = new Uri("pack://application:,,,/pics/add.png");
            addBitmapImage.EndInit();
            addImage.Source = addBitmapImage;

            TextBlock addButtonText = new TextBlock();
            addButtonText.Text = "Add";
            addButtonText.FontSize = 20;
            addButtonText.FontWeight = FontWeights.SemiBold;
            addButtonText.Background = Brushes.Transparent;
            addButtonText.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#85be65");
            addButtonText.VerticalAlignment = VerticalAlignment.Center;
            addButtonText.Margin = new Thickness(7, 0, 0, 0);

            //delete button
            delete.Name = "delete";
            delete.Background = Brushes.White;
            delete.Padding = new Thickness(3);
            delete.Margin = new Thickness(20, 20, 0, 0);
            delete.BorderThickness = new Thickness(2.5);
            delete.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#e44020");
            delete.IsEnabled = false;
            delete.Click += Delete_Click;

            Image deleteImage = new Image();
            deleteImage.Width = 32;
            deleteImage.Height = 32;
            BitmapImage deleteBitmapImage = new BitmapImage();
            deleteBitmapImage.BeginInit();
            deleteBitmapImage.UriSource = new Uri("pack://application:,,,/pics/delete.png");
            deleteBitmapImage.EndInit();
            deleteImage.Source = deleteBitmapImage;

            TextBlock deleteButtonText = new TextBlock();
            deleteButtonText.Text = "Delete";
            deleteButtonText.FontSize = 20;
            deleteButtonText.FontWeight = FontWeights.SemiBold;
            deleteButtonText.Background = Brushes.Transparent;
            deleteButtonText.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#e44020");
            deleteButtonText.VerticalAlignment = VerticalAlignment.Center;
            deleteButtonText.Margin = new Thickness(7, 0, 0, 0);
            #endregion

            //tabControl and its properties
            #region
            tabControl = new TabControl();
            tabControl.HorizontalAlignment = HorizontalAlignment.Left;
            tabControl.TabStripPlacement = Dock.Left;
            tabControl.Margin = new Thickness(15, 30, 0, 0);
            tabControl.Width = 580;
            tabControl.Height = 430;
            tabControl.SelectedIndex = 0;
            tabControl.SelectionChanged += TabControl_SelectionChanged;

            //tabItems and their properties
            #region

            //define icons for tabItems
            Image userIcon = new Image();
            userIcon.Width = 50;
            userIcon.Height = 50;
            BitmapImage userIconBitmapImage = new BitmapImage();
            userIconBitmapImage.BeginInit();
            userIconBitmapImage.UriSource = new Uri("pack://application:,,,/pics/user.png");
            userIconBitmapImage.EndInit();
            userIcon.Source = userIconBitmapImage;

            Image settingsIcon = new Image();
            settingsIcon.Width = 50;
            settingsIcon.Height = 50;
            BitmapImage settingsIconBitmapImage = new BitmapImage();
            settingsIconBitmapImage.BeginInit();
            settingsIconBitmapImage.UriSource = new Uri("pack://application:,,,/pics/settings.png");
            settingsIconBitmapImage.EndInit();
            settingsIcon.Source = settingsIconBitmapImage;

            //create tabItems
            TabItem adminWindow = new TabItem();
            TabItem settings = new TabItem();

            //add icons to tabItems
            adminWindow.Header = userIcon;            
            settings.Header = settingsIcon;
 
            //elements of tabItems
            #region

            //settings content
            #region

            TextBlock header = new TextBlock();
            header.Text = "Settings";
            header.FontSize = 24;
            header.FontWeight = FontWeights.DemiBold;
            header.HorizontalAlignment = HorizontalAlignment.Center;
            header.TextAlignment = TextAlignment.Center;
            header.Padding = new Thickness(5);
            header.Background = Brushes.LightPink;
            header.Width = 580;

            //username
            TextBlock usr = new TextBlock();
            usr.Text = "Username";
            usr.FontSize = 18;
            usr.FontStyle = FontStyles.Italic;        
            usr.Margin = new Thickness(30, 10, 0, 0);

            usrBox = new TextBox();
            usrBox.Text = "admin";
            usrBox.Width = 200;
            usrBox.Margin = new Thickness(132, 10, 0, 0);
            usrBox.Padding = new Thickness(2);
            usrBox.FontSize = 16;
            usrBox.IsEnabled = false;
            usrBox.Background = Brushes.LightGray;
            
            //old password
            TextBlock oldPass = new TextBlock();
            oldPass.Text = "Old Password";
            oldPass.FontSize = 18;
            oldPass.FontStyle = FontStyles.Italic;
            oldPass.Margin = new Thickness(30, 10, 0, 0);

            oldPassBox = new PasswordBox();
            oldPassBox.Width = 200;
            oldPassBox.Margin = new Thickness(104, 10, 0, 0);
            oldPassBox.Padding = new Thickness(2);
            oldPassBox.FontSize = 16;
            oldPassBox.IsEnabled = false;
            oldPassBox.Background = Brushes.LightGray;

            //new password
            TextBlock nwPass = new TextBlock();
            nwPass.Text = "New Password";
            nwPass.FontSize = 18;
            nwPass.FontStyle = FontStyles.Italic;
            nwPass.Margin = new Thickness(30, 10, 0, 0);

            nwPassBox = new PasswordBox();
            nwPassBox.Width = 200;
            nwPassBox.Margin = new Thickness(98, 10, 0, 0);
            nwPassBox.Padding = new Thickness(2);
            nwPassBox.FontSize = 16;
            nwPassBox.IsEnabled = false;
            nwPassBox.Background = Brushes.LightGray;

            //confirm new password
            TextBlock ConfirmPass = new TextBlock();
            ConfirmPass.Text = "Confirm New Password";
            ConfirmPass.FontSize = 18;
            ConfirmPass.FontStyle = FontStyles.Italic;
            ConfirmPass.Margin = new Thickness(30, 10, 0, 0);

            ConfirmPassBox = new PasswordBox();
            ConfirmPassBox.Width = 200;
            ConfirmPassBox.Margin = new Thickness(30, 10, 0, 0);
            ConfirmPassBox.Padding = new Thickness(2);
            ConfirmPassBox.FontSize = 16;
            ConfirmPassBox.IsEnabled = false;
            ConfirmPassBox.Background = Brushes.LightGray;
            #endregion

            //adminWindow content
            #region

            TextBlock adminWindowHeader = new TextBlock();
            adminWindowHeader.Text = "Donor Information";
            adminWindowHeader.FontSize = 24;
            adminWindowHeader.FontWeight = FontWeights.DemiBold;
            adminWindowHeader.HorizontalAlignment = HorizontalAlignment.Center;
            adminWindowHeader.TextAlignment = TextAlignment.Center;
            adminWindowHeader.Padding = new Thickness(5);
            adminWindowHeader.Background = Brushes.LightPink;
            adminWindowHeader.Width = 580;

            //donor name
            TextBlock donorName = new TextBlock();
            donorName.Text = "Name";
            donorName.FontSize = 18;
            donorName.FontStyle = FontStyles.Italic;
            donorName.Margin = new Thickness(33, 10, 0, 0);

            name = new TextBox();
            name.Width = 200;
            name.Margin = new Thickness(30, 10, 0, 0);
            name.Padding = new Thickness(2);
            name.FontSize = 16;
            name.IsEnabled = false;
            name.Background = Brushes.LightGray;

            //donor surname
            TextBlock donorSurname = new TextBlock();
            donorSurname.Text = "Surname";
            donorSurname.FontSize = 18;
            donorSurname.FontStyle = FontStyles.Italic;
            donorSurname.Margin = new Thickness(203, 10, 0, 0);

            surname = new TextBox();
            surname.Width = 200;
            surname.Margin = new Thickness(50, 10, 0, 0);
            surname.Padding = new Thickness(2);
            surname.FontSize = 16;
            surname.IsEnabled = false;
            surname.Background = Brushes.LightGray;

            //donor address
            TextBlock donorAddress = new TextBlock();
            donorAddress.Text = "Address";
            donorAddress.FontSize = 18;
            donorAddress.FontStyle = FontStyles.Italic;
            donorAddress.Margin = new Thickness(33, 10, 0, 0);

            address = new TextBox();
            address.Width = 200;
            address.Margin = new Thickness(30, 10, 0, 0);
            address.Padding = new Thickness(2);
            address.FontSize = 16;
            address.IsEnabled = false;
            address.Background = Brushes.LightGray;

            //donor phone number
            TextBlock donorPhoneNum = new TextBlock();
            donorPhoneNum.Text = "Phone Number";
            donorPhoneNum.FontSize = 18;
            donorPhoneNum.FontStyle = FontStyles.Italic;
            donorPhoneNum.Margin = new Thickness(192, 10, 0, 0);

            phoneNumber = new TextBox();
            phoneNumber.Width = 200;
            phoneNumber.Margin = new Thickness(52, 10, 0, 0);
            phoneNumber.Padding = new Thickness(2);
            phoneNumber.FontSize = 16;
            phoneNumber.IsEnabled = false;
            phoneNumber.Background = Brushes.LightGray;

            //donor blood type
            TextBlock donorBloodType = new TextBlock();
            donorBloodType.Text = "Blood Type";
            donorBloodType.FontSize = 18;
            donorBloodType.FontStyle = FontStyles.Italic;
            donorBloodType.Margin = new Thickness(33, 10, 0, 0);

            bloodType = new TextBox();
            bloodType.Width = 200;
            bloodType.Margin = new Thickness(30, 10, 0, 0);
            bloodType.Padding = new Thickness(2);
            bloodType.FontSize = 16;
            bloodType.IsEnabled = false;
            bloodType.Background = Brushes.LightGray;           
            #endregion

            #endregion

            #endregion
           
            #endregion

            //listBox and its properties
            #region

            listBoxHeader = new TextBlock();
            listBoxHeader.Text = "List of Donors";
            listBoxHeader.FontSize = 17;            
            listBoxHeader.FontWeight = FontWeights.DemiBold;
            listBoxHeader.HorizontalAlignment = HorizontalAlignment.Center;
            listBoxHeader.TextAlignment = TextAlignment.Center;
            listBoxHeader.Padding = new Thickness(3);
            listBoxHeader.Margin = new Thickness(40, 30, 0, -2);
            listBoxHeader.Background = Brushes.LightPink;
            listBoxHeader.Width = 229;
            listBoxHeader.Height = 30;

            listBox = new ListBox();
            listBox.Width = 229.95;
            listBox.Height = 402;
            listBox.Margin = new Thickness(40, 0, 0, 0);

            listBox.FontSize = 16;
            listBox.SelectionChanged += ListBox_SelectionChanged;            
            #endregion
                     
            //footer and its properties
            #region

            //create comboBox
            comboBox = new ComboBox();
            comboBox.Width = 105;
            comboBox.Height = 30;
            comboBox.FontSize = 14;
            comboBox.SelectedIndex = 0;
            comboBox.Items.Add("Donors");
            comboBox.Items.Add("Blood Types");
            comboBox.Padding = new Thickness(5);
            comboBox.Margin = new Thickness(55, 20, 0, 0);
            comboBox.SelectionChanged += ComboBox_SelectionChanged;

            //create searchBox
            searchBox = new TextBox();
            searchBox.FontSize = 14;
            searchBox.Foreground = Brushes.LightGray;
            searchBox.Text = "Search For Donors";
            searchBox.BorderBrush = Brushes.LightGray;
            searchBox.Width = 230;
            searchBox.Height = 30;
            searchBox.Padding = new Thickness(4);
            searchBox.Margin = new Thickness(40, 20, 0, 0);
            searchBox.GotFocus += Search_GotFocus;
            searchBox.LostFocus += Search_LostFocus;            
            searchBox.TextChanged += SearchBox_TextChanged;
            #endregion

            //ADD EVERYTHING TO THE SCREEN
            #region

            //add buttons to minAndClosePanel
            minAndClosePanel.Children.Add(minimizeButton);
            minAndClosePanel.Children.Add(closeButton);
            
            //child stacks of adminWindowStack
            donorInfo.Children.Add(donorName);
            donorInfo.Children.Add(donorSurname);

            donorInfo2.Children.Add(name);
            donorInfo2.Children.Add(surname);

            donorInfo3.Children.Add(donorAddress);
            donorInfo3.Children.Add(donorPhoneNum);

            donorInfo4.Children.Add(address);
            donorInfo4.Children.Add(phoneNumber);

            donorInfo5.Children.Add(donorBloodType);
            donorInfo5.Children.Add(bloodType);
                       
            //add all of them to adminWindowStack
            adminWindowStack.Children.Add(adminWindowHeader);
            adminWindowStack.Children.Add(donorInfo);
            adminWindowStack.Children.Add(donorInfo2);
            adminWindowStack.Children.Add(donorInfo3);
            adminWindowStack.Children.Add(donorInfo4);
            adminWindowStack.Children.Add(donorInfo5);
            adminWindowStack.Children.Add(donorInfo6);

            //child stacks of settingsPanel
            usrInfo.Children.Add(usr);
            usrInfo.Children.Add(usrBox);

            usrInfo2.Children.Add(oldPass);
            usrInfo2.Children.Add(oldPassBox);

            usrInfo3.Children.Add(nwPass);
            usrInfo3.Children.Add(nwPassBox);

            usrInfo4.Children.Add(ConfirmPass);
            usrInfo4.Children.Add(ConfirmPassBox);
           
            //add all of them to settingsPanel
            settingsPanel.Children.Add(header);
            settingsPanel.Children.Add(usrInfo);
            settingsPanel.Children.Add(usrInfo2);
            settingsPanel.Children.Add(usrInfo3);
            settingsPanel.Children.Add(usrInfo4);
            settingsPanel.Children.Add(usrInfo5);

            //finally, add everything to adminWindow and settings tabItems
            adminWindow.Content = adminWindowStack;
            settings.Content = settingsPanel;
                       
            //add tabItems to tabControl
            tabControl.Items.Add(adminWindow);
            tabControl.Items.Add(settings);

            //add listBoxHeader and listBox to listBoxStack
            listBoxStack.Children.Add(listBoxHeader);
            listBoxStack.Children.Add(listBox);
            
            //add tabControl and listBoxStack to windowStack
            windowStack.Children.Add(tabControl);
            windowStack.Children.Add(listBoxStack);
            
            //child stacks of footer
            addButtonStack.Children.Add(addImage);
            addButtonStack.Children.Add(addButtonText);

            deleteButtonStack.Children.Add(deleteImage);
            deleteButtonStack.Children.Add(deleteButtonText);

            add.Content = addButtonStack;            
            delete.Content = deleteButtonStack;

            //add 'add' and 'delete' buttons, comboBox and searchBox to footer
            footer.Children.Add(add);
            footer.Children.Add(delete);
            footer.Children.Add(comboBox);
            footer.Children.Add(searchBox);            
                      
            //add minAndClosePanel, windowStack and footer to mainStackPanel
            mainStackPanel.Children.Add(minAndClosePanel);
            mainStackPanel.Children.Add(windowStack);
            mainStackPanel.Children.Add(footer);

            //add mainStackPanel to adminPanelBorder
            adminPanelBorder.Child = mainStackPanel;

            //and add adminPanelBorder to adminPanel
            adminPanel.Content = adminPanelBorder;

            #endregion

            adminPanel.Show();

            ShowInfo(comboBox.SelectedValue.ToString());

            this.Close();            

            if (!isWindow2Closed)
            {
                window2.Close();
            }            

            //Local Functions
            void Settings_Click(object sender, RoutedEventArgs e)
            {
                string senderName = ((Button)sender).Name;

                if (senderName.Equals("edit"))
                {
                    if (!isEditable)
                    {
                        if (tabControl.SelectedIndex == 0)
                        {
                            if (name.IsEnabled == false && surname.IsEnabled == false && address.IsEnabled == false && phoneNumber.IsEnabled == false && bloodType.IsEnabled == false)
                            {
                                name.IsEnabled = true;
                                surname.IsEnabled = true;
                                address.IsEnabled = true;
                                phoneNumber.IsEnabled = true;
                                bloodType.IsEnabled = true;

                                name.Background = Brushes.White;
                                surname.Background = Brushes.White;
                                address.Background = Brushes.White;
                                phoneNumber.Background = Brushes.White;
                                bloodType.Background = Brushes.White;

                                isEditable = true;
                            }
                        }
                        else if (tabControl.SelectedIndex == 1)
                        {
                            if (oldPassBox.IsEnabled == false && nwPassBox.IsEnabled == false && ConfirmPassBox.IsEnabled == false)
                            {
                                oldPassBox.IsEnabled = true;
                                nwPassBox.IsEnabled = true;
                                ConfirmPassBox.IsEnabled = true;

                                oldPassBox.Background = Brushes.White;
                                nwPassBox.Background = Brushes.White;
                                ConfirmPassBox.Background = Brushes.White;

                                isEditable = true;
                            }
                        }                        
                    }                    
                }
                else if (senderName.Equals("cancel"))
                {
                    if (tabControl.SelectedIndex == 0)
                    {                      
                            if (name.Text.Equals("") && surname.Text.Equals("") && address.Text.Equals("") && phoneNumber.Text.Equals("") && bloodType.Text.Equals(""))
                            {
                                name.IsEnabled = false;
                                surname.IsEnabled = false;
                                address.IsEnabled = false;
                                phoneNumber.IsEnabled = false;
                                bloodType.IsEnabled = false;

                                name.Background = Brushes.LightGray;
                                surname.Background = Brushes.LightGray;
                                address.Background = Brushes.LightGray;
                                phoneNumber.Background = Brushes.LightGray;
                                bloodType.Background = Brushes.LightGray;

                                isEditable = false;
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("All unsaved changes will be lost. Do you want to continue?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    name.IsEnabled = false;
                                    surname.IsEnabled = false;
                                    address.IsEnabled = false;
                                    phoneNumber.IsEnabled = false;
                                    bloodType.IsEnabled = false;

                                    name.Background = Brushes.LightGray;
                                    surname.Background = Brushes.LightGray;
                                    address.Background = Brushes.LightGray;
                                    phoneNumber.Background = Brushes.LightGray;
                                    bloodType.Background = Brushes.LightGray;

                                    name.Text = "";
                                    surname.Text = "";
                                    address.Text = "";
                                    phoneNumber.Text = "";
                                    bloodType.Text = "";

                                    isEditable = false;
                                    listBox.SelectedItem = null;
                                    delete.IsEnabled = false;
                                    initDonor.Clear();
                                }
                            }
                    }
                    else if (tabControl.SelectedIndex == 1)
                    {
                            if (oldPassBox.Password.ToString().Equals("") && nwPassBox.Password.ToString().Equals("") && ConfirmPassBox.Password.ToString().Equals(""))
                            {
                                oldPassBox.IsEnabled = false;
                                nwPassBox.IsEnabled = false;
                                ConfirmPassBox.IsEnabled = false;

                                oldPassBox.Background = Brushes.LightGray;
                                nwPassBox.Background = Brushes.LightGray;
                                ConfirmPassBox.Background = Brushes.LightGray;

                                isEditable = false;
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("All unsaved changes will be lost. Do you want to continue?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    oldPassBox.IsEnabled = false;
                                    nwPassBox.IsEnabled = false;
                                    ConfirmPassBox.IsEnabled = false;

                                    oldPassBox.Background = Brushes.LightGray;
                                    nwPassBox.Background = Brushes.LightGray;
                                    ConfirmPassBox.Background = Brushes.LightGray;

                                    oldPassBox.Password = "";
                                    nwPassBox.Password = "";
                                    ConfirmPassBox.Password = "";

                                    isEditable = false;
                                }
                            }
                    }                    
                }
                else if (senderName.Equals("saveChanges"))
                {
                        if (tabControl.SelectedIndex == 0)
                        {                        
                            if (listBox.SelectedItem == null)
                            {
                                MessageBox.Show("Please, add/choose a donor to edit and save changes.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                name.Text = "";
                                surname.Text = "";
                                address.Text = "";
                                phoneNumber.Text = "";
                                bloodType.Text = "";
                            }
                            else
                            {
                                if (name.Text.Equals(initDonor.ElementAt(0)) && surname.Text.Equals(initDonor.ElementAt(1)) && address.Text.Equals(initDonor.ElementAt(2)) && phoneNumber.Text.Equals(initDonor.ElementAt(3)) && bloodType.Text.Equals(initDonor.ElementAt(4)))
                                {
                                    MessageBox.Show("Nothing has been changed.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    if (name.Text.Equals("") || surname.Text.Equals("") || address.Text.Equals("") || phoneNumber.Text.Equals("") || bloodType.Text.Equals(""))
                                    {
                                        MessageBox.Show("All fields are required.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        if (name.Text.All(char.IsLetter) && surname.Text.All(char.IsLetter) && (address.Text.Length > 0 && Char.IsLetter(address.Text.ElementAt(0))) && phoneNumber.Text.All(char.IsDigit))
                                        {
                                            string pattern = @"(O\(I\) |A\(II\) |B\(III\) |AB\(IV\) )Rh(\+|\-)";

                                            Regex regex = new Regex(pattern);

                                            if (regex.IsMatch(bloodType.Text))
                                            {
                                                int phone_num = Int32.Parse(phoneNumber.Text);

                                                conn.Open();

                                                string sqlQuery = @"UPDATE Donors SET [DonorName] = @name, [DonorSurname] = @surname, [Address] = @address, [PhoneNum] = @phoneNum, [BloodType] = @bloodType WHERE [ID] = @id";

                                                OleDbCommand oleDbCommand = new OleDbCommand(sqlQuery, conn);
                                                oleDbCommand.Parameters.AddWithValue("@name", name.Text);
                                                oleDbCommand.Parameters.AddWithValue("@surname", surname.Text);
                                                oleDbCommand.Parameters.AddWithValue("@address", address.Text);
                                                oleDbCommand.Parameters.AddWithValue("@phoneNum", phone_num);
                                                oleDbCommand.Parameters.AddWithValue("@bloodType", bloodType.Text);
                                                oleDbCommand.Parameters.AddWithValue("@id", Int32.Parse(donorTable.Rows[listBox.SelectedIndex]["ID"].ToString()));

                                                oleDbCommand.ExecuteScalar();

                                                MessageBox.Show("Donor Information has been successfully changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                                name.Text = "";
                                                surname.Text = "";
                                                address.Text = "";
                                                phoneNumber.Text = "";
                                                bloodType.Text = "";

                                                conn.Close();

                                                initDonor.Clear();    

                                                ShowInfo(comboBox.SelectedItem.ToString());
                                            }
                                            else
                                            {
                                                MessageBox.Show("Blood Type has a wrong format!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Entered values have a wrong format!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            name.Text = "";
                                            surname.Text = "";
                                            address.Text = "";
                                            phoneNumber.Text = "";
                                            bloodType.Text = "";
                                        }
                                    }                                   
                                }
                            }
                        }
                        else if (tabControl.SelectedIndex == 1)
                        {
                            bool bRead = false;

                            string selectQuery = @"SELECT * FROM Users WHERE [User] = @name AND [Pass] = @pass";

                            conn.Open();

                            OleDbCommand oleDbCmd = new OleDbCommand(selectQuery, conn);
                            oleDbCmd.Parameters.AddWithValue("@name", usrBox.Text);
                            oleDbCmd.Parameters.AddWithValue("@pass", oldPassBox.Password.ToString());

                            OleDbDataReader dataReader = oleDbCmd.ExecuteReader();
                            bRead = dataReader.Read();

                            dataReader.Close();

                            //if password fields are not empty                                                
                            if (!nwPassBox.Password.ToString().Equals("") && !ConfirmPassBox.Password.ToString().Equals("") && !oldPassBox.Password.ToString().Equals(""))
                            {
                                //if old password is correct
                                if (bRead)
                                {
                                    //if new passwords are same
                                    if (nwPassBox.Password.ToString().Equals(ConfirmPassBox.Password.ToString()))
                                    {
                                        string query = @"UPDATE Users SET [Pass] = @pass";

                                        OleDbCommand oleDbCommand = new OleDbCommand(query, conn);
                                        oleDbCommand.Parameters.AddWithValue("@pass", nwPassBox.Password);

                                        oleDbCommand.ExecuteScalar();

                                        MessageBox.Show("Password has been changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                                        isEditable = false;

                                        oldPassBox.IsEnabled = false;
                                        nwPassBox.IsEnabled = false;
                                        ConfirmPassBox.IsEnabled = false;

                                        oldPassBox.Background = Brushes.LightGray;
                                        nwPassBox.Background = Brushes.LightGray;
                                        ConfirmPassBox.Background = Brushes.LightGray;

                                        oldPassBox.Password = "";
                                        nwPassBox.Password = "";
                                        ConfirmPassBox.Password = "";
                                    }
                                    else
                                    {
                                        MessageBox.Show("Values of 'New Password' and 'Confirm New Password' fields are not same!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                        oldPassBox.Password = "";
                                        nwPassBox.Password = "";
                                        ConfirmPassBox.Password = "";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Incorrect Password! Please, try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    oldPassBox.Password = "";
                                    nwPassBox.Password = "";
                                    ConfirmPassBox.Password = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("All fields must be filled!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                                oldPassBox.Password = "";
                                nwPassBox.Password = "";
                                ConfirmPassBox.Password = "";
                            }

                            conn.Close();
                        }                                            
                }
            }

            void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                searchBox.Foreground = Brushes.LightGray;

                if (comboBox.SelectedItem.ToString().Equals("Donors"))
                {
                    listBoxHeader.Text = "List of Donors";
                    searchBox.Text = "Search For Donors";                    
                }
                else if (comboBox.SelectedItem.ToString().Equals("Blood Types"))
                {
                    listBoxHeader.Text = "List of Blood Types";
                    searchBox.Text = "Search For Blood Types";                   
                }

                ShowInfo(comboBox.SelectedItem.ToString());
            }

            void Search_GotFocus(object sender, RoutedEventArgs e)
            {
                searchBox.Foreground = Brushes.Black;

                if(searchBox.Text.Equals("Search For Donors") || searchBox.Text.Equals("Search For Blood Types"))
                    searchBox.Text = "";
            }

            void Search_LostFocus(object sender, RoutedEventArgs e)
            {                
                if (searchBox.Text.Length == 0)
                {
                    searchBox.Foreground = Brushes.LightGray;
                    searchBox.Text = "Search For " + comboBox.SelectedItem.ToString();
                }                   
            }

            void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                ShowInfo(comboBox.SelectedItem.ToString());
            }

            void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (tabControl.SelectedIndex == 0)
                {
                    usrInfo5.Children.Clear();
                    donorInfo6.Children.Add(edit);
                    donorInfo6.Children.Add(saveChanges);
                    donorInfo6.Children.Add(cancel);

                    if (isEditable)
                    {
                        oldPassBox.IsEnabled = false;
                        nwPassBox.IsEnabled = false;
                        ConfirmPassBox.IsEnabled = false;

                        oldPassBox.Background = Brushes.LightGray;
                        nwPassBox.Background = Brushes.LightGray;
                        ConfirmPassBox.Background = Brushes.LightGray;

                        isEditable = false;       
                    }
                }
                else if (tabControl.SelectedIndex == 1)
                {
                    donorInfo6.Children.Clear();
                    usrInfo5.Children.Add(edit);
                    usrInfo5.Children.Add(saveChanges);
                    usrInfo5.Children.Add(cancel);

                    if (isEditable)
                    {
                         name.IsEnabled = false;
                         surname.IsEnabled = false;
                         address.IsEnabled = false;
                         phoneNumber.IsEnabled = false;
                         bloodType.IsEnabled = false;

                         name.Background = Brushes.LightGray;
                         surname.Background = Brushes.LightGray;
                         address.Background = Brushes.LightGray;
                         phoneNumber.Background = Brushes.LightGray;
                         bloodType.Background = Brushes.LightGray;

                         isEditable = false;                        
                    }          
                }
            }

            void Add_Click(object sender, RoutedEventArgs e)
            {
                string query = "INSERT INTO Donors (DonorName, DonorSurname, Address, PhoneNum, BloodType) VALUES (@name, @surname, @address, @phoneNum, @bloodType)";

                string defaultDonor = "Donor " + counter;

                conn.Open();

                OleDbCommand command = new OleDbCommand(query, conn);
                command.Parameters.AddWithValue("@name", defaultDonor);
                command.Parameters.AddWithValue("@surname", defaultDonor);
                command.Parameters.AddWithValue("@address", defaultDonor);
                command.Parameters.AddWithValue("@phoneNum", counter);
                command.Parameters.AddWithValue("@bloodType", defaultDonor);
            
                command.ExecuteScalar();

                ShowInfo(comboBox.SelectedValue.ToString());

                counter++;

                conn.Close();
            }

            void Delete_Click(object sender, RoutedEventArgs e)
            {
                if (listBox.SelectedItem != null)
                {
                    conn.Open();

                    string query = @"DELETE FROM Donors WHERE [Id] = @id AND [DonorName] = @name AND [DonorSurname] = @surname AND [Address] = @address AND [PhoneNum] = @phoneNum AND [BloodType] = @bloodType";

                    OleDbCommand oleDbCommand = new OleDbCommand(query, conn);
                    oleDbCommand.Parameters.AddWithValue("@id", donorTable.Rows[listBox.SelectedIndex]["ID"].ToString());
                    oleDbCommand.Parameters.AddWithValue("@name", donorTable.Rows[listBox.SelectedIndex]["DonorName"].ToString());
                    oleDbCommand.Parameters.AddWithValue("@surname", donorTable.Rows[listBox.SelectedIndex]["DonorSurname"].ToString());
                    oleDbCommand.Parameters.AddWithValue("@address", donorTable.Rows[listBox.SelectedIndex]["Address"].ToString());
                    oleDbCommand.Parameters.AddWithValue("@phoneNum", donorTable.Rows[listBox.SelectedIndex]["PhoneNUm"].ToString());
                    oleDbCommand.Parameters.AddWithValue("@bloodType", donorTable.Rows[listBox.SelectedIndex]["BloodType"].ToString());

                    oleDbCommand.ExecuteScalar();

                    conn.Close();

                    ShowInfo(comboBox.SelectedValue.ToString());

                    listBox.SelectedItem = null;
                    initDonor.Clear();
                    name.Text = "";
                    surname.Text = "";
                    address.Text = "";
                    phoneNumber.Text = "";
                    bloodType.Text = "";
                }
                else
                {
                    delete.IsEnabled = false;
                }            
            }

            void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (listBox.Items.Count > 0)
                {
                    delete.IsEnabled = true;

                    if(listBox.SelectedItem != null)
                    {                        
                        if (initDonor.Count != 0)
                        {
                            if (name.Text.Equals(initDonor.ElementAt(0)) && surname.Text.Equals(initDonor.ElementAt(1)) && address.Text.Equals(initDonor.ElementAt(2)) && phoneNumber.Text.Equals(initDonor.ElementAt(3)) && bloodType.Text.Equals(initDonor.ElementAt(4)))
                            {
                                initDonor.Clear();

                                name.Text = donorTable.Rows[listBox.SelectedIndex]["DonorName"].ToString();
                                surname.Text = donorTable.Rows[listBox.SelectedIndex]["DonorSurname"].ToString();
                                address.Text = donorTable.Rows[listBox.SelectedIndex]["Address"].ToString();
                                phoneNumber.Text = donorTable.Rows[listBox.SelectedIndex]["PhoneNum"].ToString();
                                bloodType.Text = donorTable.Rows[listBox.SelectedIndex]["BloodType"].ToString();

                                initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["DonorName"].ToString());
                                initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["DonorSurname"].ToString());
                                initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["Address"].ToString());
                                initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["PhoneNum"].ToString());
                                initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["BloodType"].ToString());
                                initDonor.Add(listBox.SelectedIndex.ToString());
                            }
                            else
                            {
                                if (listBox.SelectedIndex != Int32.Parse(initDonor.ElementAt(5)))
                                {
                                    MessageBoxResult result = MessageBox.Show("All unsaved changes will be lost. Do you want to continue?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                                    if (result == MessageBoxResult.Yes)
                                    {
                                        initDonor.Clear();
                                        name.Text = donorTable.Rows[listBox.SelectedIndex]["DonorName"].ToString();
                                        surname.Text = donorTable.Rows[listBox.SelectedIndex]["DonorSurname"].ToString();
                                        address.Text = donorTable.Rows[listBox.SelectedIndex]["Address"].ToString();
                                        phoneNumber.Text = donorTable.Rows[listBox.SelectedIndex]["PhoneNum"].ToString();
                                        bloodType.Text = donorTable.Rows[listBox.SelectedIndex]["BloodType"].ToString();

                                        initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["DonorName"].ToString());
                                        initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["DonorSurname"].ToString());
                                        initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["Address"].ToString());
                                        initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["PhoneNum"].ToString());
                                        initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["BloodType"].ToString());
                                        initDonor.Add(listBox.SelectedIndex.ToString());
                                    }
                                    else
                                    {
                                        listBox.SelectedIndex = Int32.Parse(initDonor.ElementAt(5));
                                    }
                                }                                
                            }
                        }
                        else
                        {
                            name.Text = donorTable.Rows[listBox.SelectedIndex]["DonorName"].ToString();
                            surname.Text = donorTable.Rows[listBox.SelectedIndex]["DonorSurname"].ToString();
                            address.Text = donorTable.Rows[listBox.SelectedIndex]["Address"].ToString();
                            phoneNumber.Text = donorTable.Rows[listBox.SelectedIndex]["PhoneNum"].ToString();
                            bloodType.Text = donorTable.Rows[listBox.SelectedIndex]["BloodType"].ToString();

                            initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["DonorName"].ToString());
                            initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["DonorSurname"].ToString());
                            initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["Address"].ToString());
                            initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["PhoneNum"].ToString());
                            initDonor.Add(donorTable.Rows[listBox.SelectedIndex]["BloodType"].ToString());
                            initDonor.Add(listBox.SelectedIndex.ToString());
                        }
                    }                    
                }                   
                else
                    delete.IsEnabled = false;
            }
        }        

        private void ShowInfo(string type)
        {

            if (searchBox.Text.Equals("Search For Donors"))
            {
                query = "SELECT * FROM Donors ORDER BY DonorName";
            }
            else if (searchBox.Text.Equals("Search For Blood Types"))
            {
                query = "SELECT * FROM Donors ORDER BY BloodType";
            }
            else
            {
                if (type.Equals("Donors"))
                {
                    query = "SELECT * FROM Donors WHERE DonorName LIKE '" + searchBox.Text + "%' ORDER BY DonorName";
                }
                else if (type.Equals("Blood Types"))
                {
                    query = "SELECT * FROM Donors WHERE BloodType LIKE '" + searchBox.Text + "%' ORDER BY BloodType";
                }
            }
            
            conn.Close();

            try
            {
                OleDbCommand oleDbCmd = new OleDbCommand(query, conn);

                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCmd);

                using (oleDbDataAdapter)
                {
                    donorTable = new DataTable();

                    oleDbDataAdapter.Fill(donorTable);

                    listBox.Items.Clear();

                    for (int i = 0; i < donorTable.Rows.Count; i++)
                    {
                        if (type.Equals("Donors"))
                        {
                            listBox.Items.Add(donorTable.Rows[i]["DonorName"].ToString() + " " + donorTable.Rows[i]["DonorSurname"].ToString());
                        }
                        else if (type.Equals("Blood Types"))
                        {
                            listBox.Items.Add(donorTable.Rows[i]["BloodType"].ToString());
                        }                        
                    }                                     
                }
            }
            catch (Exception e)
            {
                //initially, if there is not entry in the database, it will throw an error
                //this part prevents that
            }
        }
    }
}
