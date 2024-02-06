using Microsoft.Win32;
using System;
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
using System.Windows.Shapes;
using System.IO;
using System.Security.Principal;

namespace jPass_new
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {

        public jPassField jPassField { get; set; }
        public Window4()
        {
            InitializeComponent();
        }

        class Window4_lm
        {
            public string[] titleFiled_lb_new = new string[2] { "New Field", "Новое поле" };
            public string[] titleFiled_lb_edit = new string[2] { "Edit Field", "Правка поля" };
            public string[] winTitle = new string[2] { "Fields", "Поля" };

            public string[] update_btn_save = new string[2] { "Save", "Сохранить" };
            public string[] update_btn_update = new string[2] { "Update", "Обновить" };
            public string[] del_btn = new string[2] { "delete", "Удалить" };
            public string[] new_btn = new string[2] { "new", "Создать" };
            public string[] img_btn = new string[2] { "Img", "Изобр." };

            public string[] fieldName_lbl = new string[2] { "fieldName: ", "Название поля: " };
            public string[] encrypt_chb = new string[2] { "encrypt", "Зашифровать" };

            public string[] fieldName_default = new string[2] { "new-field-name", "Новое поле - имя" };

            public string[] info_noErr = new string[2] { "no err", "нет ошибок" };
            public string[] updateErr_not_changed = new string[2] { "name not changed", "наименование не менялось" };
            public string[] updateErr_double = new string[2] { "Double name error", "Повтор имени - ошибка" };
            public string[] updateErr_reserved = new string[2] { "Name reserved ", "Имя зарезервировано" };


            public string[] useFilter_lbl_on = new string[2] { "On", "Вкл" };
            public string[] useFilter_lbl_off = new string[2] { "Off", "Выкл" };
            public string[] srpRefresh_btn = new string[2] { "Refresh", "Обновить" };
            public string[] srpFound_lbl = new string[2] { "found: ", "Найдено: " };
            public string[] srfFilter_lbl = new string[2] { "filter: ", "Фильтр: " };

            public string[] dtf_fImg = new string[2] { "fImg", "полеИз" };
            public string[] dtf_fName = new string[2] { "fieldName", "Название поля" };
            public string[] dtf_encrypt = new string[2] { "encrypt", "Шифруется" };

            public string[] dialogTitle = new string[2] { "Pick file", "Выберите файл" };

            public string[] alert_del_conf = new string[2] { " Accounts contain this field,\nremove field from all Accounts?", 
                " учеток содержат это поле\nУдалить поле из всех учеток?" };
            public string[] alert_del_title = new string[2] { "Delete Confirmation", "Подтверждение удаления" };

            public string[] alert_encrypt_conf = new string[2] { " Accounts contain this field,\nEncrypt field in all Accounts?", 
                " учеток содержат это поле\nЗашифровать поле у всех учеток?" };
            public string[] alert_encrypt_title = new string[2] { "Encrypt Confirmation", "Подтверждение шифрования" };
            public string[] alert_decrypt_conf = new string[2] { " Accounts contain this field,\nDecrypt field in all Accounts?",
                " учеток содержат это поле\nЗашифровать поле у всех учеток?" };
            public string[] alert_decrypt_title = new string[2] { "Decrypt Confirmation", "Подтверждение расшифровки" };
        }

        Window4_lm window4_lm = new Window4_lm();
        void useWin4_lm()
        {
            titleFiled_lb.Content = window4_lm.titleFiled_lb_new[jPass.lang];
            Title = window4_lm.winTitle[jPass.lang];
            fieldName_tb.Text = window4_lm.fieldName_default[jPass.lang];

            del_btn.Content = window4_lm.del_btn[jPass.lang];
            new_btn.Content = window4_lm.new_btn[jPass.lang];
            img_btn.Content = window4_lm.img_btn[jPass.lang];

            srpFound_lbl.Content = window4_lm.srpFound_lbl[jPass.lang];
            srfFilter_lbl.Content = window4_lm.srfFilter_lbl[jPass.lang];
            srpRefresh_btn.Content = window4_lm.srpRefresh_btn[jPass.lang];

            update_btn.Content = window4_lm.update_btn_save[jPass.lang];

            fieldName_lbl.Content = window4_lm.fieldName_lbl[jPass.lang];
            encrypt_chb.Content = window4_lm.encrypt_chb[jPass.lang];


            fields_dtg.Columns[0].Header = window4_lm.dtf_fImg[jPass.lang];
            fields_dtg.Columns[1].Header = window4_lm.dtf_fName[jPass.lang];
            fields_dtg.Columns[2].Header = window4_lm.dtf_encrypt[jPass.lang];

            info_lb.Content = window4_lm.info_noErr[jPass.lang];
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            useWin4_lm();
            jPassField = new jPassField();
            img_btn.IsEnabled = false;
            del_btn.Visibility = Visibility.Hidden;
            filterFieldsList();
            new_btn.Visibility = Visibility.Hidden;
            field_img.Source = jPass.default_img;
            filterFieldsList();
        }

        private void fields_dtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                jPassField = jPass.jpFields.jFieldsList.Single(r => r.id == ((dynamic)fields_dtg.SelectedItem).id);
                field_img.Source = jPassField.image;
                fieldId_lb.Content = "id: " + jPassField.id;
                fieldName_tb.Text = jPassField.name;
                img_btn.IsEnabled = true;
                del_btn.Visibility = Visibility.Visible;
                titleFiled_lb.Content = window4_lm.titleFiled_lb_edit[jPass.lang];
                new_btn.Visibility = Visibility.Visible;
                update_btn.Content = window4_lm.update_btn_update[jPass.lang];
                encrypt_chb.IsChecked = jPassField.encrypt;
            }
            catch (Exception ex)
            {
                info_lb.Content = ex.Message;
            }
        }

        private void img_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = false,
                CheckPathExists = true,
                Multiselect = false,
                Title = window4_lm.dialogTitle[jPass.lang],
                Filter = "Files|*.jpg;*.jpeg;*.png;*.bmp;",
            };

            if (dialog.ShowDialog() == true)
            {
                string filename = dialog.FileName;
                string extension = System.IO.Path.GetExtension(dialog.FileName);

                if (jPassField.img != null)
                {

                    File.Delete(jpOptions.data_dir + "\\img\\" + jPassField.img);
                }

                jPassField.img = Guid.NewGuid().ToString() + extension;

                File.Copy(filename, jpOptions.data_dir + "\\img\\" + jPassField.img);

                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\fields.jpass");

                string encryptSign = null;
                if (jPassField.encrypt == true)
                {
                    encryptSign = "y";
                }
                else
                {
                    encryptSign = "n";
                }

                arrLine[jPassField.lineNum] = "id: " + jPassField.id + ", name: " + jPassField.name +
                    ", encrypt: " + encryptSign +
                    ", img: " + jPassField.img;

                File.WriteAllLines(jpOptions.data_dir + "\\fields.jpass", arrLine);

                jPassField.image = jPass.toBitmap(File.ReadAllBytes(jpOptions.data_dir + "\\img\\" + jPassField.img));

                field_img.Source = jPassField.image;

                string time = DateTime.Now.ToString("hh:mm:ss");
                string date = DateTime.Now.ToString("dd/MM/yy");

                jPass.jpFields.jFieldsList[jPassField.lineNum].image = jPassField.image;

                fields_dtg.ItemsSource = null;
                fields_dtg.ItemsSource = jPass.jpFields.jFieldsList;

                info_lb.Content = window4_lm.info_noErr[jPass.lang] + time;
            }
        }

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {
            string update_err = window4_lm.info_noErr[jPass.lang];

            if (jPassField.id == null)
            {
                if (jPass.jpFields.jFieldsList.Where(x => x.name == fieldName_tb.Text).Count() == 0)
                {
                    if (jPass.checkReservedWords(fieldName_tb.Text))
                    {
                        jPassField.id = Guid.NewGuid().ToString();

                        int lineCount = File.ReadLines(jpOptions.data_dir + "\\fields.jpass").Count();
                        jPassField.name = fieldName_tb.Text;
                        jPassField.lineNum = lineCount;

                        string encryptSign = null;
                        if (encrypt_chb.IsChecked == true)
                        {
                            jPassField.encrypt = true;
                            encryptSign = "y";
                        }
                        else
                        {
                            jPassField.encrypt = false;
                            encryptSign = "n";
                        }
                        jPassField.image = jPass.default_img;

                        File.AppendAllText(jpOptions.data_dir + "\\fields.jpass",
                            "id: " + jPassField.id + ", name: " + jPassField.name +
                            ", encrypt: " + encryptSign + ", img: " + jPassField.img + Environment.NewLine);

                        img_btn.IsEnabled = true;
                        titleFiled_lb.Content = window4_lm.titleFiled_lb_edit[jPass.lang];
                        update_btn.Content = window4_lm.update_btn_update[jPass.lang];
                        del_btn.IsEnabled = true;
                        fieldId_lb.Content = "id: " + jPassField.id;
                        jPass.jpFields.jFieldsList.Add(jPassField);
                        fields_dtg.ItemsSource = null;
                        fields_dtg.ItemsSource = jPass.jpFields.jFieldsList;
                        new_btn.Visibility = Visibility.Visible;
                        del_btn.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        update_err = window4_lm.updateErr_reserved[jPass.lang];
                    }

                }
                else
                {
                    update_err = window4_lm.updateErr_double[jPass.lang];
                }


            }
            else
            {
                bool decrypt_flag = false;
                bool encrypt_flag = false;

                bool encryptVal_flag = false;


                if (encrypt_chb.IsChecked == true)
                {
                    if (jPassField.encrypt == false)
                    {
                        encrypt_flag = true;
                    }
                    encryptVal_flag = true;
                }
                else
                {
                    if (jPassField.encrypt == true)
                    {
                        decrypt_flag = true;
                    }
                }


                if ((jPassField.name != fieldName_tb.Text) || ((decrypt_flag == true) || (encrypt_flag == true)))
                {

                    bool fName_err = false;

                    if (jPassField.name != fieldName_tb.Text)
                    {
                        if (jPass.jpFields.jFieldsList.Where(x => x.name == fieldName_tb.Text).Count() == 0)
                        {
                            if (!jPass.checkReservedWords(fieldName_tb.Text))
                            {
                                update_err = window4_lm.updateErr_reserved[jPass.lang];
                                fName_err = true;
                            }
                        }
                        else
                        {
                            fName_err = true;
                            update_err = window4_lm.updateErr_double[jPass.lang];
                        }
                    }

                    if (!fName_err)
                    {


                        string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\fields.jpass");

                        jPassField.name = fieldName_tb.Text;


                        if ((decrypt_flag == true) || (encrypt_flag == true))
                        {


                            List<jPassAccount> recryptAccList = new List<jPassAccount>();

                            foreach (var account in jPass.jpAccounts.jAccountsList)
                            {
                                jPassAccFields checkAccFields = new jPassAccFields();
                                checkAccFields.jPassAccount = account;

                                checkAccFields.fillAccFileds();

                                foreach (var accField in checkAccFields.jAccFieldsList.Where(x => x.field_id == jPassField.id))
                                {
                                    recryptAccList.Add(account);
                                }

                            }

                            string alert_cript_title;
                            string alert_cript_confirm;

                            if (decrypt_flag == true)
                            {
                                alert_cript_title = window4_lm.alert_decrypt_title[jPass.lang];
                                alert_cript_confirm = recryptAccList.Count().ToString() + window4_lm.alert_decrypt_conf[jPass.lang];
                            }
                            else
                            {
                                alert_cript_title = window4_lm.alert_encrypt_title[jPass.lang];
                                alert_cript_confirm = recryptAccList.Count().ToString() + window4_lm.alert_encrypt_conf[jPass.lang];
                            }
                            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(alert_cript_confirm, alert_cript_title, System.Windows.MessageBoxButton.YesNo);

                            if (messageBoxResult == MessageBoxResult.Yes)
                            {

                                foreach (var account in recryptAccList)
                                {
                                    jPassAccFields recryptAccFields = new jPassAccFields();
                                    recryptAccFields.jPassAccount = account;
                                    recryptAccFields.fillAccFileds();

                                    string[] recryptFiledLines = new string[recryptAccFields.jAccFieldsList.Count];
                                    int lineCounter = 0;

                                    foreach (var accField in recryptAccFields.jAccFieldsList)
                                    {

                                        if (accField.field_id == jPassField.id)
                                        {
                                            string valToSave = null;

                                            if (decrypt_flag)
                                            {
                                                valToSave = StringCipher.Decrypt(accField.val, jPass.userPassword);
                                            }

                                            if (encrypt_flag)
                                            {
                                                valToSave = StringCipher.Encrypt(accField.val, jPass.userPassword);


                                            }
                                            recryptFiledLines[lineCounter] = "field_id: " + accField.field_id + ", value: " + valToSave + ", lastUpdate: " + accField.lastUpdate;

                                        }
                                        else
                                        {
                                            recryptFiledLines[lineCounter] = "field_id: " + accField.field_id + ", value: " + accField.val + ", lastUpdate: " + accField.lastUpdate;
                                        }
                                        lineCounter++;
                                    }
                                    File.WriteAllLines(jpOptions.data_dir + "\\accounts\\" + account.id + ".jpass", recryptFiledLines);
                                }

                            }
                            else
                            {
                                encrypt_chb.IsChecked = jPassField.encrypt;
                                encryptVal_flag = jPassField.encrypt;
                            }
                        }

                        jPassField.encrypt = encryptVal_flag;

                        string encryptSign = null;

                        if (jPassField.encrypt == true)
                        {
                            encryptSign = "y";
                        }
                        else
                        {
                            encryptSign = "n";
                        }

                        arrLine[jPassField.lineNum] = "id: " + jPassField.id + ", name: " + jPassField.name +
                            ", encrypt: " + encryptSign + ", img: " + jPassField.img;

                        File.WriteAllLines(jpOptions.data_dir + "\\fields.jpass", arrLine);

                        jPass.jpFields.jFieldsList[jPassField.lineNum].name = jPassField.name;

                        fields_dtg.ItemsSource = null;
                        fields_dtg.ItemsSource = jPass.jpFields.jFieldsList;
                    }
                }

                else
                {
                    update_err = window4_lm.updateErr_not_changed[jPass.lang];
                }
            }
            string time = DateTime.Now.ToString("hh:mm:ss");
            string date = DateTime.Now.ToString("dd/MM/yy");

            info_lb.Content = update_err + time;
        }

        private void del_btn_Click(object sender, RoutedEventArgs e)
        {
            List<jPassAccount> delAccFieldsList = new List<jPassAccount>();

            foreach (var account in jPass.jpAccounts.jAccountsList)
            {
                jPassAccFields checkAccFields = new jPassAccFields();
                checkAccFields.jPassAccount = account;
                checkAccFields.fillAccFileds();

                foreach (var accField in checkAccFields.jAccFieldsList.Where(x => x.field_id == jPassField.id))
                {
                    delAccFieldsList.Add(account);
                }
            }


            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(delAccFieldsList.Count().ToString() +
                window4_lm.alert_del_conf[jPass.lang], window4_lm.alert_del_title[jPass.lang], System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var account in delAccFieldsList)
                {
                    jPassAccFields delAccFields = new jPassAccFields();
                    delAccFields.jPassAccount = account;
                    delAccFields.fillAccFileds();

                    string[] delFiledLines = new string[delAccFields.jAccFieldsList.Count - 1];
                    int lineCounter = 0;

                    foreach (var accField in delAccFields.jAccFieldsList)
                    {

                        if (accField.field_id != jPassField.id)
                        {
                            delFiledLines[lineCounter] = "field_id: " + accField.field_id + ", value: " + accField.val + ", lastUpdate: " + accField.lastUpdate;
                            lineCounter++;
                        }
                    }
                    File.WriteAllLines(jpOptions.data_dir + "\\accounts\\" + account.id + ".jpass", delFiledLines);
                }

                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\fields.jpass");


                var itemToRemove = jPass.jpFields.jFieldsList.Single(r => r.id == jPassField.id);

                jPass.jpFields.jFieldsList.Remove(itemToRemove);

                arrLine = arrLine.Where(w => w != arrLine[jPassField.lineNum]).ToArray();


                File.WriteAllLines(jpOptions.data_dir + "\\fields.jpass", arrLine);

                int sListCounter = 0;
                foreach (var jField in jPass.jpFields.jFieldsList)
                {
                    jPass.jpFields.jFieldsList[sListCounter].lineNum = sListCounter;
                    sListCounter++;
                }

                fields_dtg.ItemsSource = null;

                if (jPassField.img != null)
                {
                    File.Delete(jpOptions.data_dir + "\\img\\" + jPassField.img);
                }

                jPassField = null; ;
                img_btn.IsEnabled = false;
                del_btn.Visibility = Visibility.Hidden;
                fields_dtg.ItemsSource = jPass.jpFields.jFieldsList;
                new_btn.Visibility = Visibility.Hidden;
                field_img.Source = jPass.default_img;
                titleFiled_lb.Content = window4_lm.titleFiled_lb_new[jPass.lang];
                fieldName_tb.Text = window4_lm.fieldName_default[jPass.lang];
                update_btn.Content = window4_lm.update_btn_save[jPass.lang];

            }

        }

        private void new_btn_Click(object sender, RoutedEventArgs e)
        {
            del_btn.Visibility = Visibility.Hidden;
            fieldName_tb.Text = window4_lm.fieldName_default[jPass.lang];
            fieldId_lb.Content = "id: ";
            field_img.Source = jPass.default_img;
            jPassField = new jPassField();
            titleFiled_lb.Content = window4_lm.titleFiled_lb_new[jPass.lang];
            img_btn.IsEnabled = false;
            update_btn.Content = window4_lm.update_btn_save[jPass.lang];
            new_btn.Visibility = Visibility.Hidden;
        }

        private void fieldsFilterName_tb_KeyUp(object sender, KeyEventArgs e)
        {
            filterFieldsList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fieldsFilterName_tb.Text = "";
            filterFieldsList();
        }

        public void filterFieldsList()
        {
            fields_dtg.ItemsSource = null;

            bool use_filter = false;

            List<jPassField> filteredFields_list = new List<jPassField>();

            if (fieldsFilterName_tb.Text != "")
            {
                use_filter = true;

                filteredFields_list = jPass.jpFields.jFieldsList.Where(x => x.name.ToLower().Contains(fieldsFilterName_tb.Text.ToLower())).ToList();
            }
            else
            {
                filteredFields_list = jPass.jpFields.jFieldsList;
            }

            if (use_filter)
            {
                useFilter_lbl.Background = Brushes.Firebrick;
                useFilter_lbl.Foreground = Brushes.White;
                useFilter_lbl.Content = window4_lm.useFilter_lbl_on[jPass.lang];
            }
            else
            {
                useFilter_lbl.Background = Brushes.Silver;
                useFilter_lbl.Content = window4_lm.useFilter_lbl_off[jPass.lang];
                useFilter_lbl.Foreground = Brushes.Black;
            }

            fields_dtg.ItemsSource = filteredFields_list;
            foundCount_lbl.Content = filteredFields_list.Count().ToString();

        }
    }
}

