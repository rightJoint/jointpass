using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace jPass_new
{
    /// <summary>
    /// Логика взаимодействия для Window6.xaml
    /// </summary>
    public partial class Window6 : Window
    {
        public jPassAccFields jPassAccFields { get; set; }
        public jPassAccField jPassAccField { get; set; }

        public Window6()
        {
            InitializeComponent();
        }

        class Window6_lm
        {
            public string[] winTitle = new string[2] { "AccFields", "Поля учетной записи" };

            public string[] encrypt_chb = new string[2] { "ecnrypt", "Шифруется" };

            public string[] titleFiled_lb_add = new string[2] { "Add Field", "Добавить поле" };
            public string[] titleFiled_lb_edit = new string[2] { "Edit Value", "Редактировать" };


            public string[] fieldVal_tb_default = new string[2] { "enter-field-value", "значение-поля" };            

            public string[] accF_dtg_fImg = new string[2] { "fImg", "полеИз" };
            public string[] accF_dtg_fName = new string[2] { "fieldName", "Название поля" };
            public string[] accF_dtg_gridVal = new string[2] { "value", "Значение" };
            public string[] accF_dtg_lastUpd = new string[2] { "lastUpdate", "Обновлено" };

            public string[] update_btn_save = new string[2] { "Save", "Сохранить" };
            public string[] update_btn_update = new string[2] { "Update", "Обновить" };
            public string[] del_btn = new string[2] { "delete", "Удалить" };
            public string[] generate_btn = new string[2] { "generate", "создать" };

            public string[] info_lb_noErr = new string[2] { "no-err", "нет ошибок" };



            public string[] fieldVal_lbl = new string[2] { "fieldValue:", "Значение:" };

            public string[] accGrpup_label_ng = new string[2] { "no-group", "без группы" };
            public string[] accCateg_label_nc = new string[2] { "no-category", "без категории" };

            public string[] alert_del_conf = new string[2] { "Are you sure?", "Вы уверены?" };
            public string[] alert_del_title = new string[2] { "Delete Confirmation", "Подтверждение удаления" };
        }

        Window6_lm window6_lm = new Window6_lm();
        void useWin6_lm()
        {
            Title = window6_lm.winTitle[jPass.lang];

            accF_dtg.Columns[0].Header = window6_lm.accF_dtg_fImg[jPass.lang];
            accF_dtg.Columns[1].Header = window6_lm.accF_dtg_fName[jPass.lang];
            accF_dtg.Columns[2].Header = window6_lm.accF_dtg_gridVal[jPass.lang];
            accF_dtg.Columns[3].Header = window6_lm.accF_dtg_lastUpd[jPass.lang];

            encrypt_chb.Content = window6_lm.encrypt_chb[jPass.lang];

            fieldVal_lbl.Content = window6_lm.fieldVal_lbl[jPass.lang];

            del_btn.Content = window6_lm.del_btn[jPass.lang];
            generate_btn.Content = window6_lm.generate_btn[jPass.lang] ;

            info_lb.Content = window6_lm.info_lb_noErr[jPass.lang];
        }

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {

            string time = DateTime.Now.ToString("hh:mm:ss");
            string date = DateTime.Now.ToString("dd/MM/yy");

            if (titleFiled_lb.Content.ToString() == window6_lm.titleFiled_lb_add[jPass.lang])
            {
                bool proceed = true;
                if (fieldF_cbb.Text == jPass.login_word[jPass.lang])
                {
                    jPassAccField = jpOptions.loginAccField;
                    proceed = false;

                }

                if (fieldF_cbb.Text == jPass.pass_word[jPass.lang])
                {
                    jPassAccField = jpOptions.passAccField;
                  
                    proceed = false;
                }

                if (proceed) {
                    jPassField jPassField = jPass.jpFields.jFieldsList.Single(r => r.name == fieldF_cbb.Text);

                    jPassAccField.field_id = jPassField.id;
                    jPassAccField.field_name = jPassField.name;
                    jPassAccField.encrypt = jPassField.encrypt;
                    jPassAccField.field_image = jPassField.image;
                }

                if (jPassAccField.encrypt) {
                    jPassAccField.val = StringCipher.Encrypt(fieldVal_tb.Text, jPass.userPassword);
                    jPassAccField.grid_val = jPass.encrypted_word[jPass.lang];
                    jPassAccField.decrypt_val = fieldVal_tb.Text;
                }
                else {
                    jPassAccField.val = fieldVal_tb.Text;
                    jPassAccField.decrypt_val = fieldVal_tb.Text;
                    jPassAccField.grid_val = jPassAccField.val;
                }

                jPassAccField.lastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); ;

                if (!File.Exists(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass"))
                {
                    using (var tw = new StreamWriter(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass", true))
                    {

                    }
                }

                int lineCount = File.ReadLines(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass").Count();

                jPassAccField.lineNum = lineCount;
                jPassAccField.lastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                File.AppendAllText(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass",
                    "field_id: " + jPassAccField.field_id + ", value: " + jPassAccField.val + ", lastUpdate: " + jPassAccField.lastUpdate + Environment.NewLine);

                jPassAccFields.jAccFieldsList.Add(jPassAccField);

                accF_dtg.ItemsSource = null;
                accF_dtg.ItemsSource = jPassAccFields.jAccFieldsList;

                turnUpdate();               
            }
            else
            {
                
                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass");

                if (jPassAccField.encrypt)
                {
                    jPassAccField.val = StringCipher.Encrypt(fieldVal_tb.Text, jPass.userPassword);
                    jPassAccField.grid_val = jPass.encrypted_word[jPass.lang];
                    jPassAccField.decrypt_val = fieldVal_tb.Text;

                }
                else
                {
                    jPassAccField.val = fieldVal_tb.Text;
                    jPassAccField.decrypt_val = fieldVal_tb.Text;
                    jPassAccField.grid_val = fieldVal_tb.Text;
                }

                jPassAccField.lastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                arrLine[jPassAccField.lineNum] = "field_id: " + jPassAccField.field_id + ", value: " + jPassAccField.val + ", lastUpdate: " + jPassAccField.lastUpdate;

                File.WriteAllLines(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass", arrLine);

                jPassAccFields.jAccFieldsList[jPassAccField.lineNum] = jPassAccField;

                accF_dtg.ItemsSource = null;
                accF_dtg.ItemsSource = jPassAccFields.jAccFieldsList;

            }

            //last update password in accounts
            if (jPassAccField.field_id == jpOptions.passAccField.field_id)
            {
                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\accounts.jpass");

                jPassAccFields.jPassAccount.lastUpdate = jPassAccField.lastUpdate;

                arrLine[jPassAccFields.jPassAccount.lineNum] = "id: " + jPassAccFields.jPassAccount.id + ", name: " + jPassAccFields.jPassAccount.name +
   ", group_id: " + jPassAccFields.jPassAccount.group_id + ", category_id: " + jPassAccFields.jPassAccount.category_id +
   ", comment: " + jPassAccFields.jPassAccount.comment + ", lastUpdate: " + jPassAccFields.jPassAccount.lastUpdate;

                jPass.jpAccounts.jAccountsList[jPassAccFields.jPassAccount.lineNum].lastUpdate = jPassAccFields.jPassAccount.lastUpdate;

                File.WriteAllLines(jpOptions.data_dir + "\\accounts.jpass", arrLine);
            }

            info_lb.Content = window6_lm.info_lb_noErr[jPass.lang] + time;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            useWin6_lm();

            accName_lb.Content = jPassAccFields.jPassAccount.name;
            accCateg_image.Source = jPassAccFields.jPassAccount.categoryImage;
            accGroup_image.Source = jPassAccFields.jPassAccount.groupImage;
            if (jPassAccFields.jPassAccount.category_name == null)
            {
                accCateg_label.Content = window6_lm.accCateg_label_nc[jPass.lang];
            }
            else {
                accCateg_label.Content = jPassAccFields.jPassAccount.category_name;
                
            }

            if (jPassAccFields.jPassAccount.group_name == null)
            {
                accGrpup_label.Content = window6_lm.accGrpup_label_ng[jPass.lang];
                
            }
            else {
                accGrpup_label.Content = jPassAccFields.jPassAccount.group_name;
            }
            
            

            jPassAccField = new jPassAccField();           

            bool useLogin = false;

            foreach (var accField in jPassAccFields.jAccFieldsList) {
                if (accField.field_id == jpOptions.loginAccField.field_id) {
                    jPassAccField = accField;
                    useLogin = true;
                    break;
                }
            }

            fieldF_cbb.Items.Add(jPass.login_word[jPass.lang]);
            fieldF_cbb.Items.Add(jPass.pass_word[jPass.lang]);

            foreach (var field in jPass.jpFields.jFieldsList.OrderBy(x => x.name)) {
                fieldF_cbb.Items.Add(field.name);
            }

            if (useLogin)
            {
                turnUpdate();
            }
            else {
                jPassAccField = jpOptions.loginAccField;
                turnNew();
            }

            accF_dtg.ItemsSource = jPassAccFields.jAccFieldsList;
        }

        public void turnNew()
        {
            titleFiled_lb.Content = window6_lm.titleFiled_lb_add[jPass.lang];
            del_btn.Visibility = Visibility.Hidden;
            update_btn.Content = window6_lm.update_btn_save[jPass.lang];
            fieldVal_tb.Text = window6_lm.fieldVal_tb_default[jPass.lang];
            if (jPassAccField.encrypt)
            {
                encrypt_chb.IsChecked = true;
            }
            else
            {
                encrypt_chb.IsChecked = false;
            }
            fieldF_img.Source = jPassAccField.field_image;
            fieldId_lb.Content = "id: " + jPassAccField.field_id;
            fieldF_cbb.Text = jPassAccField.field_name;
            if (jPassAccField.field_id == jpOptions.passAccField.field_id)
            {
                generate_btn.Visibility = Visibility.Visible;
            }
            else
            {
                generate_btn.Visibility = Visibility.Hidden;
            }
        }

        public void turnUpdate()
        {
            titleFiled_lb.Content = window6_lm.titleFiled_lb_edit[jPass.lang];
            del_btn.Visibility = Visibility.Visible;
            update_btn.Content = window6_lm.update_btn_update[jPass.lang];
            if (jPassAccField.encrypt)
            {
                encrypt_chb.IsChecked = true;
                fieldVal_tb.Text = jPassAccField.decrypt_val;
            }
            else
            {
                encrypt_chb.IsChecked = false;
                fieldVal_tb.Text = jPassAccField.val;
            }
            fieldF_img.Source = jPassAccField.field_image;
            fieldId_lb.Content = "id: " + jPassAccField.field_id;
            fieldF_cbb.Text = jPassAccField.field_name;
            if (jPassAccField.field_id == jpOptions.passAccField.field_id)
            {
                generate_btn.Visibility = Visibility.Visible;
            }
            else
            {
                generate_btn.Visibility = Visibility.Hidden;
            }
        }

        private void fieldF_cbb_DropDownClosed(object sender, EventArgs e)
        {
            jPassAccField = new jPassAccField();
                
            bool new_flag = true;

            foreach (var item in jPassAccFields.jAccFieldsList)
            {
                if (item.field_name == fieldF_cbb.Text) {
                    new_flag = false;
                    break;
                }
            }

            if (new_flag)
            {
                bool foundF = false;
                if (fieldF_cbb.Text == jPass.login_word[jPass.lang])
                {
                    jPassAccField = jpOptions.loginAccField;
                    foundF = true;
                }

                if (fieldF_cbb.Text == jPass.pass_word[jPass.lang])
                {
                    jPassAccField = jpOptions.passAccField;
                    foundF = true;
                }

                if (!foundF)
                {
                    jPassField jPassField = jPass.jpFields.jFieldsList.Single(r => r.name == fieldF_cbb.Text);

                    jPassAccField.encrypt = jPassField.encrypt;
                    jPassAccField.field_image = jPassField.image;
                    jPassAccField.field_name = jPassField.name;
                    jPassAccField.field_id = jPassField.id;
                    jPassAccField.grid_val = null;
                    jPassAccField.decrypt_val = null;
                    jPassAccField.val = null;
                }

                turnNew();
            }
            else {
                jPassAccField = jPassAccFields.jAccFieldsList.Single(r => r.field_name == fieldF_cbb.Text);

                turnUpdate();
            }
        }

        private void accF_dtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                jPassAccField = jPassAccFields.jAccFieldsList.Single(r => r.field_id == ((dynamic)accF_dtg.SelectedItem).field_id);

                turnUpdate() ;
            }
            catch (Exception ex)
            {
                info_lb.Content = ex.Message;
            }
        }

        private void del_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(window6_lm.alert_del_conf[jPass.lang], window6_lm.alert_del_title[jPass.lang], System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass");


                var itemToRemove = jPassAccFields.jAccFieldsList.Single(r => r.field_id == jPassAccField.field_id);

                jPassAccFields.jAccFieldsList.Remove(itemToRemove);

                arrLine = arrLine.Where(w => w != arrLine[jPassAccField.lineNum]).ToArray();


                File.WriteAllLines(jpOptions.data_dir + "\\accounts\\" + jPassAccFields.jPassAccount.id + ".jpass", arrLine);

                int sListCounter = 0;

                bool is_pass_del_field = false;

                foreach (var jField in jPassAccFields.jAccFieldsList)
                {
                    jPassAccFields.jAccFieldsList[sListCounter].lineNum = sListCounter;
                    sListCounter++;
                }

                //last update password set null in accounts.jpass
                if (itemToRemove.field_id == jpOptions.passAccField.field_id) 
                {
                        string[] accountsLines = File.ReadAllLines(jpOptions.data_dir + "\\accounts.jpass");

                        jPassAccFields.jPassAccount.lastUpdate = "";

                    accountsLines[jPassAccFields.jPassAccount.lineNum] = "id: " + jPassAccFields.jPassAccount.id + ", name: " + jPassAccFields.jPassAccount.name +
           ", group_id: " + jPassAccFields.jPassAccount.group_id + ", category_id: " + jPassAccFields.jPassAccount.category_id +
           ", comment: " + jPassAccFields.jPassAccount.comment + ", lastUpdate: " ;

                        jPass.jpAccounts.jAccountsList[jPassAccFields.jPassAccount.lineNum].lastUpdate = jPassAccFields.jPassAccount.lastUpdate;

                        File.WriteAllLines(jpOptions.data_dir + "\\accounts.jpass", accountsLines);

                }

                accF_dtg.ItemsSource = null;
                accF_dtg.ItemsSource = jPassAccFields.jAccFieldsList;

                jPassAccField.val = null;
                jPassAccField.grid_val = null;
                jPassAccField.decrypt_val = null;
                jPassAccField.lastUpdate = null;

                update_btn.Content = window6_lm.update_btn_save[jPass.lang];
                fieldVal_tb.Text = null;

                turnNew();

            }
        }

        private void generate_btn_Click(object sender, RoutedEventArgs e)
        {
            string password = Membership.GeneratePassword(12, 1);
            fieldVal_tb.Text = password;
        }
    }    
}
