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

namespace TaskManager
{
    public partial class TaskView : Form
    {

        /*
         * features to come?
         * *task catagories
         * *show tasks based on catagories?
         * *allow user to select theme? light/dark
         * *taskbar notifications?           
         */


        private string currentUser;
        private string currentTask;
        private string currentDesc;

        private int thisTask;
        private int userTasks;

        private DateTime currentDate;

        public TaskView()
        {
            InitializeComponent();
        }

        public TaskView(string User, string taskName, string taskDesc, DateTime taskDate, int tasks, int taskid)
        {
            InitializeComponent();
            currentUser = User;
            currentTask = taskName;
            currentDate = taskDate;
            currentDesc = taskDesc;
            userTasks = tasks;
            thisTask = taskid;
        }


        private void TaskView_Load(object sender, EventArgs e)
        {
            this.Text = $"TASK: {currentTask} || DUE BY: {currentDate}";
            taskNameBox.Text = currentTask;
            descBox.Text = currentDesc;
            datePicker.Value = currentDate;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveButton.Visible == true)
                MessageBox.Show("Are you sure you want to exit? You are still editing!");
            else
            {
                TaskPage page = new TaskPage(currentUser, userTasks);
                this.Hide();
                page.Show();

            }
           
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            editButton.Visible = false;

            saveButton.Visible = true;
            taskNameBox.Enabled = true;
            descBox.Enabled = true;
            datePicker.Enabled = true;

           

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (taskNameBox.TextLength < 1)
            {
                MessageBox.Show("Please enter a task!");

            }
            else if (taskNameBox.Text.Contains('/') || taskNameBox.Text.Contains('\\'))
                MessageBox.Show("A task cannot contain '/' or '\\'. Please change this character");
            else
            {
                editButton.Visible = true;

                saveButton.Visible = false;
                taskNameBox.Enabled = false;
                descBox.Enabled = false;
                datePicker.Enabled = false;


                string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/task{thisTask}.txt";
                StreamWriter task = new StreamWriter(path);
                task.WriteLine(currentUser);
                task.WriteLine(taskNameBox.Text);
                task.WriteLine(descBox.Text);
                task.WriteLine(datePicker.Value);
                task.Close();
            }    
           


        }

        private void taskNameBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
