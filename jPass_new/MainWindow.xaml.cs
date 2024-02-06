using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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



namespace jPass_new
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        class MainWindow_lm
        {
            public string[] header_lb = new string[2] { "Get in jointPass", "Войти в ДжойнтПасс" };
            public string[] header_lb_change = new string[2] { "Change Master Pass", "Изменить Master Pass" };
            public string[] submit_btn = new string[2] { "Submit", "Принять" };
            public string[] mpEnter_lb = new string[2] { "Enter master pass", "Введите master pass" };
            public string[] mpEnter_lb_change = new string[2] { "Enter new master pass", "Введите новый master pass" };
            public string[] mpRepeat_lb = new string[2] { "Repeat master pass", "Повторите master pass" };
            public string[] mpRepeat_lb_change = new string[2] { "Repeat new master pass", "Повторите новый master pass" };
            public string[] winTitle = new string[2] { "Autorization", "Авторизация" };
            public string[] dtdir_lb = new string[2] { "data_dir: ", "Папка данных: " };
            public string[] changePass_chb = new string[2] { "change password: ", "Изменить пароль: " };

        }

        private void useMv_lm()
        {
            if (jPass.lang == 1)
            {
                lang_rb.IsChecked = true;
            }
            else
            {
                lang_rb1.IsChecked = true;
            }
            submit_btn.Content = mainWindow_Lm.submit_btn[jPass.lang];
            Title = mainWindow_Lm.winTitle[jPass.lang];
            dtdir_lb.Content = mainWindow_Lm.dtdir_lb[jPass.lang];
            changePass_chb.Content = mainWindow_Lm.changePass_chb[jPass.lang];

            if (changePass_chb.IsChecked == true)
            {
                header_lb.Content = mainWindow_Lm.header_lb_change[jPass.lang];
                mpEnter_lb.Content = mainWindow_Lm.mpEnter_lb_change[jPass.lang];
                mpRepeat_lb.Content = mainWindow_Lm.mpRepeat_lb_change[jPass.lang];
            }
            else
            {
                header_lb.Content = mainWindow_Lm.header_lb[jPass.lang];
                mpEnter_lb.Content = mainWindow_Lm.mpEnter_lb[jPass.lang];
                mpRepeat_lb.Content = mainWindow_Lm.mpRepeat_lb[jPass.lang];
            }
        }

        MainWindow_lm mainWindow_Lm = new MainWindow_lm();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog().ToString() == "OK") {


                    string[] parts = dialog.SelectedPath.Split('\\');

                    if (parts[parts.Length - 1] != "jPass_data")
                    {
                        data_dir_tb.Text = "";
                            data_dir_tb.Text = dialog.SelectedPath + "\\jPass_data";
                    }
                    else
                    {
                        data_dir_tb.Text = dialog.SelectedPath;
                    }
                }
            }           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            error_lb.Visibility = Visibility.Hidden;
            changePass_chb.Visibility = Visibility.Hidden;

            if (!File.Exists(jpOptions.exec_path + "\\" + jpOptions.ini_file))
            {           
                using (var tw = new StreamWriter(jpOptions.exec_path + "\\" + jpOptions.ini_file, true))
                {
                }
            }

            string data_dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\jPass_data";

            string[] ini_lines = File.ReadLines(jpOptions.exec_path + "\\" + jpOptions.ini_file).ToArray();
            int ini_lineCount = File.ReadAllLines(jpOptions.exec_path + "\\" + jpOptions.ini_file).Length;

            if (ini_lineCount > 0)
            {
                changePass_chb.Visibility = Visibility.Visible;
                if (ini_lines[0].IndexOf("data_dir: ") == 0)
                {
                    jpOptions.data_dir = ini_lines[0].Substring(10, ini_lines[0].Length - 10);
                    data_dir_tb.Text = jpOptions.data_dir;

                    if (ini_lines[1].IndexOf("master_pass_hash: ") == 0)
                    {
                        error_lb.Visibility = Visibility.Hidden;
                        jpOptions.master_pass_hash = ini_lines[1].Substring(18, ini_lines[1].Length - 18);

                        if (ini_lines[2].IndexOf("master_pass_salt: ") == 0)
                        {
                            data_dir_tb.Text = jpOptions.data_dir;
                            data_dir_tb.IsReadOnly = true;
                            mpRepeat_pb.Visibility = Visibility.Hidden;
                            mpRepeat_lb.Visibility = Visibility.Hidden;

                            selectDir_bt.Visibility = Visibility.Hidden;
                            jpOptions.master_pass_salt = ini_lines[2].Substring(18, ini_lines[2].Length - 18);

                            if (ini_lines.Length > 3)
                            {
                                if (ini_lines[3].IndexOf("user_lang: ") == 0)
                                {
                                    string tmp_lang = ini_lines[3].Substring(11, ini_lines[3].Length - 11);
                                    if (tmp_lang == "1")
                                    {
                                        jPass.lang = 1;
                                    }
                                    else
                                    {
                                        jPass.lang = 0;
                                    }
                                }
                                else
                                {
                                    jPass.lang = 0;
                                }
                            }
                            else
                            {
                                jPass.lang = 0;
                            }
                        }
                        else
                        {
                            error_lb.Visibility = Visibility.Visible;
                            error_lb.Content = "mw-wl-error-ini-mp_salt";
                        }
                    }
                    else
                    {
                        error_lb.Visibility = Visibility.Visible;
                        error_lb.Content = "mw-wl-error-ini-mp_hash";
                    }
                }
                else
                {
                    error_lb.Visibility = Visibility.Visible;
                    error_lb.Content = "mw-wl-error-ini-data_dir";
                }
            }
            else
            {
                data_dir_tb.Text = data_dir;
            }
            useMv_lm();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (jpOptions.master_pass_hash.Length > 1)
            {
                if (mpEnter_pb.Password.Length > 2)
                {
                    if (HashString(mpEnter_pb.Password + jpOptions.master_pass_salt) == jpOptions.master_pass_hash)
                    {
                        //change master pass mode
                        if (changePass_chb.IsChecked == true)
                        {
                            jPass.userPassword = mpEnter_pb.Password;
                            mpRepeat_pb.Visibility = Visibility.Visible;
                            mpRepeat_lb.Visibility = Visibility.Visible;
                            mpEnter_lb.Content = mainWindow_Lm.mpEnter_lb_change[jPass.lang];
                            mpRepeat_lb.Content = mainWindow_Lm.mpRepeat_lb_change[jPass.lang];
                            mpEnter_pb.Password = null;
                            header_lb.Content = mainWindow_Lm.header_lb_change[jPass.lang];
                            changePass_chb.IsEnabled = false;
                            jpOptions.master_pass_hash = "";
                            error_lb.Content = "";
                            error_lb.Visibility = Visibility.Hidden;
                            return;
                        }

                        checkAppDataDirectory();
                        checkAppDataFiles();
                        jpOptions.master_pass_salt = Guid.NewGuid().ToString();
                        jpOptions.master_pass_hash = HashString(mpEnter_pb.Password + jpOptions.master_pass_salt);
                        string[] jp_options = { "data_dir: " + jpOptions.data_dir,
                            "master_pass_hash: " + jpOptions.master_pass_hash,
                            "master_pass_salt: "+jpOptions.master_pass_salt,
                            "user_lang: "+jPass.lang.ToString(),
                        };
                        File.WriteAllLines(jpOptions.ini_file, jp_options);
                        error_lb.Visibility = Visibility.Hidden;
                        Window jpMain_win = new Window1();
                        jpMain_win.Show();
                        this.Close();
                        jPass.userPassword = mpEnter_pb.Password;
                    }
                    else
                    {
                        error_lb.Content = "wrong password";
                        error_lb.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    error_lb.Content = "wrong password";
                    error_lb.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (mpEnter_pb.Password.Length > 2)
                {
                    if (mpEnter_pb.Password == mpRepeat_pb.Password)
                    {
                        string usrMasterPass = mpEnter_pb.Password;

                        //change master pass mode
                        if (changePass_chb.IsChecked == true)
                        {
                            jPass.reEncryptData(usrMasterPass);
                        }

                        mpEnter_lb.Content = mainWindow_Lm.mpEnter_lb[jPass.lang];
                        header_lb.Content = mainWindow_Lm.header_lb[jPass.lang];
                        mpRepeat_pb.Visibility = Visibility.Hidden;
                        mpRepeat_lb.Visibility = Visibility.Hidden;
                        mpEnter_pb.Password = null;
                        mpRepeat_pb.Password = null;
                        changePass_chb.IsEnabled = true;
                        changePass_chb.IsChecked = false;

                        checkAppDataDirectory();
                        checkAppDataFiles();
                        jpOptions.data_dir = data_dir_tb.Text;
                        jpOptions.master_pass_salt = Guid.NewGuid().ToString();
                        jpOptions.master_pass_hash = HashString(usrMasterPass + jpOptions.master_pass_salt);
                        string[] jp_options = {
                                "data_dir: " + jpOptions.data_dir,
                                "master_pass_hash: " + jpOptions.master_pass_hash,
                                "master_pass_salt: "+jpOptions.master_pass_salt,
                                "user_lang: "+jPass.lang.ToString(),
                            };
                        File.WriteAllLines(jpOptions.ini_file, jp_options);
                        error_lb.Visibility = Visibility.Hidden;

                        data_dir_tb.IsReadOnly = true;
                        mpRepeat_lb.Visibility = Visibility.Hidden;
                        selectDir_bt.Visibility = Visibility.Hidden;
                        changePass_chb.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        error_lb.Visibility = Visibility.Visible;
                        error_lb.Content = "passwords does not concur";
                    }
                }
                else
                {
                    error_lb.Visibility = Visibility.Visible;
                    error_lb.Content = "enter three or more symbols";
                }
            }

        }

        bool checkAppDataDirectory()
        {

            bool cadd_return = true;

            if (!System.IO.Directory.Exists(data_dir_tb.Text))
            {
                System.IO.Directory.CreateDirectory(data_dir_tb.Text);
                cadd_return = false;
            }

            if (!System.IO.Directory.Exists(data_dir_tb.Text + "\\img"))
            {
                System.IO.Directory.CreateDirectory(data_dir_tb.Text + "\\img");
                cadd_return = false;
            }

            if (!System.IO.Directory.Exists(data_dir_tb.Text + "\\accounts"))
            {
                System.IO.Directory.CreateDirectory(data_dir_tb.Text + "\\accounts");
                cadd_return = false;
            }

            return cadd_return;
        }

        bool checkAppDataFiles()
        {
            bool cadf_return = true;

            if (!File.Exists(data_dir_tb.Text + "\\groups.jpass"))
            {
                using (var tw = new StreamWriter(data_dir_tb.Text + "\\groups.jpass", true))
                {
                }
                cadf_return = false;
            }

            if (!File.Exists(data_dir_tb.Text + "\\categories.jpass"))
            {
                using (var tw = new StreamWriter(data_dir_tb.Text + "\\categories.jpass", true))
                {
                }
                cadf_return = false;
            }

            if (!File.Exists(data_dir_tb.Text + "\\accounts.jpass"))
            {
                using (var tw = new StreamWriter(data_dir_tb.Text + "\\accounts.jpass", true))
                {
                }
                cadf_return = false;
            }

            if (!File.Exists(data_dir_tb.Text + "\\fields.jpass"))
            {
                using (var tw = new StreamWriter(data_dir_tb.Text + "\\fields.jpass", true))
                {
                }
                cadf_return = false;
            }

            return cadf_return;
        }

        static string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }

        private void lang_rb_Checked(object sender, RoutedEventArgs e)
        {
            jPass.lang = 1;
            useMv_lm();
        }

        private void lang_rb1_Checked(object sender, RoutedEventArgs e)
        {
            jPass.lang = 0;
            useMv_lm();
        }
    }


    public static class jpOptions
    {
        public static string exec_path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        public static string ini_file = "jPass.ini";
        public static string data_dir;
        public static string master_pass_hash = "";
        public static string master_pass;
        public static string master_pass_salt = "";
        public static jPassAccField passAccField = new jPassAccField()
        {
            field_id = "a27y82w9-de3b-48de-bca2-bf76a916beak",
            field_name = jPass.pass_word[jPass.lang],
            field_image = jPass.password_img,
            encrypt = true,
        };
        public static jPassAccField loginAccField = new jPassAccField()
        {
            field_id = "x9p2c517-92bi-4y8z-81d2-a1f8928i684b",
            field_name = jPass.login_word[jPass.lang],
            field_image = jPass.login_img,
        };


    }

    public static class jPass
    {
        public static jPassGroups jpGroups = new jPassGroups();
        public static jPassCategories jpCategories = new jPassCategories();
        public static jPassAccounts jpAccounts = new jPassAccounts();
        public static jPassFields jpFields = new jPassFields();
        public static BitmapImage default_img = new BitmapImage(new Uri(@"/jPass_new;component/jpData/default_img.png", UriKind.Relative));
        public static BitmapImage password_img = new BitmapImage(new Uri(@"/jPass_new;component/jpData/password_img.png", UriKind.Relative));
        public static BitmapImage login_img = new BitmapImage(new Uri(@"/jPass_new;component/jpData/login_img.png", UriKind.Relative));
        public static string userPassword = null;

        public static string[] pass_word = new string[2] { "password", "пароль" };
        public static string[] login_word = new string[2] { "login", "логин" };
        public static string[] encrypted_word = new string[2] { "encrypted", "зашифровано" };

        public static string[] recrypt_title = new string[2] { "Recrypt passwords info", "Инфо о перешифровке" };
        public static string[] recrypt_acc = new string[2] { "Found accounts: ", "Проверено учеток: " };
        public static string[] recrypt_fields = new string[2] { "Recrypt fields: ", "Перешифровано полей: " };

        public static int lang = 1;


        public static BitmapImage toBitmap(Byte[] value)
        {
            if (value != null && value is byte[])
            {
                byte[] ByteArray = value as byte[];
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(ByteArray);
                bmp.EndInit();
                return bmp;
            }
            return null;
        }

        public static void reEncryptData(string usrMasterPass)
        {
            //change master pass option
            jpCategories.data_dir = jpOptions.data_dir;
            jpCategories.fillJCList();
            jpGroups.data_dir = jpOptions.data_dir;
            jpGroups.fillJGList();
            jpFields.fillJFList();
            jpAccounts.fillJAList();

            int accCounter = 0;
            int fieldsCounter = 0;

            foreach (var account in jpAccounts.jAccountsList)
            {

                accCounter++; ;
                jPassAccFields checkAccFields = new jPassAccFields();
                checkAccFields.jPassAccount = account;
                checkAccFields.fillAccFileds();

                int accEncrF_count = 0;

                foreach (var accField in checkAccFields.jAccFieldsList.Where(x => x.encrypt == true))
                {
                    fieldsCounter++;
                    accEncrF_count++;
                }

                if (accEncrF_count > 0)
                {
                    string[] arrLine = new string[checkAccFields.jAccFieldsList.Count];

                    int lineCounter = 0;
                    foreach (var accField in checkAccFields.jAccFieldsList)
                    {
                        if (accField.encrypt == true)
                        {
                            accField.val = StringCipher.Encrypt(accField.decrypt_val, usrMasterPass);

                        }
                        arrLine[lineCounter] = "field_id: " + accField.field_id + ", value: " + accField.val + ", lastUpdate: " + accField.lastUpdate;
                        lineCounter++;
                    }

                    File.WriteAllLines(jpOptions.data_dir + "\\accounts\\" + account.id + ".jpass", arrLine);
                }
            }


            jpAccounts = new jPassAccounts();
            jpFields = new jPassFields();
            jpCategories = new jPassCategories();
            jpGroups = new jPassGroups();
            MessageBox.Show(jPass.recrypt_acc[jPass.lang] + accCounter.ToString() + "\n" 
                + jPass.recrypt_fields[jPass.lang] + fieldsCounter.ToString(), jPass.recrypt_title[jPass.lang]);
        }

        public static bool checkReservedWords(string checkWord)
        {
            if (checkWord.Length < 2) { 
                return false;
            }

            string[] reserverNames = new string[] { "password", "login", "логин", "пароль" };

            foreach (string rsName in reserverNames) {
                if (checkWord == rsName)
                {
                    return false;
                }
            }         

            return true;
        }

    }





    public class jPassAccount
    {
        public string id { get; set; }
        public int lineNum { get; set; }
        public string name { get; set; }
        public string group_id { get; set; }
        public string category_id { get; set; }
        public string group_name { get; set; }
        public string category_name { get; set; }
        public string comment { get; set; }
        public string lastUpdate { get; set; }
        public BitmapImage groupImage { get; set; }
        public BitmapImage categoryImage { get; set; }
    }

    public class jPassCategory
    {
        public string id { get; set; }
        public int lineNum { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public int usageCount { get; set; }
        public BitmapImage image { get; set; }
    }

    public class jPassField
    {
        public string id { get; set; }
        public string name { get; set; }
        public int lineNum { get; set; }
        public string img { get; set; }
        public bool encrypt { get; set; }
        public BitmapImage image { get; set; }

    }

    public class jPassAccField
    {
        public string field_id { get; set; }
        public string field_name { get; set; }
        public int lineNum { get; set; }
        public string field_img { get; set; }
        public bool encrypt { get; set; }
        public string decrypt_val { get; set; }
        public string val { get; set; }
        public string grid_val { get; set; }
        public BitmapImage field_image { get; set; }
        public string lastUpdate { get; set; }

    }

    public class jPassGroup
    {
        public string id { get; set; }
        public int lineNum { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public BitmapImage image { get; set; }

    }

    public class jPassAccFields
    {

        public jPassAccount jPassAccount;

        public List<jPassAccField> jAccFieldsList = new List<jPassAccField>();

        public void fillAccFileds()
        {
            jAccFieldsList = new List<jPassAccField>();
            if (File.Exists(jpOptions.data_dir + "\\accounts\\" + jPassAccount.id + ".jpass"))
            {
                int lineCounter = 0;
                foreach (string line in System.IO.File.ReadLines(jpOptions.data_dir + "\\accounts\\" + jPassAccount.id + ".jpass"))
                {
                    var jAccField = new jPassAccField();
                    string restStr = line;

                    if (restStr.IndexOf("field_id: ") == 0)
                    {
                        jAccField.field_id = restStr.Substring(10, 36);
                        restStr = restStr.Substring(48, restStr.Length - 48);
                    }

                    bool proceed = true;
                    if (jAccField.field_id == jpOptions.loginAccField.field_id)
                    {
                        jAccField = jpOptions.loginAccField;
                        proceed = false;
                        //wrong name using jpOptions.loginAccField.name???
                        jAccField.field_name = jPass.login_word[jPass.lang];
                    }

                    if (jAccField.field_id == jpOptions.passAccField.field_id)
                    {
                        jAccField = jpOptions.passAccField;
                        proceed = false;
                        //wrong name using jpOptions.passAccField.name???
                        jAccField.field_name = jPass.pass_word[jPass.lang];
                    }

                    if (proceed)
                    {
                        jPassField jPassField = jPass.jpFields.jFieldsList.Single(r => r.id == jAccField.field_id);
                        jAccField.field_image = jPassField.image;
                        jAccField.field_name = jPassField.name;
                        jAccField.encrypt = jPassField.encrypt;
                    }

                    jAccField.lineNum = lineCounter;

                    int lastUpdate_pos = restStr.IndexOf(", lastUpdate: ");

                    if (restStr.IndexOf("value: ") == 0)
                    {
                        jAccField.val = restStr.Substring(7, lastUpdate_pos - 7);
                        jAccField.lastUpdate = restStr.Substring(lastUpdate_pos + 14, restStr.Length - lastUpdate_pos - 14);
                    }

                    if (jAccField.encrypt)
                    {
                        jAccField.grid_val = jPass.encrypted_word[jPass.lang];
                        jAccField.decrypt_val = StringCipher.Decrypt(jAccField.val, jPass.userPassword);
                    }
                    else
                    {
                        jAccField.grid_val = jAccField.val;
                    }

                    jAccFieldsList.Add(jAccField);

                    lineCounter++;
                }
            }
        }
    }



    public class jPassFields
    {
        public List<jPassField> jFieldsList = new List<jPassField>();
        public void fillJFList()
        {
            int lineCounter = 0;

            foreach (string line in System.IO.File.ReadLines(jpOptions.data_dir + "\\fields.jpass"))
            {
                var jField = new jPassField();

                jField.lineNum = lineCounter;

                string restStr = line;

                if (restStr.IndexOf("id: ") == 0)
                {
                    jField.id = restStr.Substring(4, 36);
                    restStr = restStr.Substring(42, restStr.Length - 42);
                }
                else
                {
                    jField.id = "no-id";
                }

                jField.name = restStr;

                int encrypt_pos = restStr.IndexOf(", encrypt: ");

                if (restStr.IndexOf("name: ") == 0)
                {
                    jField.name = restStr.Substring(6, encrypt_pos - 6);
                    restStr = restStr.Substring(encrypt_pos + 2, restStr.Length - encrypt_pos - 2);
                }
                else
                {
                    jField.name = "no-name: " + restStr;
                }

                string strEncrypt = "n";
                if (restStr.IndexOf("encrypt: ") == 0)
                {
                    strEncrypt = restStr.Substring(9, 1);
                    restStr = restStr.Substring(12, restStr.Length - 12);
                }

                if (strEncrypt == "y")
                {
                    jField.encrypt = true;
                }
                else
                {
                    jField.encrypt = false;
                }

                if (restStr.IndexOf("img: ") == 0)
                {
                    jField.img = restStr.Substring(5, restStr.Length - 5);
                    if (jField.img.Length > 3)
                    {
                        jField.image = jPass.toBitmap(File.ReadAllBytes(jpOptions.data_dir + "\\img\\" + jField.img));
                    }
                    else
                    {
                        jField.image = jPass.default_img;
                        jField.img = null;
                    }
                }

                jFieldsList.Add(jField);

                lineCounter++;
            }
        }
    }


    public class jPassAccounts
    {

        public List<jPassAccount> jAccountsList = new List<jPassAccount>();
        public void fillJAList()
        {
            int lineCounter = 0;

            foreach (string line in System.IO.File.ReadLines(jpOptions.data_dir + "\\accounts.jpass"))
            {
                var jAccount = new jPassAccount();

                jAccount.lineNum = lineCounter;

                string restStr = line;

                if (restStr.IndexOf("id: ") == 0)
                {
                    jAccount.id = restStr.Substring(4, 36);
                    restStr = restStr.Substring(42, restStr.Length - 42);
                }
                else
                {
                    jAccount.id = "no-id";
                }
                int groupId_pos = restStr.IndexOf(", group_id: ");
                jAccount.name = restStr.Substring(6, groupId_pos - 6);
                restStr = restStr.Substring(groupId_pos + 2, restStr.Length - groupId_pos - 2);
                jAccount.group_id = restStr;

                int categId_pos = restStr.IndexOf(", category_id: ");

                jAccount.group_id = restStr.Substring(10, categId_pos - 10);
                restStr = restStr.Substring(categId_pos + 2, restStr.Length - categId_pos - 2);

                int comment_pos = restStr.IndexOf(", comment: ");

                jAccount.category_id = restStr.Substring(13, comment_pos - 13);
                int lastUpd_pos = restStr.IndexOf(", lastUpdate: ");

                jAccount.comment = restStr.Substring(comment_pos + 11, lastUpd_pos - comment_pos - 11);


                jAccount.lastUpdate = restStr.Substring(lastUpd_pos + 14, restStr.Length - lastUpd_pos - 14);

                if (jAccount.group_id.Length > 3)
                {
                    var findGroup = jPass.jpGroups.jGroupsList.Single(r => r.id == jAccount.group_id);

                    jAccount.groupImage = findGroup.image;
                    jAccount.group_name = findGroup.name;
                }
                else
                {
                    jAccount.groupImage = jPass.default_img;
                }

                if (jAccount.category_id.Length > 3)
                {
                    var findCategory = jPass.jpCategories.jCategoriesList.Single(r => r.id == jAccount.category_id);

                    jAccount.categoryImage = findCategory.image;
                    jAccount.category_name = findCategory.name;
                }
                else
                {
                    jAccount.categoryImage = jPass.default_img;
                }
                jAccountsList.Add(jAccount);

                lineCounter++;
            }

        }
    }

    public class jPassCategories
    {

        public List<jPassCategory> jCategoriesList = new List<jPassCategory>();

        public string data_dir;

        public string category_file = "categories.jpass";

        public static BitmapImage toBitmap(Byte[] value)
        {
            if (value != null && value is byte[])
            {
                byte[] ByteArray = value as byte[];
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(ByteArray);
                bmp.EndInit();
                return bmp;
            }
            return null;
        }

        public void fillJCList()
        {

            int lineCounter = 0;

            foreach (string line in System.IO.File.ReadLines(data_dir + "\\categories.jpass"))
            {
                var jCategory = new jPassCategory();

                jCategory.lineNum = lineCounter;

                string restStr = line;

                if (restStr.IndexOf("id: ") == 0)
                {
                    jCategory.id = restStr.Substring(4, 36);
                    restStr = restStr.Substring(42, restStr.Length - 42);
                }
                else
                {
                    jCategory.id = "no-id";
                }

                int img_index = restStr.IndexOf(", img: ");

                if (restStr.IndexOf("name: ") == 0)
                {
                    jCategory.name = restStr.Substring(6, img_index - 6);
                }
                else
                {
                    jCategory.name = "no-name: " + restStr;
                }


                if (img_index > 7)
                {
                    restStr = restStr.Substring(img_index + 7, restStr.Length - img_index - 7);
                }

                if (restStr.Length > 3)
                {
                    jCategory.image = toBitmap(File.ReadAllBytes(data_dir + "\\img\\" + restStr));
                    jCategory.img = restStr;
                }
                else
                {
                    jCategory.image = jPass.default_img;
                    jCategory.img = null;
                }

                jCategoriesList.Add(jCategory);

                lineCounter++;
            }
        }
    }

    public class jPassGroups
    {

        public List<jPassGroup> jGroupsList = new List<jPassGroup>();

        public string data_dir;
        public string groups_file = "groups.jpass";
        public void fillJGList()
        {

            int lineCounter = 0;

            foreach (string line in System.IO.File.ReadLines(data_dir + "\\" + groups_file))
            {
                var jGroup = new jPassGroup();

                jGroup.lineNum = lineCounter;

                string restStr = line;

                if (restStr.IndexOf("id: ") == 0)
                {
                    jGroup.id = restStr.Substring(4, 36);
                    restStr = restStr.Substring(42, restStr.Length - 42);
                }
                else
                {
                    jGroup.id = "no-id";
                }

                int img_index = restStr.IndexOf(", img: ");

                if (restStr.IndexOf("name: ") == 0)
                {
                    jGroup.name = restStr.Substring(6, img_index - 6);
                }
                else
                {
                    jGroup.name = "no-name: " + restStr;
                }


                if (img_index > 7)
                {
                    restStr = restStr.Substring(img_index + 7, restStr.Length - img_index - 7);
                }

                if (restStr.Length > 3)
                {
                    jGroup.img = restStr;
                    jGroup.image = jPass.toBitmap(File.ReadAllBytes(data_dir + "\\img\\" + restStr));
                }
                else
                {
                    jGroup.image = jPass.default_img;
                    jGroup.img = null;
                }

                jGroupsList.Add(jGroup);

                lineCounter++;
            }
        }
    }

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            //var hashalg = new HashAlgorithmName("SHA256");
            //using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations, hashalg))
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();
            //var hashalg = new HashAlgorithmName("SHA256");
            //using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations, hashalg))
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }


}
