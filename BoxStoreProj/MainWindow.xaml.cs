using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BoxLib;

namespace BoxStoreProj
{
    public partial class MainWindow : Window
    {
        SoundPlayer player;
        Organizer organizer;
        Params _params;
        TimeSpan exprationDate, timeInterval;
        int maxCapacity, minCapacity;

        public MainWindow()
        {
            InitializeComponent();
            GetParametersFromAppConfig();
            organizer = new Organizer(_params = new Params(exprationDate, timeInterval, maxCapacity, minCapacity));
            player = new SoundPlayer("C:\\Users\\osher\\source\\repos\\BoxStoreProj\\BoxStoreProj\\Assets\\OakLabTrimmed.wav");
            PlayTune();
            MessageBox.Show("Welcome to Box Management Software V1.1 By Osher Dror", "BoxManagement");
            organizer.LoadItems("boxes.json");
            ApplyText();
        }

        private void ApplyText()
        {
            MaxCapacity.Text += _params.MaxCapacity;
            MinCapacity.Text += _params.MinCapacity;
        }

        private void GetParametersFromAppConfig()
        {

            if (ConfigurationManager.AppSettings.AllKeys.ToList().Contains("exprationDate"))
                TimeSpan.TryParse(ConfigurationManager.AppSettings["exprationDate"], out exprationDate);
            else
                exprationDate = TimeSpan.FromDays(7);
            if (ConfigurationManager.AppSettings.AllKeys.ToList().Contains("timeInterval"))
                TimeSpan.TryParse(ConfigurationManager.AppSettings["timeInterval"], out timeInterval);
            else
                timeInterval = TimeSpan.FromMinutes(10);
            if (ConfigurationManager.AppSettings.AllKeys.ToList().Contains("maxCapacity"))
                int.TryParse(ConfigurationManager.AppSettings["maxCapacity"], out maxCapacity);
            else
                maxCapacity = 100;
            if (ConfigurationManager.AppSettings.AllKeys.ToList().Contains("minCapacity"))
                int.TryParse(ConfigurationManager.AppSettings["minCapacity"], out minCapacity);
            else
                minCapacity = 5;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            organizer.SaveItems("boxes.json");
        }

        private void Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)

                if (!(Char.IsDigit(ch) || ch.Equals('.')))
                {
                    e.Handled = true;
                }
        }

        private void Num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (TBX.Text != "" && TBY.Text != "" && TBQuantity.Text != "")
            {
                if (double.TryParse(TBX.Text, out double x) && double.TryParse(TBY.Text, out double y) && int.TryParse(TBQuantity.Text, out int quantity))
                {
                    if (x > 0 && y > 0 && quantity > 0)
                    {
                        MessageBox.Show(organizer.AddBox(x, y, quantity, DateTime.Now), "BoxManagement");
                        return;
                    }
                    MessageBox.Show("X /Y /Quantity Cannot be 0 or lower!\nPlease check your details", "BoxManagement");
                    return;
                }
                MessageBox.Show("Please fill the details correctly!\nX Size must be a Number value (like 13 or 8.93)\nY Size must be a Number value (for example 13 or 8.93)\nQuantity must be a number value (like 29, a whole number!)", "BoxManagement");
            }
            else
                MessageBox.Show("Some of the text boxes are empty, Please fill all the details!", "BoxManagement");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ClearAllClick();
        }

        private void CheckQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (TBXCQ.Text != "" && TBYCQ.Text != "")
            {
                if (double.TryParse(TBXCQ.Text, out double x) && double.TryParse(TBYCQ.Text, out double y))
                {
                    if (organizer.CheckQuantity(x, y) == null)
                    {
                        MessageBox.Show("Item not found!", "BoxManagement");
                        return;
                    }
                    string quantity;
                    quantity = organizer.CheckQuantity(x, y).ToString();
                    MessageBox.Show("Quantity of searched item is: " + quantity, "BoxManagement");
                    CQTCheckQuantity.Text = quantity;
                    return;
                }
                MessageBox.Show("Please fill the details correctly!\nX Size must be a Number value (like 13 or 8.93)\nY Size must be a Number value (for example 13 or 8.93)", "BoxManagement");
            }
            else
                MessageBox.Show("X box or Y box cannot be empty, Please fill all the details!", "BoxManagement");
        }

        private void SubmitSellTab_Click(object sender, RoutedEventArgs e)
        {
            if (TBXSellTab.Text != "" && TBYSellTab.Text != "" && TBQuantitySellTab.Text != "")
            {
                if (double.TryParse(TBXSellTab.Text, out double x) && double.TryParse(TBYSellTab.Text, out double y) && int.TryParse(TBQuantitySellTab.Text, out int quantity) && x>0 && y>0 && quantity>0)
                {
                    if (organizer.CheckQuantity(x, y) - quantity < _params.MinCapacity)
                        if (MessageBox.Show($"Quantity now at {organizer.CheckQuantity(x, y) }\nAre you sure want to sell below Mininmum ({_params.MinCapacity})? ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                            return;
                        else
                        {
                            MessageBox.Show(organizer.SellBox(x, y, quantity), "BoxManagement");
                            return;
                        }
                    MessageBox.Show(organizer.SellBox(x, y, quantity), "BoxManagement");
                    return;
                }
                MessageBox.Show("Please fill the details correctly!\nX Size must be a Number value (like 13 or 8.93)\nY Size must be a Number value (for example 13 or 8.93)\nQuantity must be a number value (like 29, a whole number!)\nAdditional Info:\nValues given cannot be 0 or lower!", "BoxManagement");
            }
            else
                MessageBox.Show("Some of the text boxes are empty, Please fill all the details!", "BoxManagement");
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            var listOfItems = organizer.MakeList().ToList();

            if (listOfItems == null || listOfItems.Count == 0)
            {
                ListTab.ItemsSource = null;
                MessageBox.Show("Right now there is not a single item!\n Go to Add Box tab to start adding!", "BoxManagement");
                return;
            }

            ListTab.ItemsSource = listOfItems;
            MessageBox.Show("All items has been loaded successfully!", "BoxManagement");
        }
        private void PlayTune()
        {
            player.Load();
            player.PlayLooping();
            PlayMusic.IsEnabled = false;
            StopMusic.IsEnabled = true;
        }
        private void PlayMusic_Click(object sender, RoutedEventArgs e)
        {
            PlayTune();
        }

        private void StopMusic_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            StopMusic.IsEnabled = false;
            PlayMusic.IsEnabled = true;
        }

        private void ItemDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ListTab.SelectedItem != null)
            {
                var item = ListTab.SelectedItem as Details;
                if (item != null)
                    MessageBox.Show(item.ToString(), "BoxManagement");
            }
            else
                MessageBox.Show("Please choose an item from the list to display Item's details", "BoxManagement");
        }

        private void ClearAllClick()
        {
            CQTCheckQuantity.Text = TBXCQ.Text = TBYCQ.Text = String.Empty;
            TBXSellTab.Text = TBYSellTab.Text = TBQuantitySellTab.Text = String.Empty;
            TBX.Text = TBY.Text = TBQuantity.Text = String.Empty;
            ListTab.ItemsSource = null;
            MessageBox.Show("Form cleared", "BoxManagement");
        }
    }
}