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

namespace jPass_new
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public jPassCategory jPassCategory { get; set; }
        public Window3()
        {
            InitializeComponent();
        }

        class Window3_lm
        {
            public string[] winTitle = new string[2] { "Categories", "Категории" };

            public string[] titleCateg_lb_new = new string[2] { "New Category", "Создать категорию" };
            public string[] titleCateg_lb_edit = new string[2] { "Edit Category", "Правка категории" };

            public string[] update_btn_save = new string[2] { "Save", "Сохранить" };
            public string[] update_btn_update = new string[2] { "Update", "Обновить" };
            public string[] del_btn = new string[2] { "delete", "Удалить" };
            public string[] new_btn = new string[2] { "new", "Создать" };
            public string[] img_btn = new string[2] { "Img", "Изобр." };


            public string[] categoryName_lbl = new string[2] { "categoryName:", "Название категории: " };


            public string[] categoryName_default = new string[2] { "new-category-name", "Новая категория - имя" };
            
            public string[] updateErr_not_changed = new string[2] { "name not changed", "наименование не менялось" };
            public string[] updateErr_double = new string[2] { "Double name error", "Повтор имени - ошибка" };
            public string[] updateErr_reserved = new string[2] { "Name reserved ", "Имя зарезервировано" };

            public string[] useFilter_lbl_on = new string[2] { "On", "Вкл" };
            public string[] useFilter_lbl_off = new string[2] { "Off", "Выкл" };
            public string[] srpRefresh_btn = new string[2] { "Refresh", "Обновить" };
            public string[] srpFound_lbl = new string[2] { "found: ", "Найдено: " };
            public string[] srfFilter_lbl = new string[2] { "filter: ", "Фильтр: " };

            public string[] dialog_alert_message = new string[2] { " account in this category. Are you sure?", " учетки в этой категории. Вы уверены?" };
            public string[] dialog_alert_header = new string[2] { "Delete Confirmation", "Подтверждение удаления" };

            public string[] dialogTitle = new string[2] { "Pick file", "Выберите файл" };

            public string[] info_noErr = new string[2] { "no-err", "Нет ошибок" };

            public string[] dtc_cImg = new string[2] { "cImg", "кИз" };
            public string[] dtc_cName = new string[2] { "categoryName", "Название категории" };
        }

        Window3_lm window3_lm = new Window3_lm();
        void useWin3_lm()
        {
            titleCateg_lb.Content = window3_lm.titleCateg_lb_new[jPass.lang];
            Title = window3_lm.winTitle[jPass.lang];
            categoryName_tb.Text = window3_lm.categoryName_default[jPass.lang];

            del_btn.Content = window3_lm.del_btn[jPass.lang];
            new_btn.Content = window3_lm.new_btn[jPass.lang];
            img_btn.Content = window3_lm.img_btn[jPass.lang];
            update_btn.Content = window3_lm.update_btn_save[jPass.lang];

            srpFound_lbl.Content = window3_lm.srpFound_lbl[jPass.lang];
            srfFilter_lbl.Content = window3_lm.srfFilter_lbl[jPass.lang];
            srpRefresh_btn.Content = window3_lm.srpRefresh_btn[jPass.lang];

            info_lb.Content = window3_lm.info_noErr[jPass.lang];

            categoryName_lbl.Content = window3_lm.categoryName_lbl[jPass.lang];

            categs_dtg.Columns[0].Header = window3_lm.dtc_cImg[jPass.lang];
            categs_dtg.Columns[1].Header = window3_lm.dtc_cName[jPass.lang];
        }


        private void categs_dtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                jPassCategory = jPass.jpCategories.jCategoriesList.Single(r => r.id == ((dynamic)categs_dtg.SelectedItem).id);
                categ_img.Source = jPassCategory.image;
                categoryId_lb.Content = "id: " + jPassCategory.id;
                categoryName_tb.Text = jPassCategory.name;
                img_btn.IsEnabled = true;
                del_btn.Visibility = Visibility.Visible;
                titleCateg_lb.Content = window3_lm.titleCateg_lb_edit[jPass.lang];
                new_btn.Visibility = Visibility.Visible;
                update_btn.Content = window3_lm.update_btn_update[jPass.lang];
            }
            catch (Exception ex)
            {
                info_lb.Content = ex.Message;
            }
        }

        private void del_btn_Click(object sender, RoutedEventArgs e)
        {

            int countAccCat = jPass.jpAccounts.jAccountsList.Where(x => x.category_id == jPassCategory.id).Count();

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(countAccCat.ToString() + window3_lm.dialog_alert_message[jPass.lang],
                window3_lm.dialog_alert_header[jPass.lang], System.Windows.MessageBoxButton.YesNo);
            
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                if (countAccCat > 0)
                {
                    string[] updateCatAcc = new string[jPass.jpAccounts.jAccountsList.Count()];
                    int accGrCounter = 0;

                    foreach (var account in jPass.jpAccounts.jAccountsList)
                    {
                        if (account.category_id == jPassCategory.id)
                        {
                            jPass.jpAccounts.jAccountsList[accGrCounter].category_id = "";
                            jPass.jpAccounts.jAccountsList[accGrCounter].categoryImage = jPass.default_img;
                            jPass.jpAccounts.jAccountsList[accGrCounter].category_name = "";
                            account.category_id = "";
                        }

                        updateCatAcc[accGrCounter] = "id: " + account.id + ", name: " + account.name +
   ", group_id: " + account.group_id + ", category_id: " + account.category_id +
   ", comment: " + account.comment + ", lastUpdate: " + account.lastUpdate;

                        accGrCounter++;
                    }
                    File.WriteAllLines(jpOptions.data_dir + "\\accounts.jpass", updateCatAcc);
                }

                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\categories.jpass");


                var itemToRemove = jPass.jpCategories.jCategoriesList.Single(r => r.id == jPassCategory.id);

                jPass.jpCategories.jCategoriesList.Remove(itemToRemove);

                arrLine = arrLine.Where(w => w != arrLine[jPassCategory.lineNum]).ToArray();


                File.WriteAllLines(jpOptions.data_dir + "\\categories.jpass", arrLine);

                int sListCounter = 0;
                foreach (var jCategory in jPass.jpCategories.jCategoriesList)
                {
                    jPass.jpCategories.jCategoriesList[sListCounter].lineNum = sListCounter;
                    sListCounter++;
                }

                categs_dtg.ItemsSource = null;

                if (jPassCategory.img != null)
                {
                    File.Delete(jpOptions.data_dir + "\\img\\" + jPassCategory.img);
                }

                jPassCategory = null; ;
                img_btn.IsEnabled = false;
                del_btn.Visibility = Visibility.Hidden;
                categs_dtg.ItemsSource = jPass.jpCategories.jCategoriesList;
                new_btn.Visibility = Visibility.Hidden;
                categ_img.Source = jPass.default_img;
                titleCateg_lb.Content = window3_lm.titleCateg_lb_new[jPass.lang];
                categoryName_tb.Text = window3_lm.categoryName_default[jPass.lang];
                update_btn.Content = window3_lm.update_btn_save[jPass.lang];


            }
        }

        private void new_btn_Click(object sender, RoutedEventArgs e)
        {
            del_btn.Visibility = Visibility.Hidden;
            categoryName_tb.Text = window3_lm.categoryName_default[jPass.lang];
            categoryId_lb.Content = "id: ";
            categ_img.Source = jPass.default_img;
            jPassCategory = new jPassCategory();
            titleCateg_lb.Content = window3_lm.titleCateg_lb_new[jPass.lang];
            img_btn.IsEnabled = false;
            update_btn.Content = window3_lm.update_btn_save[jPass.lang];
            new_btn.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string update_err = window3_lm.info_noErr[jPass.lang];

            if (jPassCategory.id == null)
            {
                if (jPass.jpCategories.jCategoriesList.Where(x => x.name == categoryName_tb.Text).Count() == 0)
                {
                    if (jPass.checkReservedWords(categoryName_tb.Text))
                    {
                        jPassCategory.id = Guid.NewGuid().ToString();

                        int lineCount = File.ReadLines(jpOptions.data_dir + "\\categories.jpass").Count();
                        jPassCategory.name = categoryName_tb.Text;
                        jPassCategory.lineNum = lineCount;
                        jPassCategory.image = jPass.default_img;

                        File.AppendAllText(jpOptions.data_dir + "\\categories.jpass",
                            "id: " + jPassCategory.id + ", name: " + jPassCategory.name +
                            ", img: " + jPassCategory.img + Environment.NewLine);

                        img_btn.IsEnabled = true;
                        titleCateg_lb.Content = window3_lm.titleCateg_lb_edit[jPass.lang];
                        update_btn.Content = window3_lm.update_btn_update[jPass.lang];
                        del_btn.IsEnabled = true;
                        categoryId_lb.Content = "id: " + jPassCategory.id;
                        jPass.jpCategories.jCategoriesList.Add(jPassCategory);
                        categs_dtg.ItemsSource = null;
                        categs_dtg.ItemsSource = jPass.jpCategories.jCategoriesList;
                        new_btn.Visibility = Visibility.Visible;
                        del_btn.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        update_err = window3_lm.updateErr_reserved[jPass.lang];
                    }                        
                }
                else {
                    update_err = window3_lm.updateErr_double[jPass.lang];
                }
                    
            }
            else
            {

                if (jPassCategory.name != categoryName_tb.Text)
                {
                    if (jPass.jpCategories.jCategoriesList.Where(x => x.name == categoryName_tb.Text).Count() == 0)
                    {
                        if (jPass.checkReservedWords(categoryName_tb.Text))
                        {
                            string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\categories.jpass");

                            jPassCategory.name = categoryName_tb.Text;

                            arrLine[jPassCategory.lineNum] = "id: " + jPassCategory.id + ", name: " + jPassCategory.name +
                                ", img: " + jPassCategory.img;

                            File.WriteAllLines(jpOptions.data_dir + "\\categories.jpass", arrLine);

                            jPass.jpCategories.jCategoriesList[jPassCategory.lineNum].name = jPassCategory.name;

                            categs_dtg.ItemsSource = null;
                            categs_dtg.ItemsSource = jPass.jpCategories.jCategoriesList;
                        }
                        else {
                            update_err = window3_lm.updateErr_reserved[jPass.lang];
                        }                          
                    }
                    else {

                        update_err = window3_lm.updateErr_double[jPass.lang];
                    }
                        
                }
                else {

                    update_err = window3_lm.updateErr_not_changed[jPass.lang];
                }

            }
            string time = DateTime.Now.ToString("hh:mm:ss");
            string date = DateTime.Now.ToString("dd/MM/yy");

            info_lb.Content = update_err + time;
        }

        private void img_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = false,
                CheckPathExists = true,
                Multiselect = false,
                Title = window3_lm.dialogTitle[jPass.lang],
                Filter = "Files|*.jpg;*.jpeg;*.png;*.bmp;",
            };

            if (dialog.ShowDialog() == true)
            {
                string filename = dialog.FileName;
                string extension = System.IO.Path.GetExtension(dialog.FileName);

                if (jPassCategory.img != null)
                {

                    File.Delete(jpOptions.data_dir + "\\img\\" + jPassCategory.img);
                }

                jPassCategory.img = Guid.NewGuid().ToString() + extension;

                File.Copy(filename, jpOptions.data_dir + "\\img\\" + jPassCategory.img);

                string[] arrLine = File.ReadAllLines(jpOptions.data_dir + "\\categories.jpass");

                arrLine[jPassCategory.lineNum] = "id: " + jPassCategory.id + ", name: " + jPassCategory.name +
                    ", img: " + jPassCategory.img;

                File.WriteAllLines(jpOptions.data_dir + "\\categories.jpass", arrLine);

                jPassCategory.image = jPass.toBitmap(File.ReadAllBytes(jpOptions.data_dir + "\\img\\" + jPassCategory.img));

                categ_img.Source = jPassCategory.image;

                string time = DateTime.Now.ToString("hh:mm:ss");
                string date = DateTime.Now.ToString("dd/MM/yy");

                jPass.jpCategories.jCategoriesList[jPassCategory.lineNum].image = jPassCategory.image;

                categs_dtg.ItemsSource = null;
                categs_dtg.ItemsSource = jPass.jpCategories.jCategoriesList;

                info_lb.Content = window3_lm.info_noErr[jPass.lang] + time;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            useWin3_lm();


            jPassCategory = new jPassCategory();
            img_btn.IsEnabled = false;
            del_btn.Visibility = Visibility.Hidden;

            filterCategsList();
            new_btn.Visibility = Visibility.Hidden;
            categ_img.Source = jPass.default_img;
        }

        public void filterCategsList()
        {
            categs_dtg.ItemsSource = null;

            bool use_filter = false;

            List<jPassCategory> filteredCategs_list = new List<jPassCategory>();

            if (categsFilterName_tb.Text != "")
            {
                use_filter = true;

                filteredCategs_list = jPass.jpCategories.jCategoriesList.Where(x => x.name.ToLower().Contains(categsFilterName_tb.Text.ToLower())).ToList();
            }
            else
            {
                filteredCategs_list = jPass.jpCategories.jCategoriesList;
            }

            if (use_filter)
            {
                useFilter_lbl.Background = Brushes.Firebrick;
                useFilter_lbl.Foreground = Brushes.White;
                useFilter_lbl.Content = window3_lm.useFilter_lbl_on[jPass.lang];
            }
            else
            {
                useFilter_lbl.Background = Brushes.Silver;
                useFilter_lbl.Content = window3_lm.useFilter_lbl_off[jPass.lang];
                useFilter_lbl.Foreground = Brushes.Black;
            }

            categs_dtg.ItemsSource = filteredCategs_list;
            foundCount_lbl.Content = filteredCategs_list.Count().ToString();

        }

        private void categsFilterName_tb_KeyUp(object sender, KeyEventArgs e)
        {
            filterCategsList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            categsFilterName_tb.Text = null;
            filterCategsList();
        }
    }
}
