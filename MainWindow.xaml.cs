using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace ValorantAccountGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string GenerateEmail()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var email = new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray()) + "@changeme.xyz";
            return email;
        }

        private string GenerateUsername()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var username = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());
            return username;
        }

        private string GeneratePassword()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
            return password;
        }

        private (string day, string month, string year) GenerateBirthDate()
        {
            Random rnd = new Random();
            DateTime start = new DateTime(1990, 1, 1);
            DateTime end = new DateTime(2005, 12, 31);
            int range = (end - start).Days;
            DateTime randomDate = start.AddDays(rnd.Next(range));
            return (randomDate.Day.ToString("00"), randomDate.Month.ToString("00"), randomDate.Year.ToString());
        }

        private void ValoGen(string email, string username, string password)
        {
            var (birthDay, birthMonth, birthYear) = GenerateBirthDate();
            string baseUrl = "https://auth.riotgames.com/login#client_id=play-valorant-web-prod&nonce=NzcsMTA2LDEwMCwx&prompt=signup&redirect_uri=https%3A%2F%2Fplayvalorant.com%2Fopt_in%2F%3Fredirect%3D%2Fdownload%2F&response_type=token%20id_token&scope=account%20openid&state=c2lnbnVw&ui_locales=en";

            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--window-size=1920,1200");
            IWebDriver driver = new FirefoxDriver(options);

            try
            {
                driver.Navigate().GoToUrl(baseUrl);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20)); // Erhöhte Wartezeit

                // Warte auf die Eingabefelder und fülle die Werte aus
                var emailField = wait.Until(d => d.FindElement(By.Name("email")));
                emailField.SendKeys(email);
                Thread.Sleep(1000); // Kurze Pause

                // Klicke den nächsten Button
                var nextBtn = driver.FindElement(By.XPath("//button[@type='submit']"));
                ScrollToElement(driver, nextBtn); // Scrolle zum Button
                ClickElement(driver, nextBtn); // Klicke per JavaScript
                Thread.Sleep(1000); // Kurze Pause

                // Warte auf das Geburtsdatum-Felder und fülle sie aus
                var dayField = wait.Until(d => d.FindElement(By.Name("date_of_birth_day")));
                var monthField = driver.FindElement(By.Name("date_of_birth_month"));
                var yearField = driver.FindElement(By.Name("date_of_birth_year"));

                dayField.SendKeys(birthDay);
                monthField.SendKeys(birthMonth);
                yearField.SendKeys(birthYear);
                Thread.Sleep(1000); // Kurze Pause

                // Klicke den nächsten Button
                nextBtn = driver.FindElement(By.XPath("//button[@type='submit']"));
                ScrollToElement(driver, nextBtn); // Scrolle zum Button
                ClickElement(driver, nextBtn); // Klicke per JavaScript
                Thread.Sleep(1000); // Kurze Pause

                // Warte auf das Benutzername-Feld und fülle es aus
                var usernameField = wait.Until(d => d.FindElement(By.Name("username")));
                usernameField.SendKeys(username);
                Thread.Sleep(1000); // Kurze Pause

                // Klicke den nächsten Button
                nextBtn = driver.FindElement(By.XPath("//button[@type='submit']"));
                ScrollToElement(driver, nextBtn); // Scrolle zum Button
                ClickElement(driver, nextBtn); // Klicke per JavaScript
                Thread.Sleep(1000); // Kurze Pause

                // Warte auf die Passwortfelder und fülle sie aus
                var passwordField = wait.Until(d => d.FindElement(By.Name("password")));
                var confirmPasswordField = driver.FindElement(By.Name("confirm_password"));

                passwordField.SendKeys(password);
                confirmPasswordField.SendKeys(password);
                Thread.Sleep(1000); // Kurze Pause

                // Klicke den Bestätigungs-Button
                nextBtn = driver.FindElement(By.XPath("//button[@type='submit']"));
                ScrollToElement(driver, nextBtn); // Scrolle zum Button
                ClickElement(driver, nextBtn); // Klicke per JavaScript

                // Optionale: CAPTCHA oder andere zusätzliche Schritte hier behandeln

                MessageBox.Show("Account creation completed. Please complete the CAPTCHA if required.");

                // Speichern der Account-Daten im accounts-Ordner
                SaveAccountDetails(email, username, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                driver.Quit();
            }
        }

        private void SaveAccountDetails(string email, string username, string password)
        {
            try
            {
                string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                int fileIndex = 1;
                string filePath;

                do
                {
                    filePath = Path.Combine(directoryPath, $"account{fileIndex}.txt");
                    fileIndex++;
                } while (File.Exists(filePath));

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"Creation Time: {DateTime.Now}");
                    sw.WriteLine($"Email: {email}");
                    sw.WriteLine($"Username: {username}");
                    sw.WriteLine($"Password: {password}");
                    sw.WriteLine("---------------------------");
                }

                MessageBox.Show($"Account details saved to {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save account details: {ex.Message}");
            }
        }

        private void ScrollToElement(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        private void ClickElement(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", element);
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var email = GenerateEmail();
            var username = GenerateUsername();
            var password = GeneratePassword();

            emailTextBox.Text = email;
            usernameTextBox.Text = username;
            passwordTextBox.Text = password;

            ValoGen(email, username, password);
        }

        private void CopyEmailButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(emailTextBox.Text);
        }

        private void CopyUsernameButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(usernameTextBox.Text);
        }

        private void CopyPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(passwordTextBox.Text);
        }

        private void SaveAccountDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please generate account details before saving.");
                return;
            }

            SaveAccountDetails(email, username, password);
        }
    }
}
