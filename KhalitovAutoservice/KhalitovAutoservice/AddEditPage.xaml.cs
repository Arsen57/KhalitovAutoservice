﻿using System;
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

namespace KhalitovAutoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentServise = new Service();
        bool CheckService = false;
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentServise = SelectedService;

            DataContext = _currentServise;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentServise.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentServise.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (_currentServise.Discount < 0 || _currentServise.Discount > 100)
                errors.AppendLine("Укажите скидку");
            
            if (string.IsNullOrWhiteSpace(_currentServise.Discount.ToString()))
                _currentServise.Discount = 0;
            
            if (_currentServise.DurationInSeconds == 0)
                errors.AppendLine("Укажите длительность услуги");

            if (_currentServise.DurationInSeconds > 240 || _currentServise.DurationInSeconds < 0)
                errors.AppendLine("Длительность не может быть больше 240 минут или  меньше 0 минут");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = Khalitov_autoserviceEntities1.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentServise.Title).ToList();

            if (allServices.Count == 0 || (_currentServise.ID !=0 && allServices.Count <=1))
            {
                if (_currentServise.ID == 0)
                    Khalitov_autoserviceEntities1.GetContext().Service.Add(_currentServise);
                try
                {
                    Khalitov_autoserviceEntities1.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }
            if (_currentServise.ID == 0)
            {
                Khalitov_autoserviceEntities1.GetContext().Service.Add(_currentServise);
            }
        }
    }
}
