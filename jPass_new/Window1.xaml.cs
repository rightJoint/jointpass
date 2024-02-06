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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Cryptography;

namespace jPass_new
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// 

    public partial class Window1 : Window
    {

        public List<jPassAccount> refreshedAcc_list = new List<jPassAccount>();
        public List<jPassCategory> refreshedCat_list = new List<jPassCategory>();

        bool useFilterCats = false;
        bool useFilterGroups = false;
       
        public jPassAccFields curwAccFields { get; set; }

        class Window1_lm
        {
            public string[] winTitle = new string[2] { "jointPass", "ДжойнтПас" };


            public string[] groups_btn = new string[2] { "Groups", "Группы" };
            public string[] categs_btn = new string[2] { "Categories", "Категории" };
            public string[] fields_btn = new string[2] { "Fields", "Поля" };
            public string[] accounts_btn = new string[2] { "Accounts", "Учетки" };
            public string[] optCap_lb = new string[2] { "Options: ", "Опции: " };

            public string[] filterMain_dtg_cImg = new string[2] { "cImg", "кИз" };
            public string[] filterMain_dtg_cName = new string[2] { "Category", "Категория" };
            public string[] filterMain_dtg_q = new string[2] { "accCount", "к-во УЗ" };
            public string[] searchRes_dtg_gImg = new string[2] { "gImg", "гИз" };
            public string[] searchRes_dtg_cImg = new string[2] { "cImg", "кИз" };
            public string[] searchRes_dtg_accName = new string[2] { "accName", "Учетка" };
            public string[] searchRes_dtg_gName = new string[2] { "group", "Группа" };
            public string[] searchRes_dtg_cName = new string[2] { "Category", "Категория" };
            public string[] searchRes_dtg_lUpd = new string[2] { "lastUpdat", "Обновлен" };
            public string[] searchRes_dtg_cm = new string[2] { "Comment", "Коментарий" };
            public string[] srpFound_lbl = new string[2] { "found", "Найдено" };
            public string[] srfFilter_lbl = new string[2] { "filter: ", "Фильтр: " };
            public string[] cpPassword_btn = new string[2] { "cpPassword", "кпПароль" };
            public string[] cpLogin_btn = new string[2] { "cpLogin", "кпЛогин" };
            public string[] srpRefresh_btn = new string[2] { "Refresh", "Обновить" };
            public string[] fmRefresh_btn = new string[2] { "Refresh", "Обновить" };



            public string[] useFilter_lbl_on = new string[2] { "On", "Вкл" };
            public string[] useFilter_lbl_off = new string[2] { "Off", "Выкл" };

            public string[] group_no_group = new string[2] { "no-group", "без группы" };
            public string[] group_any_group = new string[2] { "any-group", "любая группа" };
            public string[] cat_no_cat = new string[2] { "no-category", "без категории" };


        }

        Window1_lm window1_lm = new Window1_lm();
        void useWin1_lm()
        {
            Title = window1_lm.winTitle[jPass.lang];
            groups_btn.Content = window1_lm.groups_btn[jPass.lang];
            categs_btn.Content = window1_lm.categs_btn[jPass.lang];
            fields_btn.Content = window1_lm.fields_btn[jPass.lang];
            accounts_btn.Content = window1_lm.accounts_btn[jPass.lang];
            optCap_lb.Content = window1_lm.optCap_lb[jPass.lang];

            filterMain_dtg.Columns[0].Header = window1_lm.filterMain_dtg_cImg[jPass.lang];
            filterMain_dtg.Columns[1].Header = window1_lm.filterMain_dtg_cName[jPass.lang];
            filterMain_dtg.Columns[2].Header = window1_lm.filterMain_dtg_q[jPass.lang];
            searchRes_dtg.Columns[0].Header = window1_lm.searchRes_dtg_gImg[jPass.lang];
            searchRes_dtg.Columns[1].Header = window1_lm.searchRes_dtg_cImg[jPass.lang];
            searchRes_dtg.Columns[2].Header = window1_lm.searchRes_dtg_accName[jPass.lang];
            searchRes_dtg.Columns[3].Header = window1_lm.searchRes_dtg_gName[jPass.lang];
            searchRes_dtg.Columns[4].Header = window1_lm.searchRes_dtg_cName[jPass.lang];
            searchRes_dtg.Columns[5].Header = window1_lm.searchRes_dtg_lUpd[jPass.lang];
            searchRes_dtg.Columns[6].Header = window1_lm.searchRes_dtg_cm[jPass.lang];

            srpFound_lbl.Content = window1_lm.srpFound_lbl[jPass.lang];
            srfFilter_lbl.Content = window1_lm.srfFilter_lbl[jPass.lang];
            cpLogin_btn.Content = window1_lm.cpLogin_btn[jPass.lang];
            cpPassword_btn.Content = window1_lm.cpPassword_btn[jPass.lang];
            srpRefresh_btn.Content = window1_lm.srpRefresh_btn[jPass.lang];

            fmRefresh_btn.Content = window1_lm.fmRefresh_btn[jPass.lang];

        }

        public Window1()
        {
            InitializeComponent();
        }

        private void jpMain_win_Loaded(object sender, RoutedEventArgs e)
        {

            useWin1_lm();

            dataDir_lb.Content = jpOptions.data_dir;
            iniFile_lb.Content = jpOptions.ini_file;

            jPass.jpCategories.data_dir = jpOptions.data_dir;
            jPass.jpCategories.fillJCList();

            jPass.jpGroups.data_dir = jpOptions.data_dir;
            jPass.jpGroups.fillJGList();

            jPass.jpFields.fillJFList();

            jPass.jpAccounts.fillJAList();

            refreshGroupsList();
            refreshAccList();
            refreshCategsList();

            filterMain_dtg.ItemsSource = refreshedCat_list;

            curwAccFields = new jPassAccFields();

            cpLogin_btn.Visibility = Visibility.Hidden;
            cpPassword_btn.Visibility = Visibility.Hidden; 

        }

        public void refreshAccList()
        {
            refreshedAcc_list.Clear();
            foreach (var item in jPass.jpAccounts.jAccountsList) {
                refreshedAcc_list.Add(item);
            }
            foundCount_lbl.Content = refreshedAcc_list.Count().ToString();
            filterAccList();
        }

        public void filterAccList()
        {
            searchRes_dtg.ItemsSource = null;

            List<jPassAccount> filteredAcc_list = new List<jPassAccount>();

            bool use_filter = false;

            if (useFilterGroups)
            {
                if (groups_cmb.Text == window1_lm.group_no_group[jPass.lang])
                {
                    filteredAcc_list = refreshedAcc_list.Where(x => x.group_id == "").ToList();
                    group_img.Source = jPass.default_img;
                }
                else
                {
                    jPassGroup grCmb = jPass.jpGroups.jGroupsList.Single(r => r.name == groups_cmb.Text);

                    group_img.Source = grCmb.image;

                    filteredAcc_list = refreshedAcc_list.Where(x => x.group_id == grCmb.id).ToList();
                }

                use_filter = true;
            }
            else {
                filteredAcc_list = refreshedAcc_list;
            }

            if (useFilterCats)
            {
                filteredAcc_list = filteredAcc_list.Where(x => x.category_id == ((dynamic)filterMain_dtg.SelectedItem).id).ToList();
                use_filter = true;
            }

            if (accFilterName_tb.Text != "")
            {
                use_filter = true;

                filteredAcc_list = filteredAcc_list.Where(x => x.name.ToLower().Contains(accFilterName_tb.Text.ToLower())).ToList();
            }

            if (use_filter)
            {
                useFilter_lbl.Background = Brushes.Firebrick;
                useFilter_lbl.Foreground = Brushes.White;
                useFilter_lbl.Content = window1_lm.useFilter_lbl_on[jPass.lang];
            }
            else {
                useFilter_lbl.Background = Brushes.Silver;
                useFilter_lbl.Content = window1_lm.useFilter_lbl_off[jPass.lang];
                useFilter_lbl.Foreground = Brushes.Black;
            }

            searchRes_dtg.ItemsSource = filteredAcc_list;
            foundCount_lbl.Content = filteredAcc_list.Count().ToString();

            cpLogin_btn.Visibility = Visibility.Hidden;
            cpPassword_btn.Visibility = Visibility.Hidden;
        }

        public void refreshCategsList()
        {
            refreshedCat_list.Clear();
            foreach (var item in jPass.jpCategories.jCategoriesList)
            {
                item.usageCount = jPass.jpAccounts.jAccountsList.Where(x => x.category_id == item.id).Count();
                refreshedCat_list.Add(item);
            }

            int noCat_count = jPass.jpAccounts.jAccountsList.Where(x => x.category_id == "").Count();

            if (noCat_count > 0) { 
                jPassCategory addNoCat = new jPassCategory();
                addNoCat.id = "";
                addNoCat.image = new BitmapImage(new Uri(@"/jPass_new;component/jpData/default_img.png", UriKind.Relative));
                addNoCat.name = window1_lm.cat_no_cat[jPass.lang];
                addNoCat.usageCount = noCat_count;
                refreshedCat_list.Add(addNoCat);

            }

            useFilterCats = false;
        }

        public void refreshGroupsList()
        {
            groups_cmb.Items.Clear();
            groups_cmb.Items.Add(window1_lm.group_any_group[jPass.lang]);

            foreach (var item in jPass.jpGroups.jGroupsList.OrderBy(x => x.name))
            {
                groups_cmb.Items.Add(item.name);
            }

            int noGroups_count = jPass.jpAccounts.jAccountsList.Where(x => x.group_id == "").Count();

            if (noGroups_count > 0)
            {
                groups_cmb.Items.Add(window1_lm.group_no_group[jPass.lang]);
            }

            groups_cmb.Text = window1_lm.group_any_group[jPass.lang];
            group_img.Source = jPass.default_img;
            useFilterGroups = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window groups_win = new Window2();
            groups_win.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window categs_win = new Window3();
            categs_win.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window fields_win = new Window4();
            fields_win.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Window accounts_win = new Window5();
            accounts_win.ShowDialog();

        }

        private void searchRes_dtg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Window6 accFields_win = new Window6();
            accFields_win.jPassAccFields = new jPassAccFields();
            accFields_win.jPassAccFields.jPassAccount = jPass.jpAccounts.jAccountsList.Single(r => r.id == ((dynamic)searchRes_dtg.SelectedItem).id);
            accFields_win.jPassAccFields.fillAccFileds();
            accFields_win.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            
            refreshAccList();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            refreshGroupsList();
            filterMain_dtg.ItemsSource = null;
            refreshCategsList();
            filterMain_dtg.ItemsSource = refreshedCat_list;
            filterAccList();

        }

        private void searchRes_dtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                curwAccFields.jPassAccount = jPass.jpAccounts.jAccountsList.Single(r => r.id == ((dynamic)searchRes_dtg.SelectedItem).id);

                curwAccFields.fillAccFileds();

                bool foundPass_f = false;
                bool foundLogin_f = false;

                foreach (var accFiled in curwAccFields.jAccFieldsList) {

                    if (accFiled.field_id == jpOptions.passAccField.field_id) {
                        foundPass_f = true;
                    }
                    if (accFiled.field_id == jpOptions.loginAccField.field_id)
                    {
                        foundLogin_f = true;
                    }
                    if (foundPass_f && foundLogin_f) {
                        break;
                    }
                }

                if (foundPass_f)
                {
                    cpPassword_btn.Visibility = Visibility.Visible;
                }
                else {
                    cpPassword_btn.Visibility = Visibility.Hidden;
                }

                if (foundLogin_f)
                {
                    cpLogin_btn.Visibility = Visibility.Visible;
                }
                else
                {
                    cpLogin_btn.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cpPassword_btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var accFiled in curwAccFields.jAccFieldsList)
            {
                if (accFiled.field_id == jpOptions.passAccField.field_id)
                {
                    Clipboard.SetText(accFiled.decrypt_val);
                    return;
                }
            }
        }

        private void cpLogin_btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var accFiled in curwAccFields.jAccFieldsList)
            {
                if (accFiled.field_id == jpOptions.loginAccField.field_id)
                {
                    Clipboard.SetText(accFiled.val);
                    return;
                }
            }
        }

        private void accFilterName_tb_KeyUp(object sender, KeyEventArgs e)
        {
            filterAccList();
        }

        private void filterMain_dtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                useFilterCats = true;
                filterAccList();
            }
            catch (Exception ex)
            {

            }
        }

        private void groups_cmb_DropDownClosed(object sender, EventArgs e)
        {
            if (groups_cmb.Text == window1_lm.group_any_group[jPass.lang])
            {
                group_img.Source = jPass.default_img;
                useFilterGroups = false;
                refreshAccList();
            }
            else {

                useFilterGroups = true;
                filterAccList();
            }
        }

        private void about_lbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Window7 aboutWin = new Window7();
            aboutWin.ShowDialog();
        }
    }
}
