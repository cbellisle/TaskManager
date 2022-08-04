using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

        /*
        * features to come?
        * *task catagories
        * *show tasks based on catagories?
        * *allow user to select theme? light/dark
        * *taskbar notifications?           
        */
        
namespace TaskManager
{
    public partial class TaskPage : Form
    {
        private string currentUser;
        private int userTasks;
        

        public static int maxLabels = 10;
        //identifiers
        private List<String> taskName = new List<string>();
        private List<DateTime> taskDate = new List<DateTime>();
        private List<String> taskDesc = new List<string>();


        public static Button[] buttons = new Button[maxLabels];
        public static Label[] labels = new Label[maxLabels]; 


        public TaskPage()
        {
            InitializeComponent();
        }
        public TaskPage(string user, int tasks)
        {
            InitializeComponent();
            currentUser = user;
            userTasks = tasks;
            //displayTasks();

        }
        public TaskPage(string user, string task, string taskDescription, DateTime date)
        {
            InitializeComponent();
            userTasks++;
            currentUser = user;
            //displayTasks();


        }

        private void TaskPage_Load(object sender, EventArgs e)
        {
            usernameLabel.Text = $"Hello {currentUser}! You currently have {userTasks} incomplete tasks.";
            dateLabel.Text = $"Today is {DateTime.Now}";
            this.Text = $"{currentUser}'s task manager";
            if (userTasks == 0)
                taskButton.Visible = true;
            else
            {
                displayTasks();
            }
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // when the page loads what do we need?
            //user name
            // number of tasks
            // scan through text docs and load tasks
            // display tasks
            // allow us to detect clicks from specifc tasks
            // so we need to load in all tasks, and store them into some type of array i guess
            //monthCalendar1.date




        }

        private void displayTasks()
        {
            for (int i = 0; i < userTasks; i++)
            {
                try
                {
                    string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/task{i}.txt";

                    StreamReader reader = new StreamReader(path);

                    reader.ReadLine();
                    taskName.Add(reader.ReadLine());
                    taskDesc.Add(reader.ReadLine());
                    taskDate.Add(DateTime.Parse(reader.ReadLine()));
                    reader.Close();


                    buttons[i] = new Button();
                    buttons[i].Text = "Complete";
                    buttons[i].Location = new Point(700, 100 + (30 * i));
                    buttons[i].Visible = true;
                    buttons[i].BringToFront();
                    buttons[i].Size = new Size(100, 30);
                    buttons[i].Click += (sender, e) => completeButton(sender, e, i); ;
                    buttons[i].Font = new Font("Arial", 12);
                    this.Controls.Add(buttons[i]);


                    labels[i] = new Label();

                    if (taskDate[i] < DateTime.Now)
                        labels[i].ForeColor = Color.Red;
                    else
                        labels[i].ForeColor = Color.Green;


                    labels[i].Text = taskName[i] + $"\t\t\t Due; {taskDate[i]}";

                    labels[i].Visible = true;
                    labels[i].Location = new Point(10, 100 + (30 * i));
                    labels[i].BringToFront();
                    labels[i].Font = new Font("Arial", 12);
                    labels[i].Size = new Size(350, 20);
                    labels[i].AutoSize = true;
                    labels[i].Click += (sender, e) => TaskPage_Click(sender, e, i);
                    this.Controls.Add(labels[i]);

                }
                catch
                {
                    Label catchLabel = new Label();
                    catchLabel.Text = "ERROR LOADING TASKS!";
                    catchLabel.Font = new Font("Arial", 36);
                    catchLabel.Location = new Point(10, 130);
                    catchLabel.BringToFront();
                    catchLabel.Visible = true;
                    catchLabel.Size = new Size(1000, 60);
                    this.Controls.Add(catchLabel);
                }


            }


        }

        private void completeButton(object sender, EventArgs e, int button)
        {
            int current = 0;
            for (int i = 0; i < userTasks; i++)
                if (sender == buttons[i])
                    current = i;

            string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/task{current}.txt";
            string archive = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/archive";


            labels[current].Text = taskName[current];


            string newFilePath = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/archive/{labels[current].Text}.txt";

            if (!Directory.Exists(archive))
                Directory.CreateDirectory(archive);


            DialogResult result = MessageBox.Show("Are you sure you would like to mark this task as complete and archive it?", "Mark task as complete",MessageBoxButtons.YesNo);
            switch(result)
            {
                case DialogResult.Yes:
                    
                    try
                    {
                        System.IO.File.Move(path, newFilePath);
                        System.IO.File.Delete(path);


                        updateUser();
                        updateFiles();
                        Invalidate();
                        this.Hide();
                        TaskPage page = new TaskPage(currentUser, userTasks);
                        page.ShowDialog();
                    }
                    catch
                    {
                        MessageBox.Show("File not found");
                    }

                    break;

                case DialogResult.No:
                    break;
            }
        }
        private void updateUser()
        {
            userTasks--;
            string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/{currentUser}.txt";
            StreamWriter write = new StreamWriter(path);
            write.WriteLine(currentUser);
            write.WriteLine($"Last updated {DateTime.Now}");
            write.WriteLine(userTasks);
            write.Close();

        }
        private void updateFiles()
        {
            for (int i = 0; i < userTasks; i++)
            {
                string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/task{i}.txt";
                //string newpath = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/task{i-1}.txt";

                if(!File.Exists(path))
                {
                    for(int j = i; i < userTasks; i++)
                    {
                        string movepath = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/task{j}.txt";
                        string oldpath = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/task{i + 1}.txt";
                        File.Move(oldpath, movepath);

                    }
                }


            }

        }

        private void TaskPage_Click(object sender, EventArgs e, int taskNum)
        {
            // what do we wanna do when we clikc
            //taskNum -= 1;
            for(int i = 0; i < userTasks; i++)
                if(sender == labels[i])
                {
                    TaskView view = new TaskView(currentUser, taskName[i], taskDesc[i], taskDate[i], userTasks, i);
                    this.Hide();
                    view.ShowDialog();
                }
                    
            //throw new NotImplementedException();
        }

        private void taskButton_Click(object sender, EventArgs e)
        {
            taskButton.Visible = false;

            AddEvent add = new AddEvent(currentUser, userTasks);
            this.Hide();
            add.ShowDialog();

            if(add.DialogResult == DialogResult.OK)
            
                userTasks++;

        }
        private void usernameLabel_Click(object sender, EventArgs e)
        {

        }

        private void dateLabel_Click(object sender, EventArgs e)
        {

        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(userTasks < maxLabels)
            {
                taskButton.Visible = false;

                AddEvent add = new AddEvent(currentUser, userTasks);
                add.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You have too many concurrent tasks! Please delete some or finish them to add new ones!");
            }
           
        }

        private void archivesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void TaskPage_ResizeBegin(object sender, EventArgs e)
        {

        }


        private void TaskPage_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Text = $"You currently have {userTasks} incomplete tasks.";
                if(userTasks > 0)
                {
                    notifyIcon.BalloonTipText = $"You currently have {userTasks} incomplete tasks.";
                    notifyIcon.BalloonTipTitle = "Warning, incomplete tasks!";
                    notifyIcon.ShowBalloonTip(1000);
                }

                notifyIcon.Visible = true;
            }
        }
    }

}   
