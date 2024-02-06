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
using Microsoft.Win32;
using System.Data.Common;
using System.Runtime.InteropServices.ComTypes;

namespace jPass_new
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public jPassGroup jPassGroup { get; set; }
        public Window2()
        {
            InitializeComponent();
        }

        class Window2_lm
        {
            public string[] titleGroup_lb_new = new string[2] { "New Group", "Создать группу" };
            public string[] titleGroup_lb_edit = new string[2] { "Edit Group", "Правка группы" };
            public string[] winTitle = new string[2] { "Groups", "Группы" };
            public string[] update_btn_save = new string[2] { "Save", "Сохранить" };
            public string[] update_btn_update = new string[2] { "Update", "Обновить" };

            public string[] del_btn = new string[2] { "delete", "Удалить" };
            public string[] new_btn = new string[2] { "new", "Создать" };
            public string[] img_btn = new string[2] { "Img", "Изобр." };

            public string[] groupName_lbl = new string[2] { "groupName:", "Название группы:" };


            public string[] groupName_default = new string[2] { "new-group-name", "Новая группа - имя" };

            public string[] updateErr_not_changed = new string[2] { "name not changed", "наименование не менялось" };
            public string[] updateErr_double = new string[2] { "Double name error", "Повтор имени - ошибка" };
            public string[] updateErr_reserved = new string[2] { "Name reserved ", "Имя зарезервировано" };

            public string[] useFilter_lbl_on = new string[2] { "On", "Вкл" };
            public string[] useFilter_lbl_off = new string[2] { "Off", "Выкл" };
            public string[] srpRefresh_btn = new string[2] { "Refresh", "Обновить" };
            public string[] srpFound_lbl = new string[2] { "found: ", "Найдено: " };
            public string[] srfFilter_lbl = new string[2] { "filter: ", "Фильтр: " };


            public string[] info_noErr = new string[2] { "no-err", "Нет ошибок" };

            public string[] dialogTitle = new string[2] { "Pick file", "Выберите файл" };

            

            public string[] dtg_gImg = new string[2] { "gImg", "гИз" };
            public string[] dtg_gName = new string[2] { "groupName", "Название группы" };

            public string[] alert_del_conf = new string[2] { " account in this group. Are you sure?", " учеток в этой группе, Вы уверены?" };
            public string[] alert_del_title = new string[2] { "Delete Confirmation", "Подтверждение удаления" };

        }

        Window2_lm window2_lm = new Window2_lm();
        void useWin2_lm()
        {
            titleGroup_lb.Content = window2_lm.titleGroup_lb_new[jPass.lang];
            Title = window2_lm.winTitle[jPass.lang];
            groupName_tb.Text = window2_lm.groupName_default[jPass.lang];

            update_btn.Content = window2_lm.update_btn_save[jPass.lang];
            del_btn.Content = window2_lm.del_btn[jPass.lang];
            new_btn.Content = window2_lm.new_btn[jPass.lang];
            img_btn.Content = window2_lm.img_btn[jPass.lang];
            
            groupName_lbl.Content = window2_lm.groupName_lbl[jPass.lang];

            srpFound_lbl.Content = window2_lm.srpFound_lbl[jPass.lang];
            srfFilter_lbl.Content = window2_lm.srfFilter_lbl[jPass.lang];
            srpRefresh_btn.Content = window2_lm.srpRefresh_btn[jPass.lang];


            groups_dtg.Columns[0].Header = window2_lm.dtg_gImg[jPass.lang];
            groups_dtg.Columns[1].Header = window2_lm.dtg_gName[jPass.lang];

            info_lb.Content = window2_lm.info_noErr[jPass.lang];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string update_err = window2_lm.info_noErr[jPass.lang];

            if (jPassGroup.id == null)
            {
                if (jPass.jpGroups.jGroupsList.Where(x => x.name == groupName_tb.Text).Count() == 0)
                {
                    if (jPass.checkReservedWords(groupName_tb.Text))
                    {
                        jPassGroup.id = Guid.NewGuid().ToString();

                        int lineCount = File.ReadLines(jpOptions.data_dir + "\\groups.jpass").Count();
                        jPassGroup.name = groupName_tb.Text;
                        jPassGroup.lineNum = lineCount;
                        jPassGroup.image = jPass.default_img;

                        File.AppendAllText(jpOptions.data_dir + "\\groups.jpass",
                            "id: " + jPassGroup.id + ", name: " + jPassGroup.name +
                            ", img: " + jPassGroup.img + Environment.NewLine);

                        img_btn.IsEnabled = true;
                        titleGroup_lb.Content = window2_lm.titleGroup_lb_edit[jPass.lang];
                        update_btn.Content = window2_lm.update_btn_update[jPass.lang];
                        del_btn.IsEnabled = true;
                        groupId_lb.Content = "id: " + jPassGroup.id;
                        jPass.jpGroups.jGroupsList.Add(jPassGroup);
                        groups_dtg.ItemsSource = null;
                        groups_dtg.ItemsSource = jPass.jpGroups.jGroupsList;
                        new_btn.Visibility = Visibility.Visible;
                        del_btn.Visibility = Visibility.Visible;
                    }
                    else {
                        update_err = window2_lm.updateErr_reserved[jPass.lang];
                    }

                }
                else {
                    update_err = window2_lm.updateErr_double[jPass.lang];
                }
            }
            else
            {
                if (jPassGroup.name != groupName_tb.Text)
                {
                    if (jPass.jpGroups.jGroupsList.Where(x => x.name == groupName_tb.Text).Count() == 0)
                    {
                        if (jPass.checkReservedWords(groupName_tb.Text))
                        {
                            string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\groups.jpass");

                            jPassGroup.name = groupName_tb.Text;

                            arrLine[jPassGroup.lineNum] = "id: " + jPassGroup.id + ", name: " + jPassGroup.name +
                                ", img: " + jPassGroup.img;

                            File.WriteAllLines(jpOptions.data_dir + "\\groups.jpass", arrLine);

                            jPass.jpGroups.jGroupsList[jPassGroup.lineNum].name = jPassGroup.name;

                            groups_dtg.ItemsSource = null;
                            groups_dtg.ItemsSource = jPass.jpGroups.jGroupsList;
                        }
                        else {
                            update_err = window2_lm.updateErr_reserved[jPass.lang];
                        }                            
                    }
                    else {
                        update_err = window2_lm.updateErr_double[jPass.lang];

                    }
                }
                else {
                    update_err = window2_lm.updateErr_not_changed[jPass.lang];
                }
            }
            string time = DateTime.Now.ToString("hh:mm:ss");
            string date = DateTime.Now.ToString("dd/MM/yy");

            info_lb.Content = update_err + " " + time;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            useWin2_lm();
            jPassGroup = new jPassGroup();
            img_btn.IsEnabled = false;
            del_btn.Visibility = Visibility.Hidden;
            filterGroupsList();
            new_btn.Visibility = Visibility.Hidden;
            group_img.Source = jPass.default_img;
        }

        public void filterGroupsList()
        {
            groups_dtg.ItemsSource = null;

            bool use_filter = false;

            List<jPassGroup> filteredGroups_list = new List<jPassGroup>();

            if (groupsFilterName_tb.Text != "")
            {
                use_filter = true;

                filteredGroups_list = jPass.jpGroups.jGroupsList.Where(x => x.name.ToLower().Contains(groupsFilterName_tb.Text.ToLower())).ToList();
            }
            else {
                filteredGroups_list = jPass.jpGroups.jGroupsList;
            }

            if (use_filter)
            {
                useFilter_lbl.Background = Brushes.Firebrick;
                useFilter_lbl.Foreground = Brushes.White;
                useFilter_lbl.Content = window2_lm.useFilter_lbl_on[jPass.lang];
            }
            else
            {
                useFilter_lbl.Background = Brushes.Silver;
                useFilter_lbl.Content = window2_lm.useFilter_lbl_off[jPass.lang];
                useFilter_lbl.Foreground = Brushes.Black;
            }

            groups_dtg.ItemsSource = filteredGroups_list;
            foundCount_lbl.Content = filteredGroups_list.Count().ToString();

        }

        private void groups_dtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                jPassGroup = jPass.jpGroups.jGroupsList.Single(r => r.id == ((dynamic)groups_dtg.SelectedItem).id);
                group_img.Source = jPassGroup.image;
                groupId_lb.Content = "id: " + jPassGroup.id;
                groupName_tb.Text = jPassGroup.name;
                img_btn.IsEnabled = true;
                del_btn.Visibility = Visibility.Visible;
                titleGroup_lb.Content = window2_lm.titleGroup_lb_edit[jPass.lang];
                new_btn.Visibility = Visibility.Visible;
                update_btn.Content = window2_lm.update_btn_update[jPass.lang];
                info_lb.Content = window2_lm.info_noErr[jPass.lang];
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
                Title = window2_lm.dialogTitle[jPass.lang],
                Filter = "Files|*.jpg;*.jpeg;*.png;*.bmp;",
            };

            if (dialog.ShowDialog() == true)
            {
                string filename = dialog.FileName;
                string extension = System.IO.Path.GetExtension(dialog.FileName);

                if (jPassGroup.img != null)
                {
                    File.Delete(jpOptions.data_dir + "\\img\\" + jPassGroup.img);
                }

                jPassGroup.img = Guid.NewGuid().ToString() + extension;

                File.Copy(filename, jpOptions.data_dir + "\\img\\" + jPassGroup.img);

                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\groups.jpass");

                arrLine[jPassGroup.lineNum] = "id: " + jPassGroup.id + ", name: " + jPassGroup.name +
                    ", img: " + jPassGroup.img;

                File.WriteAllLines(jpOptions.data_dir + "\\groups.jpass", arrLine);

                jPassGroup.image = jPass.toBitmap(File.ReadAllBytes(jpOptions.data_dir + "\\img\\" + jPassGroup.img));

                group_img.Source = jPassGroup.image;

                string time = DateTime.Now.ToString("hh:mm:ss");
                string date = DateTime.Now.ToString("dd/MM/yy");

                jPass.jpGroups.jGroupsList[jPassGroup.lineNum].image = jPassGroup.image;

                groups_dtg.ItemsSource = null;
                groups_dtg.ItemsSource = jPass.jpGroups.jGroupsList;

                info_lb.Content = window2_lm.info_noErr[jPass.lang] + time;
            }
        }

        private void new_btn_Click(object sender, RoutedEventArgs e)
        {
            del_btn.Visibility = Visibility.Hidden;
            groupName_tb.Text = window2_lm.groupName_default[jPass.lang];
            groupId_lb.Content = "id: ";
            group_img.Source = jPass.default_img;
            jPassGroup = new jPassGroup();
            titleGroup_lb.Content = window2_lm.titleGroup_lb_new[jPass.lang];
            img_btn.IsEnabled = false;
            update_btn.Content = window2_lm.update_btn_save[jPass.lang];
            new_btn.Visibility = Visibility.Hidden;
        }

        private void del_btn_Click(object sender, RoutedEventArgs e)
        {
            int countAccGroup = jPass.jpAccounts.jAccountsList.Where(x => x.group_id == jPassGroup.id).Count();

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(countAccGroup.ToString()+window2_lm.alert_del_conf[jPass.lang], 
                window2_lm.alert_del_title[jPass.lang], System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {               
                if (countAccGroup > 0) {
                    string[] updateGrAcc = new string[jPass.jpAccounts.jAccountsList.Count()];
                    int accGrCounter = 0;

                    foreach (var account in jPass.jpAccounts.jAccountsList) {
                        if (account.group_id == jPassGroup.id) {
                            jPass.jpAccounts.jAccountsList[accGrCounter].group_id = "";
                            jPass.jpAccounts.jAccountsList[accGrCounter].groupImage = jPass.default_img;
                            jPass.jpAccounts.jAccountsList[accGrCounter].group_name = "";
                            account.group_id = "";
                        }

                        updateGrAcc[accGrCounter] = "id: " + account.id + ", name: " + account.name +
   ", group_id: " + account.group_id + ", category_id: " + account.category_id +
   ", comment: " + account.comment + ", lastUpdate: " + account.lastUpdate;

                        accGrCounter++;
                    }
                    File.WriteAllLines(jpOptions.data_dir + "\\accounts.jpass", updateGrAcc);
                }
                
                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\groups.jpass");

                var itemToRemove = jPass.jpGroups.jGroupsList.Single(r => r.id == jPassGroup.id);

                jPass.jpGroups.jGroupsList.Remove(itemToRemove);

                arrLine = arrLine.Where(w => w != arrLine[jPassGroup.lineNum]).ToArray();

                File.WriteAllLines(jpOptions.data_dir + "\\groups.jpass", arrLine);

                int sListCounter = 0;
                foreach (var jGroup in jPass.jpGroups.jGroupsList)
                {
                    jPass.jpGroups.jGroupsList[sListCounter].lineNum = sListCounter;
                    sListCounter++;
                }

                groups_dtg.ItemsSource = null;
                groups_dtg.ItemsSource = jPass.jpGroups.jGroupsList;

                if (jPassGroup.img != null)
                {
                    File.Delete(jpOptions.data_dir + "\\img\\" + jPassGroup.img);
                }

                del_btn.Visibility = Visibility.Hidden;
                groupName_tb.Text = window2_lm.groupName_default[jPass.lang];
                groupId_lb.Content = "id: ";
                group_img.Source = jPass.default_img;
                jPassGroup = new jPassGroup();
                titleGroup_lb.Content = window2_lm.titleGroup_lb_new[jPass.lang];

                img_btn.IsEnabled = false;
                update_btn.Content = window2_lm.update_btn_save[jPass.lang];
                new_btn.Visibility = Visibility.Hidden;
                
            }
        }

        private void groupsFilterName_tb_KeyUp(object sender, KeyEventArgs e)
        {
            filterGroupsList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            groupsFilterName_tb.Text = null;
            filterGroupsList();
        }
    }
}
