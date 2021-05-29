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
            Action[] TextWipe = new Action[]
                {
                    FirstNameTextBox.Clear,
                    SecondNameTextBox.Clear,
                    PasswordTextBox.Clear,
                    PasswordTextBox2.Clear,
                    MailTextBox.Clear
                };
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
            if (RegMode)    //If you're creating new account
            {
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
                if ((PasswordTextBox.Text == PasswordTextBox2.Text) && (PasswordTextBox.Text.Length >= 8))
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
                {
                    Users.Add(new Account(fields[0], fields[2], fields[1], fields[3], (DateTime)BirthDay, fields[4]));
                    for (int i = 0; i < 5; i++)
                        fields[i] = null;
                    foreach (Action method in TextWipe)
                        method.Invoke();
                    GenderSelecter.SelectedIndex = default;
                    BirthSelecter.SelectedDate = default;
                }
                // Here should be ELSE block but I didn't make a Data Base or JSON saving
            }
        }

        void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            RegMode = !RegMode;
            if (RegMode)
            {
                PasswordTextBox2.Visibility = Visibility.Visible;
                PasswordLabel2.Visibility = Visibility.Visible;
                MailTextBox.Visibility = Visibility.Visible;
                MailLabel.Visibility = Visibility.Visible;
                SecondNameTextBox.Visibility = Visibility.Visible;
                SecondNameLabel.Visibility = Visibility.Visible;
                BirthSelecter.Visibility = Visibility.Visible;
                DateLabel.Visibility = Visibility.Visible;
                GenderSelecter.Visibility = Visibility.Visible;
                GenderLabel.Visibility = Visibility.Visible;
                MainLabel.Content = "Регистрация";
                Terms.Content = "I agree all rules...";
            }
            else
            {
                PasswordTextBox2.Visibility = Visibility.Hidden;
                PasswordLabel2.Visibility = Visibility.Hidden;
                MailTextBox.Visibility = Visibility.Hidden;
                MailLabel.Visibility = Visibility.Hidden;
                SecondNameTextBox.Visibility = Visibility.Hidden;
                SecondNameLabel.Visibility = Visibility.Hidden;
                BirthSelecter.Visibility = Visibility.Hidden;
                DateLabel.Visibility = Visibility.Hidden;
                GenderSelecter.Visibility = Visibility.Hidden;
                GenderLabel.Visibility = Visibility.Hidden;
                MainLabel.Content = "Авторизация";
                Terms.Content = "I'm not a robot";
            }
        }

        void Terms_Click(object sender, RoutedEventArgs e)
        {
            DoneButton.IsEnabled = (bool)Terms.IsChecked;
        }
    }

    class Account
    {
        string name, second_name, password, mail;
        byte gender;
        DateTime date;
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
                return name;
            }
            internal set
            {
                if (!long.TryParse(value, out long _) & !char.TryParse(value, out char _))
                    name = value;
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
                return second_name;
            }
            internal set
            {
                if (!long.TryParse(value, out long _) & !char.TryParse(value, out char _))
                    second_name = value;
                else
                    Console.WriteLine("Недопустимое второе имя");
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
            private set
            {
                if (value > new DateTime(1900, 1, 1) & value < DateTime.Now.AddYears(-6))
                    date = value;
                else
                    Console.WriteLine("Недопустимая дата рождения");
            }
        }
        public byte Gender
        {
            get
            {
                return gender;
            }
            private set
            {
                if (value >= 0 & value <= 3)
                    gender = value;
            }
        }
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
