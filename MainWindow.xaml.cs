using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace halloween_party
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			CostumeList.ItemsSource = Guest.Costumes;
		}

		public void Render()
		{
			GuestName.Text = "";
			GuestAge.Text = "";
			CostumeList.SelectedIndex = -1;

			GuestList.Items.Clear();
			foreach (Guest guest in Guest.Guests)
			{
				GuestList.Items.Add(guest);
			}
		}

		private void AddGuest(object sender, RoutedEventArgs e)
		{
			string name = GuestName.Text;
			int age = GuestAge.Text == "" ? 0 : int.Parse(GuestAge.Text);
			string costume = CostumeList.Text;

			if (name == "" || age == 0 || costume == "")
			{
				MessageBox.Show("Minden mezőt ki kell tölteni!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Guest.Guests.Add(new Guest(name, age, costume));

			RotateImage();
			Render();
		}

		private void DeleteGuest(object sender, RoutedEventArgs e)
		{
			if (GuestList.SelectedItem == null)
			{
				return;
			}

			MessageBoxResult confirmed = MessageBox.Show("Biztosan törölni akarod a vendéget?", "Törlés", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (confirmed == MessageBoxResult.Yes)
			{
				Guest.Guests.Remove((Guest)GuestList.SelectedItem);
				Render();
			}
			else
			{
				GuestList.SelectedItem = null;
				return;
			}
		}

		private void CostumeStatistics(object sender, RoutedEventArgs e)
		{
			if(Guest.Guests.Count == 0)
			{
				MessageBox.Show("Nincs vendég az adatbázisban!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			Dictionary<string, int> costumeCount = new Dictionary<string, int>();

			foreach (Guest guest in Guest.Guests)
			{
				if(costumeCount.ContainsKey(guest.Costume))
				{
					costumeCount[guest.Costume] += 1;
					continue;
				}

				costumeCount[guest.Costume] = 1;
			}

			string costumeStats = "";
			foreach (KeyValuePair<string, int> costume in costumeCount)
			{
				costumeStats += $"{costume.Key}: {costume.Value}\n";
			}

			MessageBox.Show(costumeStats, "Jelmez szerinti statisztika");
		}

		private void ImportGuests(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "txt (*.txt)|*.txt|csv (*.csv)|*.csv";

			if (openFileDialog.ShowDialog() == false) return;

			try
			{
				Guest.LoadGuests(openFileDialog.FileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			RotateImage();
			Render();
		}

		private void ExportGuests(object sender, RoutedEventArgs e)
		{
			if(Guest.Guests.Count == 0)
			{
				MessageBox.Show("Nincs vendég az adatbázisban!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "txt (*.txt)|*.txt|csv (*.csv)|*.csv";

			if (saveFileDialog.ShowDialog() == false) return;

			Guest.SaveGuests(saveFileDialog.FileName);
			MessageBox.Show("Sikeres exportálás!", "Siker!", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void OnlyAllowNumbers(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		public void RotateImage()
		{
			DoubleAnimation rotateAnimation = new DoubleAnimation();
			rotateAnimation.From = 0;
			rotateAnimation.To = 360;
			rotateAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

			RotateTransform rotateTransform = new RotateTransform();
			PumpkinImage.RenderTransform = rotateTransform;
			PumpkinImage.RenderTransformOrigin = new Point(0.5, 0.5);
			rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
		}

	}
}