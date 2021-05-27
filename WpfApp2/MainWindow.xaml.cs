using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Account> Users = new List<Account>();
        bool RegMode = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        void ScaleText()
        {
            MailLabel.FontSize = Sign_In.Height / 35;
            NameLabel.FontSize = Sign_In.Height / 35;
            PasswordLabel.FontSize = Sign_In.Height / 35;
            SecondNameLabel.FontSize = Sign_In.Height / 35;
            PasswordLabel2.FontSize = Sign_In.Height / 35;
            DateLabel.FontSize = Sign_In.Height / 35;
            MailLabel.FontSize = Sign_In.Height / 35;
            GenderLabel.FontSize = Sign_In.Height / 35;
            Terms.FontSize = Sign_In.Height / 35;
            DoneButton.FontSize = Sign_In.Height / 35;
            SwapButton.FontSize = Sign_In.Height / 35;
        }

        void Sign_In_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Sign_In.Width = Sign_In.Height / 1.3;
            ScaleText();
        }

        void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            string[] fields = new string[5];
            /*
             0 - first name
            1 - password
            2 - mail
            3 - second name
            4 - gender
             */
            DateTime? BirthDay = default;
            List<string> errors = new List<string>();
            if (FirstNameTextBox.Text.Length >= 3)
            {
                fields[0] = FirstNameTextBox.Text;
                NameError.Visibility = Visibility.Hidden;
            }
            else
            {
                errors.Add("Имя слишком короткое!");
                NameError.Visibility = Visibility.Visible;
            }
            if (RegMode)    //If you're creating new account
            {
                if ((PasswordTextBox.Text == PasswordTextBox2.Text) & (PasswordTextBox.Text.Length >= 8))
                {
                    fields[1] = PasswordTextBox.Text;
                    PasswordError.Visibility = Visibility.Hidden;
                    PasswordError2.Visibility = Visibility.Hidden;
                }
                else
                {
                    if (PasswordTextBox.Text != PasswordTextBox2.Text)
                    {
                        errors.Add("Пароли не совпадают!");
                        PasswordError2.Visibility = Visibility.Visible;
                    }
                    if (PasswordTextBox.Text != PasswordTextBox2.Text)
                    {
                        errors.Add("Пароль должен состоять минимум из 8 символов!");
                        PasswordError2.Visibility = Visibility.Visible;
                    }
                }
                if (MailTextBox.Text.Contains("@"))
                {
                    fields[2] = MailTextBox.Text;
                    MailError.Visibility = Visibility.Hidden;
                }
                else
                {
                    errors.Add($"Некорректный адрес почты! Возможно, в нём нет '@'?");
                    MailError.Visibility = Visibility.Visible;
                }
                fields[3] = SecondNameTextBox.Text;
                if (BirthSelecter.SelectedDate >= new DateTime(1900, 1, 1) | (BirthSelecter.SelectedDate < DateTime.Now.AddYears(-6)))
                {
                    BirthDay = BirthSelecter.SelectedDate;
                    DateError.Visibility = Visibility.Hidden;
                }
                else
                {
                    if (BirthSelecter.SelectedDate < new DateTime(1900, 1, 1))
                        errors.Add("Вы слишком стары для создания аккаунта...");
                    if (BirthSelecter.SelectedDate < DateTime.Now.AddYears(-6))
                        errors.Add("Вы слишком малы для создания аккаунта...");
                    DateError.Visibility = Visibility.Visible;
                }
                if (GenderSelecter.SelectedItem != null)
                {
                    fields[4] = GenderSelecter.SelectedItem.ToString();
                    GenderError.Visibility = Visibility.Hidden;
                }
                else
                {
                    errors.Add("Вы не выбрали свой пол!");
                    GenderError.Visibility = Visibility.Visible;
                }
            }
            if (errors.Count > 0)
            {
                string err = "";
                foreach (string msg in errors)
                {
                    err += msg + "\n";
                }
                MessageBox.Show(err);
                errors.Clear();
            }
            else
            {
                if (RegMode)
                    Users.Add(new Account(fields[0], fields[2], fields[1], fields[3], (DateTime)BirthDay, fields[4]));
                // Here should be ELSE block but I didn't make a Data Base or JSON saving
            }
        }
    }

    class Account
    {
        static Dictionary<string, byte> GenderValuePairs = new Dictionary<string, byte>
        {
            {"Other", 0},
            {"Male", 1},
            {"Female", 2},
            {"Robot", 3}
        };
        public string Name
        {
            get
            {
                return Name;
            }
            internal set
            {
                if (!long.TryParse(value, out long _) & !char.TryParse(value, out char _))
                    Name = value;
                else
                {
                    Console.WriteLine("Недопустимое имя аккаунта");
                }
            }
        }
        public string SecondName
        {
            get
            {
                return SecondName;
            }
            internal set
            {
                if (!long.TryParse(value, out long _) & !char.TryParse(value, out char _))
                    Name = value;
                else
                    Console.WriteLine("Недопустимое второе имя");
            }
        }
        public DateTime Date
        {
            get
            {
                return Date;
            }
            private set
            {
                if (value > new DateTime(1900, 1, 1) & value < DateTime.Now.AddYears(-6))
                    Date = value;
                else
                    Console.WriteLine("Недопустимая дата рождения");
            }
        }
        public byte Gender
        {
            get
            {
                return Gender;
            }
            private set
            {
                if (value >= 0 & value <= 3)
                    Gender = value;
            }
        }
        private string password, mail;
        public Account(string name, string mail, string password, string second = null, DateTime date = default, string gender = null)
        {
            Name = name;
            this.mail = mail;
            this.password = password;
            if (second != "")
                SecondName = second;
            if (date != null)
                Date = date;
            if (gender != null)
            {
                if (GenderValuePairs.TryGetValue(gender, out byte _))
                    Gender = GenderValuePairs[gender];
                else
                    Console.WriteLine("Gender value вне диапазона допустимых значений");
            }
        }
    }
}
