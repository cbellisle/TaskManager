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
    public partial class AddEvent : Form
    {
        private string User;
        private int numTasks;
        public AddEvent()
        {
            InitializeComponent();
        }

        public AddEvent(string currentUser, int tasks)
        {

            InitializeComponent();
            User = currentUser;
            numTasks = tasks;
        }

        private void AddEvent_Load(object sender, EventArgs e)
        {
            nameLabel.Text = $"What do you have planned for today {User}?";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;


        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            if (nameBox.TextLength < 1)
            {
                MessageBox.Show("Please enter a task!");

            }
            else
            {
                string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{User}/task{numTasks}.txt";

                string Userpath = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{User}/{User}.txt";

                StreamWriter sw = new StreamWriter(Userpath);
                numTasks++;
                sw.WriteLine(User);
                sw.WriteLine($"Last updated {DateTime.Now}");
                sw.WriteLine(numTasks);
                sw.Close();

                StreamWriter task = new StreamWriter(path);
                task.WriteLine(User);
                task.WriteLine(nameBox.Text);
                task.WriteLine(descriptionBox.Text);
                task.WriteLine(dateTimePicker1.Value);
                task.Close();

                this.Hide();
                TaskPage task1 = new TaskPage(User, numTasks);
                task1.Show();
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            TaskPage task1 = new TaskPage(User, numTasks);
            task1.Show();

        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
