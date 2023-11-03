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

namespace ibakov_autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();

       
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();

            if(SelectedService != null)
                this._currentService=SelectedService;

            DataContext = _currentService;

            var _currentClient = Ibakov_autoserviceEntities.GetContext().client_a_import.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private ClientService _currentClientService = new ClientService();
        private void SaveBut0ton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if(errors.Length>0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(_currentClientService);

            if (_currentClientService.ID == 0)
                Ibakov_autoserviceEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                Ibakov_autoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            if (s.Length < 3 || s.Length > 5 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(new char[] { ':' });
                try
                {

                    if (Convert.ToInt32(start[0].ToString()) >= 0 && Convert.ToInt32(start[0].ToString()) <= 23 && Convert.ToInt32(start[1].ToString()) >= 0 && Convert.ToInt32(start[1].ToString()) <= 59 && start[1].Length == 2)
                    {
                        int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                        int startMin = Convert.ToInt32(start[1].ToString());

                        int sum = startHour + startMin + _currentService.DurationInSeconds;

                        string EndHour = (sum / 60 % 24).ToString();
                        string EndMin = (sum % 60).ToString();
                        if (Convert.ToInt32(EndMin) / 10 == 0)
                        {
                            EndMin = '0' + EndMin;
                        }
                        s = EndHour.ToString() + ":" + EndMin;
                        TBEnd.Text = s;
                    }
                    else
                    {
                        TBEnd.Text = "";
                    }
                }
                catch
                {
                    TBEnd.Text = "";
                }
            }
        }

        private void TBStart_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
