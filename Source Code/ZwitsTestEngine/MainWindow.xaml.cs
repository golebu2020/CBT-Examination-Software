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
using System.Threading;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Media.Animation;
using System.IO;
using System.Data.Entity;
using System.Collections;
using System.Windows.Threading;
using System.Resources;


namespace ZwitsTestEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BackgroundWorker bWorker = new BackgroundWorker();
        DispatcherTimer timer = new DispatcherTimer();
        string[] objectives = new string[]{"Agriculture","Arabic","Art","Biology","Chemistry","Commerce","CRS",
                "Economics","French","Geography","Government","Hausa","History","HomeEcons1","IGBO","Introduction",
                "IslamicStudies1","Literature","Mathematics","Music","Physics","PrinciplesofAcct","UseofEnglish","Yoruba"};

        string[] faculties = new string[] {"Administration","Agriculture","Art and Humanities","Education","Engineering and Engineering Technology","Law","Medical, Pharmaceutical and HealthScience","Sciences","Social and Management Science"};
        public MainWindow()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            InitializeComponent();
            bWorker.WorkerReportsProgress = true;
            bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
            bWorker.ProgressChanged += new ProgressChangedEventHandler(bWorker_ProgressChanged);
            bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWorker_RunWorkerCompleted);
            lblOlufisayo.Opacity = 100;
            lblProgram.Opacity = 100;
            DoGridThings();
            LoadInCombo();
            ArrayList ComboExamTypeSet = new ArrayList() { "SetA", "SetB", "SetC", "SetD", "SetE", "SetF", "SetG", "SetH" };
            foreach (var i in ComboExamTypeSet)
                comboExamType.Items.Add(i);
            lblTiming.Opacity = 0;

            pdfdisplay.Navigate(new System.Uri(@"c:\pyvision.pdf"));

            //this line of code loads the comboboxof cbshowgeneralobjectives


            foreach (var k in objectives)
            {
                lstObjectives.Items.Add(k);
            }
            foreach (var i in faculties)
            {
                lstFaculties.Items.Add(i);
            }
          





        }
        int TIMECOUNTER = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            lblTiming.Content = DateTime.Now.ToLongTimeString();
            TIMECOUNTER = TIMECOUNTER + 1;
            if (TIMECOUNTER == 7)
            {
                ShowTimeNotification();
            }
            if (TIMECOUNTER == 10)
            {
                TIMECOUNTER = 0;
                lblTiming.Opacity = 0;
                tabRoot.SelectedIndex = 0;
                timer.Stop();
                tbControlOptions.Opacity = 0;
            }
        }
        private void ShowTimeNotification()
        {

        }
        #region This code block helps in populating the comboexamID combobox so that thera can be an uodatinfgo of ej f

        public void LoadInCombo()
        {
            comboExamID.Items.Clear();
            var tables = ObjEmp.Tables;
            foreach (var table in tables)
            {
                comboExamID.Items.Add(table.StudentId);
            }
        }
        #endregion
        //instantiating the Entity of the data context under consideration
        AdminEntities ObjEmp = new AdminEntities();
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        int backgroundWorkerChecker = 1;
        #region
        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {

            backgroundWorkerChecker = 1;
            adminpassword = passManagement.Password;
            progressBar.Opacity = 100;
            bWorker.RunWorkerAsync();
        }
        string adminpassword; string contextadminpassword;
        private void ValidatePassword()
        {
            var contexts = ObjEmp.AdminTables;
            foreach (var context in contexts)
            {
                contextadminpassword = context.AdminPassword;
                if (context.AdminPassword == adminpassword)
                {
                }
            }
        }
        private void passManagement_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                adminpassword = passManagement.Password;
                progressBar.Opacity = 100;
                bWorker.RunWorkerAsync();
            }
        }
        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorkerChecker == 1)
            {
                for (int i = 0; i <= 10; i++)
                {
                    ValidatePassword();
                    bWorker.ReportProgress(i);
                }
            }
            if (backgroundWorkerChecker == 2)
            {
                InsertStudentInfo();
                Thread.Sleep(500);

            }
        }
        private void bWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (backgroundWorkerChecker == 1)
                progressBar.Value = e.ProgressPercentage * 10;
            if (backgroundWorkerChecker == 2)
                lblprocessing.Content = "       Processing...";
        }
        private void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorkerChecker == 1)
            {
                if (contextadminpassword == adminpassword)
                {
                    tabRoot.SelectedIndex = 1;
                    progressBar.Opacity = 0;
                    ErrorImage.Opacity = 0;
                    passManagement.Password = "";
                }
                else
                {
                    progressBar.Opacity = 0;
                    ErrorImage.Opacity = 100;
                }
            }
            if (backgroundWorkerChecker == 2)
            {
                List<TextBox> studentInput = new List<TextBox>() { txtName, txtSurname, txtExamID };
                Photo.Opacity = 0;
                LoadInCombo();
                DoGridThings();
                statusBar.Text = "SuccessFul Operation";
                foreach (var i in studentInput)
                { i.Clear(); comboExamType.Text = ""; }
                LoadInCombo();
                lblprocessing.Content = "";
            }
        }
        private void Expander_Collapsed_1(object sender, RoutedEventArgs e)
        { }
        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        { }
        #endregion
        //this function of method dows htwe  animatiio n of thje scrolling od hw  text.
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleanimation = new DoubleAnimation();
            doubleanimation.From = -tbmarquee.ActualHeight;
            doubleanimation.To = canMain.ActualWidth;
            doubleanimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleanimation.Duration = new Duration(TimeSpan.Parse("0:0:20"));
            tbmarquee.BeginAnimation(Canvas.LeftProperty, doubleanimation);


            // the below block of code populates the comboboxes
            List<string> myModule = new List<string>() { "Module1", "Module2", "Module3", "Module4", "Module5" };
            List<string> mySubject = new List<string>(){"Physics","Chemistry","Biology","Mathematics","Literature","CRK","Government","Economics",
                 "Commerce","Accounting"};
            string[] OptionAnswer = new string[] { "A", "B", "C", "D" };
            foreach (var i in myModule)
            {
                cbSelectModule.Items.Add(i);
                cbGlobalModule.Items.Add(i);
            }
            foreach (var j in mySubject)
                cbSelectSubject.Items.Add(j);
            foreach (var k in OptionAnswer)
                cbSelectAnswer.Items.Add(k);
            for (int t = 1; t <= 50; t++)
                cbSelectQuestionNumber.Items.Add(t);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_PreviewMouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            tabRoot.SelectedIndex = 0;
        }

        private void Image_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void tabRoot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #region This populate the grid
        private void DoGridThings()
        {
            System.Windows.Data.CollectionViewSource tableViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // tableViewSource.Source = [generic data source]
            AdminEntities context = new AdminEntities();
            context.Tables.Load();
            tableViewSource.Source = context.Tables.Local;
            context.Dispose();
        }
        #endregion
        //this part of the code shows the start of the operation of adding to the database
        private void btnSubmitStudentInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblprocessing.Content = "       Processsing...";
                AcceptInput();
                RadioSignal.IsChecked = true;
                backgroundWorkerChecker = 2;

                bWorker.RunWorkerAsync();

            }
            catch { MessageBox.Show("Please Enter Correct Info."); }
        }
        #region this block of code inserts the information into the database and makes the Photo too white
        string txtNameValue, txtSurnameValue, txtExamIDValue, comboExamTypeValue;
        private void AcceptInput()
        {
            txtNameValue = txtName.Text;
            txtSurnameValue = txtSurname.Text;
            txtExamIDValue = txtExamID.Text;
            comboExamTypeValue = comboExamType.Text;
        }
        private void InsertStudentInfo()
        {
            Table table = new Table();
            StudentExamTable examtable = new StudentExamTable();
            try
            {
                // for Table Data Source
                int Tracker = ObjEmp.Tables.Max(u => u.StudentId);
                table.StudentId = Tracker + 1;
                table.Name = txtNameValue;
                table.Surname = txtSurnameValue;
                table.ExamID = txtExamIDValue;
                table.ExamType = comboExamTypeValue;
                table.Passport = File.ReadAllBytes(MyPhoto);

                // for StudentExamTable Data Source
                int studentTracker = ObjEmp.StudentExamTables.Max(u => u.StudentId);
                examtable.StudentId = studentTracker + 1;
                examtable.StudentName = txtNameValue;
                examtable.StudentSurname = txtSurnameValue;
                examtable.StudentSet = comboExamTypeValue;

                #region Adding the required subjects to the table for each registered student and asigning a global value
                if (comboExamTypeValue == "SetA")
                {
                    examtable.StudentSubject1 = "Physics";
                    examtable.StudentSubject2 = "Chemistry";
                    examtable.StudentSubject3 = "Biology";
                }
                if (comboExamTypeValue == "SetB")
                {
                    examtable.StudentSubject1 = "Physics";
                    examtable.StudentSubject2 = "Chemistry";
                    examtable.StudentSubject3 = "Mathematics";
                }
                if (comboExamTypeValue == "SetC")
                {
                    examtable.StudentSubject1 = "Literature";
                    examtable.StudentSubject2 = "C.R.K";
                    examtable.StudentSubject3 = "Government";
                }
                if (comboExamTypeValue == "SetD")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Government";
                    examtable.StudentSubject3 = "Literature";
                }
                if (comboExamTypeValue == "SetE")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Accounting";
                }
                if (comboExamTypeValue == "SetF")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Commerce";
                }
                if (comboExamTypeValue == "SetG")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Government";
                }
                if (comboExamTypeValue == "SetH")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Physics";
                }
                #endregion
            }
            catch
            {
                table.StudentId = 1;
                table.Name = txtNameValue;
                table.Surname = txtSurnameValue;
                table.ExamID = txtExamIDValue;
                table.ExamType = comboExamTypeValue;
                table.Passport = File.ReadAllBytes(MyPhoto);


                int studentTracker = 1;
                examtable.StudentId = studentTracker + 1;
                examtable.StudentName = txtNameValue;
                examtable.StudentSurname = txtSurnameValue;
                examtable.StudentSet = comboExamTypeValue;
                #region Adding the required subjects to the table for each registered student and asigning a global value
                if (comboExamTypeValue == "SetA")
                {
                    examtable.StudentSubject1 = "Physics";
                    examtable.StudentSubject2 = "Chemistry";
                    examtable.StudentSubject3 = "Biology";
                }
                if (comboExamTypeValue == "SetB")
                {
                    examtable.StudentSubject1 = "Physics";
                    examtable.StudentSubject2 = "Chemistry";
                    examtable.StudentSubject3 = "Mathematics";
                }
                if (comboExamTypeValue == "SetC")
                {
                    examtable.StudentSubject1 = "Literature";
                    examtable.StudentSubject2 = "C.R.K";
                    examtable.StudentSubject3 = "Government";
                }
                if (comboExamTypeValue == "SetD")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Government";
                    examtable.StudentSubject3 = "Literature";
                }
                if (comboExamTypeValue == "SetE")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Accounting";
                }
                if (comboExamTypeValue == "SetF")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Commerce";
                }
                if (comboExamTypeValue == "SetG")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Government";
                }
                if (comboExamTypeValue == "SetH")
                {
                    examtable.StudentSubject1 = "Economics";
                    examtable.StudentSubject2 = "Mathematics";
                    examtable.StudentSubject3 = "Physics";
                }
                #endregion
            }
            ObjEmp.Tables.Add(table); ObjEmp.StudentExamTables.Add(examtable); ObjEmp.SaveChanges();
            //DoGridThings();//This Block of code does the updating
        }
        #endregion
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        #region this code block opens a dialog window to choose and insert image into the Photo tool
        string MyPhoto;
        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            dlg.Title = "Choose Image File";
            dlg.DefaultExt = ".JPG";
            dlg.Filter = "All Supported Graphics|*.jpg;*.jpeg;*.png" + "JPEG|*.jpg;*.jpeg" + "Portable Network Graphics(*.png)|*.png";
            if (dlg.ShowDialog() == true)
            {
                Photo.Opacity = 100;
                MyPhoto = dlg.FileName;
                Photo.Source = new BitmapImage(new Uri(MyPhoto));
            }
        }
        #endregion

        private void tabRoot_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Down)
            {
                e.Handled = true;
            }
        }

        private void btnEmpyDataBase_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("ALL INFORMATION IN THE DATABASE WILL BE LOST", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                var entityObjects = ObjEmp.Tables; var examtables = ObjEmp.StudentExamTables;
                foreach (var entityobject in entityObjects)
                {
                    ObjEmp.Tables.Attach(entityobject);
                    ObjEmp.Tables.Remove(entityobject);
                }
                foreach (var examtable in examtables)
                {
                    ObjEmp.StudentExamTables.Attach(examtable);
                    ObjEmp.StudentExamTables.Remove(examtable);
                }
                ObjEmp.SaveChanges();
                statusBar.Text = "Successful Operation";
            }
            DoGridThings();
            LoadInCombo();
        }
        int nameCounter = 0;
        public void LoadStudentBio()
        {
            try
            {
                var contexts = ObjEmp.Tables;
                foreach (var context in contexts)
                {
                    if (context.Name == txtAdminSearch.Text || context.Surname == txtAdminSearch.Text || context.ExamType == txtAdminSearch.Text)
                    {
                        byte[] binaryPhoto = (byte[])context.Passport;
                        Stream stream = new MemoryStream(binaryPhoto);
                        BitmapImage studentPortrait = new BitmapImage();
                        studentPortrait.BeginInit();
                        studentPortrait.StreamSource = stream;
                        studentPortrait.EndInit();
                        //imgInspectImage.Source = merchantLogo;
                        AdminImage.Opacity = 100;
                        AdminImage.Source = studentPortrait;
                        lblAdminName.Content = context.Name;
                        lblAdminSurname.Content = context.Surname;
                        lblAdminExamID.Content = context.ExamID;
                        lblAdminExamType.Content = context.ExamType;
                    }
                }
            }
            catch { }
        }
        private void txtAdminSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var contexts = ObjEmp.Tables;
                foreach (var context in contexts)
                {
                    if (context.Surname.StartsWith(txtAdminSearch.Text))
                    {
                        nameCounter = nameCounter + 1;
                        if (nameCounter > 3)
                        {
                            ltbAdminSearch.Items.Clear();
                            nameCounter = 0;
                        }
                        ltbAdminSearch.Items.Add(context.Surname);
                    }
                    if (context.Name.StartsWith(txtAdminSearch.Text))
                    {
                        nameCounter = nameCounter + 1;
                        if (nameCounter > 3)
                        {
                            ltbAdminSearch.Items.Clear();
                            nameCounter = 0;
                        }
                        ltbAdminSearch.Items.Add(context.Name);
                    }
                    if (txtAdminSearch.Text == "")
                    {
                        ltbAdminSearch.Items.Clear();
                    }
                }
            }
            catch
            {
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadStudentBio();
        }

        private void ltbAdminSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtAdminSearch.Text = ltbAdminSearch.SelectedValue.ToString();
            LoadStudentBio();
        }

        private void btnAccessStudent_Click(object sender, RoutedEventArgs e)
        {
            var tables = ObjEmp.Tables;
            foreach (var table in tables)
            {
                if ((table.Surname == passSurname.Password) && (table.ExamID == passExamID.Password))
                {
                    tabRoot.SelectedIndex = 3;
                    lblStudentLogin.Content = passSurname.Password + "  " + table.Name.ToString();
                    passSurname.Password = "";
                    passExamID.Password = "";
                }
                else
                { }
            }
        }

        private void btnApplyChanges_Click(object sender, RoutedEventArgs e)
        {
            var contexts = ObjEmp.AdminTables;
            foreach (var context in contexts)
            {
                if (context.AdminPassword == passOldManagement.Password)
                {
                    context.AdminPassword = passNewManagement.Password;
                    lblPassChanged.Content = "Password Changed";
                    lblPassChanged.Opacity = 100;
                    passNewManagement.Password = "";
                    passOldManagement.Password = "";
                }
                else
                {
                    lblPassChanged.Content = "Wrong Password";
                    lblPassChanged.Opacity = 100;
                    passNewManagement.Password = "";
                    passOldManagement.Password = "";
                }
            } ObjEmp.SaveChanges();
        }

        private void Image_PreviewMouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            lblTiming.Opacity = 0;
            tbControlOptions.Opacity = 0;
            tabRoot.SelectedIndex = 0;
            lblStudentLogin.Content = "Test Engine Idle";
        }

        private void Label_PreviewMouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            tabRoot.SelectedIndex = 2;
        }

        private void Label_PreviewMouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            tabRoot.SelectedIndex = 2;
        }

        private void btnStartTest_Click(object sender, RoutedEventArgs e)
        {
            tbControlOptions.Opacity = 100;
            lblTiming.Opacity = 100;
            timer.Start();
        }

        private void cbSelectModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnGlobalSettings_Click(object sender, RoutedEventArgs e)
        {
            tabRoot.SelectedIndex = 4;
        }
        #region This region does the printing of student's information
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                //get the selected question printer capcbilities
                System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);
                //get scale of the print wrt to the screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / gridResult.ActualWidth,
                    capabilities.PageImageableArea.ExtentHeight / gridResult.ActualHeight);
                //Transform the visual to scal
                gridResult.LayoutTransform = new ScaleTransform(scale, scale);
                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                //update the layer of the visual to the printer page size
                gridResult.Measure(sz);
                gridResult.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));
                //now point the visual to printer to fit on the one page
                printDlg.PrintVisual(gridResult, "Student Results");
            }
        }
        #endregion

        private void Button_Click_2(object sender, RoutedEventArgs e)
        { tabRoot.SelectedIndex = 1; }

        private void comboExamType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnFindStudentResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var contexts = ObjEmp.StudentExamTables;
                var studs = ObjEmp.Tables;
                foreach (var context in contexts)
                {
                    if ((context.StudentName == txtStudentResultName.Text) && (context.StudentSurname == txtStudentResultSurname.Text))
                    {
                        lblResultName.Content = context.StudentName;
                        lblResultSurname.Content = context.StudentSurname;
                        lblExamResult.Content = context.StudentSet;
                        lblSubject1.Content = context.StudentSubject1;
                        lblSubject2.Content = context.StudentSubject2;
                        lblSubject3.Content = context.StudentSubject3;
                        lblScore1.Content = context.StudentScore1;
                        lblScore2.Content = context.StudentScore2;
                        lblScore3.Content = context.StudentScore3;
                        lblTotalScore.Content = context.StudentTotalScore;
                        foreach (var stud in studs)
                        {
                            if (stud.Surname == txtStudentResultSurname.Text && stud.Name == txtStudentResultName.Text)
                            {
                                //imgShowStudentPhoto.Source = new Uri
                                byte[] binaryPhoto = (byte[])stud.Passport;
                                Stream stream = new MemoryStream(binaryPhoto);
                                BitmapImage studentPortrait = new BitmapImage();
                                studentPortrait.BeginInit();
                                studentPortrait.StreamSource = stream;
                                studentPortrait.EndInit();
                                //imgInspectImage.Source = merchantLogo;
                                //AdminImage.Opacity = 100;
                                imgShowStudentPhoto.Source = studentPortrait;
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("ERROR OCCURRED IN THE OPERATION", "Information");
            }
        }
        private void lstObjectives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            for (int i = 0; i <= 23; i++)
            {
                if (lstObjectives.SelectedValue.ToString() == objectives[i])
                {
                    pdfdisplay.Navigate(new System.Uri("c:/VG_General_Objectives##/" + objectives[i] + ".pdf"));
                }
            }
        }

        private void lstFaculties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i <= 8; i++)
            {
                if (lstFaculties.SelectedValue.ToString() == faculties[i])
                {
                    pdfdisplay.Navigate(new System.Uri("c:/VG_General_Objectives##/Faculties/" + faculties[i] + ".pdf"));
                }
            }

        }
    }
}
