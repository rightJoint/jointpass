using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using System.IO;

namespace jPass_new
{
    /// <summary>
    /// Логика взаимодействия для Window5.xaml
    /// </summary>
    /// 


    public partial class Window5 : Window
    {
        public jPassAccount jPassAccount { get; set; }

        public Window5()
        {
            InitializeComponent();
        }


        class Window5_lm
        {
            public string[] titleFiled_lb_new = new string[2] { "New Account", "Новая учетка" };
            public string[] titleFiled_lb_edit = new string[2] { "Edit Account", "Правка учетки" };
            public string[] winTitle = new string[2] { "Accounts", "Учетные записи" };

            public string[] update_btn_save = new string[2] { "Save", "Сохранить" };
            public string[] update_btn_update = new string[2] { "Update", "Обновить" };
            public string[] del_btn = new string[2] { "delete", "Удалить" };
            public string[] new_btn = new string[2] { "new", "Создать" };
            public string[] img_btn = new string[2] { "Img", "Изобр." };

            public string[] accountName_default = new string[2] { "new-account-name", "Новая учетка - имя" };


            public string[] info_noErr = new string[2] { "no err", "нет ошибок" };
            public string[] updateErr_not_changed = new string[2] { "account not changed", "УЗ не менялась" };
            public string[] updateErr_double = new string[2] { "Double name error", "Повтор имени - ошибка" };
            public string[] updateErr_reserved = new string[2] { "Name reserved ", "Имя зарезервировано" };
            public string[] group_cbb_noGr = new string[2] { "no group", "без группы" };
            public string[] categ_cbb_noCat = new string[2] { "no category", "без категории" };

            public string[] dtAcc_gImg = new string[2] { "gImg", "гИз" };
            public string[] dtAcc_cImg = new string[2] { "cImg", "кИз" };
            public string[] dtAcc_name = new string[2] { "accountName", "Название учетки" };
            public string[] dtAcc_Comment = new string[2] { "comment", "коментарий" };
            public string[] dtAcc_gName = new string[2] { "group", "группа" };
            public string[] dtAcc_cName = new string[2] { "category", "категория" };
            public string[] dtAcc_lUpd = new string[2] { "lastUpdate", "обновлено" };

            public string[] accName_lbl = new string[2] { "accountName:", "Наименование учетки:" };
            public string[] accComment_lbl = new string[2] { "comment:", "коментарий:" };




            public string[] useFilter_lbl_on = new string[2] { "On", "Вкл" };
            public string[] useFilter_lbl_off = new string[2] { "Off", "Выкл" };
            public string[] srpRefresh_btn = new string[2] { "Refresh", "Обновить" };
            public string[] srpFound_lbl = new string[2] { "found: ", "Найдено: " };
            public string[] srfFilter_lbl = new string[2] { "filter: ", "Фильтр: " };

            public string[] dialog_alert_message = new string[2] { "Delete account with its fields?", "Удалить учетку с со всеми полями. Вы уверены?" };
            public string[] dialog_alert_header = new string[2] { "Delete Confirmation", "Подтверждение удаления" };

        }

        Window5_lm window5_lm = new Window5_lm();
        void useWin5_lm()
        {
            titleFiled_lb.Content = window5_lm.titleFiled_lb_new[jPass.lang];
            Title = window5_lm.winTitle[jPass.lang];
            accountName_tb.Text = window5_lm.accountName_default[jPass.lang];

            accName_lbl.Content = window5_lm.accName_lbl[jPass.lang];
            accComment_lbl.Content = window5_lm.accComment_lbl[jPass.lang];
            
            update_btn.Content = window5_lm.update_btn_save[jPass.lang];
            del_btn.Content = window5_lm.del_btn[jPass.lang];
            new_btn.Content = window5_lm.new_btn[jPass.lang];

            srpFound_lbl.Content = window5_lm.srpFound_lbl[jPass.lang];
            srfFilter_lbl.Content = window5_lm.srfFilter_lbl[jPass.lang];
            srpRefresh_btn.Content = window5_lm.srpRefresh_btn[jPass.lang];

            accounts_dtg.Columns[0].Header = window5_lm.dtAcc_gImg[jPass.lang];
            accounts_dtg.Columns[1].Header = window5_lm.dtAcc_cImg[jPass.lang];
            accounts_dtg.Columns[2].Header = window5_lm.dtAcc_name[jPass.lang];
            accounts_dtg.Columns[3].Header = window5_lm.dtAcc_Comment[jPass.lang];
            accounts_dtg.Columns[4].Header = window5_lm.dtAcc_gName[jPass.lang];
            accounts_dtg.Columns[5].Header = window5_lm.dtAcc_cName[jPass.lang];
            accounts_dtg.Columns[6].Header = window5_lm.dtAcc_lUpd[jPass.lang];

            info_lb.Content = window5_lm.info_noErr[jPass.lang];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            useWin5_lm();

            jPassAccount = new jPassAccount();

            filterAccList();
            new_btn.Visibility = Visibility.Hidden;
            del_btn.Visibility = Visibility.Hidden;

            group_cbb.Items.Add(window5_lm.group_cbb_noGr[jPass.lang]);
            foreach (var jGroup in jPass.jpGroups.jGroupsList.OrderBy(x => x.name))
            {
                group_cbb.Items.Add(jGroup.name);
            }
            group_cbb.Text = window5_lm.group_cbb_noGr[jPass.lang];

            categ_cbb.Items.Add(window5_lm.categ_cbb_noCat[jPass.lang]);
            foreach (var jCateg in jPass.jpCategories.jCategoriesList.OrderBy(x => x.name))
            {
                categ_cbb.Items.Add(jCateg.name);
            }
            categ_cbb.Text = window5_lm.categ_cbb_noCat[jPass.lang];
            categ_img.Source = jPass.default_img;
            group_img.Source = jPass.default_img;
        }

        private void accounts_dtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                jPassAccount = jPass.jpAccounts.jAccountsList.Single(r => r.id == ((dynamic)accounts_dtg.SelectedItem).id);
                group_img.Source = jPassAccount.groupImage;
                categ_img.Source = jPassAccount.categoryImage;
                fieldId_lb.Content = "id: " + jPassAccount.id;
                accountName_tb.Text = jPassAccount.name;
                comment_tb.Text = jPassAccount.comment;

                if (jPassAccount.group_id == "")
                {
                    group_cbb.Text = window5_lm.group_cbb_noGr[jPass.lang];
                }
                else
                {
                    group_cbb.Text = jPassAccount.group_name;
                }

                if (jPassAccount.category_id == "")
                {
                    categ_cbb.Text = window5_lm.categ_cbb_noCat[jPass.lang];
                }
                else
                {
                    categ_cbb.Text = jPassAccount.category_name;
                }

                del_btn.Visibility = Visibility.Visible;
                titleFiled_lb.Content = window5_lm.titleFiled_lb_edit[jPass.lang];
                new_btn.Visibility = Visibility.Visible;
                update_btn.Content = window5_lm.update_btn_update[jPass.lang];
                info_lb.Content = window5_lm.info_noErr[jPass.lang];
            }
            catch (Exception ex)
            {
                info_lb.Content = ex.Message;
            }
        }

        private void group_cbb_DropDownClosed(object sender, EventArgs e)
        {
            jPassGroup jPassGroup = new jPassGroup();

            if (group_cbb.Text == window5_lm.group_cbb_noGr[jPass.lang])
            {
                group_img.Source = jPass.default_img;
            }
            else
            {
                jPassGroup = jPass.jpGroups.jGroupsList.Single(r => r.name == group_cbb.Text);
                group_img.Source = jPassGroup.image;
                info_lb.Content = window5_lm.info_noErr[jPass.lang];
            }
        }

        private void categ_cbb_DropDownClosed(object sender, EventArgs e)
        {
            jPassCategory jPassCategory = new jPassCategory();

            if (categ_cbb.Text == window5_lm.categ_cbb_noCat[jPass.lang])
            {
                categ_img.Source = jPass.default_img;
            }
            else
            {
                jPassCategory = jPass.jpCategories.jCategoriesList.Single(r => r.name == categ_cbb.Text);
                categ_img.Source = jPassCategory.image;
                info_lb.Content = window5_lm.info_noErr[jPass.lang];
            }
        }

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {
            string update_err = window5_lm.info_noErr[jPass.lang];

            jPassCategory jPassCategory = new jPassCategory();
            jPassGroup jPassGroup = new jPassGroup();

            if (categ_cbb.Text != window5_lm.categ_cbb_noCat[jPass.lang])
            {
                jPassCategory = jPass.jpCategories.jCategoriesList.Single(r => r.name == categ_cbb.Text);
            }
            else
            {
                jPassCategory.image = jPass.default_img;
                jPassCategory.id = "";
            }

            if (group_cbb.Text != window5_lm.group_cbb_noGr[jPass.lang])
            {
                jPassGroup = jPass.jpGroups.jGroupsList.Single(r => r.name == group_cbb.Text);
            }
            else
            {
                jPassGroup.image = jPass.default_img;
                jPassGroup.id = "";

            }


            if (jPassAccount.id == null)
            {
                if (jPass.jpAccounts.jAccountsList.Where(x => x.name == accountName_tb.Text).Count() == 0)
                {
                    if (jPass.checkReservedWords(accountName_tb.Text))
                    {
                        jPassAccount.id = Guid.NewGuid().ToString();

                        int lineCount = File.ReadLines(jpOptions.data_dir + "\\accounts.jpass").Count();
                        jPassAccount.name = accountName_tb.Text;
                        jPassAccount.lineNum = lineCount;
                        jPassAccount.comment = comment_tb.Text;
                        jPassAccount.group_id = jPassGroup.id;
                        jPassAccount.category_id = jPassCategory.id;

                        jPassAccount.groupImage = jPassGroup.image;
                        jPassAccount.categoryImage = jPassCategory.image;
                        jPassAccount.group_name = jPassGroup.name;
                        jPassAccount.category_name = jPassCategory.name;

                        File.AppendAllText(jpOptions.data_dir + "\\accounts.jpass",
                            "id: " + jPassAccount.id + ", name: " + jPassAccount.name +
                            ", group_id: " + jPassAccount.group_id + ", category_id: " + jPassAccount.category_id +
                            ", comment: " + jPassAccount.comment + ", lastUpdate: " + jPassAccount.lastUpdate + Environment.NewLine);

                        titleFiled_lb.Content = window5_lm.titleFiled_lb_edit[jPass.lang];
                        update_btn.Content = window5_lm.update_btn_update[jPass.lang];
                        del_btn.IsEnabled = true;

                        fieldId_lb.Content = "id: " + jPassAccount.id;
                        jPass.jpAccounts.jAccountsList.Add(jPassAccount);
                        accounts_dtg.ItemsSource = null;
                        accounts_dtg.ItemsSource = jPass.jpAccounts.jAccountsList;
                        new_btn.Visibility = Visibility.Visible;
                        del_btn.Visibility = Visibility.Visible;
                    }
                    else {
                        update_err = window5_lm.updateErr_reserved[jPass.lang];
                    }
                        
                }
                else {
                    update_err = window5_lm.updateErr_double[jPass.lang];
                }
                    
            }
            else
            {
                if (isAccChanged())
                {
                    if (jPass.checkReservedWords(accountName_tb.Text))
                    {
                        string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\accounts.jpass");

                        jPassAccount.name = accountName_tb.Text;
                        jPassAccount.comment = comment_tb.Text;
                        jPassAccount.group_id = jPassGroup.id;
                        jPassAccount.group_name = jPassGroup.name;
                        jPassAccount.category_id = jPassCategory.id;
                        jPassAccount.groupImage = jPassGroup.image;
                        jPassAccount.categoryImage = jPassCategory.image;
                        jPassAccount.category_name = jPassCategory.name;

                        arrLine[jPassAccount.lineNum] = "id: " + jPassAccount.id + ", name: " + jPassAccount.name +
            ", group_id: " + jPassAccount.group_id + ", category_id: " + jPassAccount.category_id +
            ", comment: " + jPassAccount.comment + ", lastUpdate: " + jPassAccount.lastUpdate;

                        File.WriteAllLines(jpOptions.data_dir + "\\accounts.jpass", arrLine);

                        jPass.jpAccounts.jAccountsList[jPassAccount.lineNum] = jPassAccount;

                        accounts_dtg.ItemsSource = null;
                        accounts_dtg.ItemsSource = jPass.jpAccounts.jAccountsList;
                    }
                    else {
                        update_err = window5_lm.updateErr_reserved[jPass.lang];
                    }
                        
                }
                else { 
                    update_err = window5_lm.updateErr_not_changed[jPass.lang];
                
                }                
            }
            string time = DateTime.Now.ToString("hh:mm:ss");
            string date = DateTime.Now.ToString("dd/MM/yy");

            info_lb.Content = update_err + time;

        }

        private void del_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(window5_lm.dialog_alert_message[jPass.lang],
                window5_lm.dialog_alert_header[jPass.lang], System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                if (File.Exists(jpOptions.data_dir + "\\accounts\\" + jPassAccount.id + ".jpass"))
                {
                    File.Delete(jpOptions.data_dir + "\\accounts\\" + jPassAccount.id + ".jpass");
                }

                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\accounts.jpass");


                var itemToRemove = jPass.jpAccounts.jAccountsList.Single(r => r.id == jPassAccount.id);

                jPass.jpAccounts.jAccountsList.Remove(itemToRemove);

                arrLine = arrLine.Where(w => w != arrLine[jPassAccount.lineNum]).ToArray();


                File.WriteAllLines(jpOptions.data_dir + "\\accounts.jpass", arrLine);

                int sListCounter = 0;
                foreach (var jAccount in jPass.jpAccounts.jAccountsList)
                {
                    jPass.jpAccounts.jAccountsList[sListCounter].lineNum = sListCounter;
                    sListCounter++;
                }

                accounts_dtg.ItemsSource = null;
                accounts_dtg.ItemsSource = jPass.jpAccounts.jAccountsList;

                jPassAccount = null; ;
                del_btn.Visibility = Visibility.Hidden;
                new_btn.Visibility = Visibility.Hidden;
                titleFiled_lb.Content = window5_lm.titleFiled_lb_new[jPass.lang];
                accountName_tb.Text = window5_lm.accountName_default[jPass.lang];
                comment_tb.Text = null;
                update_btn.Content = window5_lm.update_btn_save[jPass.lang];


            }
        }

        private void accounts_dtg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Window6 accFields_win = new Window6();
            accFields_win.jPassAccFields = new jPassAccFields();
            accFields_win.jPassAccFields.jPassAccount = jPassAccount;
            accFields_win.jPassAccFields.fillAccFileds();
            accFields_win.ShowDialog();
        }

        private void new_btn_Click(object sender, RoutedEventArgs e)
        {
            del_btn.Visibility = Visibility.Hidden;
            accountName_tb.Text = window5_lm.accountName_default[jPass.lang];
            fieldId_lb.Content = "id: ";
            group_img.Source = jPass.default_img;
            categ_img.Source = jPass.default_img;

            group_cbb.Text = window5_lm.group_cbb_noGr[jPass.lang];
            categ_cbb.Text = window5_lm.categ_cbb_noCat[jPass.lang];

            jPassAccount = new jPassAccount();
            titleFiled_lb.Content = window5_lm.titleFiled_lb_new[jPass.lang];
            comment_tb.Text = null;

            update_btn.Content = window5_lm.update_btn_save[jPass.lang];
            new_btn.Visibility = Visibility.Hidden;
        }

        private void accFilterName_tb_KeyUp(object sender, KeyEventArgs e)
        {
            filterAccList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            accFilterName_tb.Text = "";
            filterAccList();
        }

        public void filterAccList()
        {
            accounts_dtg.ItemsSource = null;

            bool use_filter = false;

            List<jPassAccount> filteredAccounts_list = new List<jPassAccount>();

            if (accFilterName_tb.Text != "")
            {
                use_filter = true;

                filteredAccounts_list = jPass.jpAccounts.jAccountsList.Where(x => x.name.ToLower().Contains(accFilterName_tb.Text.ToLower())).ToList();
            }
            else
            {
                filteredAccounts_list = jPass.jpAccounts.jAccountsList;
            }

            if (use_filter)
            {
                useFilter_lbl.Background = Brushes.Firebrick;
                useFilter_lbl.Foreground = Brushes.White;
                useFilter_lbl.Content = window5_lm.useFilter_lbl_on[jPass.lang];
            }
            else
            {
                useFilter_lbl.Background = Brushes.Silver;
                useFilter_lbl.Content = window5_lm.useFilter_lbl_off[jPass.lang];
                useFilter_lbl.Foreground = Brushes.Black;
            }

            accounts_dtg.ItemsSource = filteredAccounts_list;
            foundCount_lbl.Content = filteredAccounts_list.Count().ToString();

        }

        public bool isAccChanged()
        {
            string compareCat = null;
            string compareGroup = null;

            if(categ_cbb.Text != window5_lm.categ_cbb_noCat[jPass.lang])
            {
                compareCat = categ_cbb.Text;
            }

            if(group_cbb.Text != window5_lm.group_cbb_noGr[jPass.lang])
            {
                compareGroup = group_cbb.Text;
            }


            if (jPassAccount.category_name != compareCat) {
                return true;
            }

            if (jPassAccount.group_name != compareGroup) {
                return true;
            }

            if (accountName_tb.Text != jPassAccount.name) {
                return true;
            }

            if (comment_tb.Text != jPassAccount.comment) {;
                return true;
            }

            return false;
        }

    }
}
