using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class LoginPage : Form
    {

        public String currentUser;
        public int currentTasks;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {

            if (nameBox.TextLength < 1)
                MessageBox.Show("Please enter a name!");
            else
            {
                //create files and path
                currentUser = nameBox.Text;
                string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/{currentUser}.txt";
                Directory.CreateDirectory($"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/");
                

                //if the path doesn't exist we must create it
                if(!File.Exists(path))
                {
                    NewUser user = new NewUser();
                    user.ShowDialog();

                    if (user.DialogResult == DialogResult.OK)
                    {
                        using (FileStream text = new FileStream(path, FileMode.Create))
                        {
                            //create necessary files and directories then close
                            text.Close();

                            //add some data into out file

                            StreamWriter writer = new StreamWriter(path);

                            // add necessary data to user file, name, date tasks

                            writer.WriteLine(currentUser);
                            writer.WriteLine(DateTime.Now);
                            // we are always gonna start with 0 but this number will change, I hope.
                            currentTasks = 0;
                            writer.WriteLine(currentTasks);

                            writer.Close();
                            loggedIn(currentUser, currentTasks);


                        }


                    }
                }
                else
                {
                    
                    StreamReader reader = new StreamReader(path);
                    reader.ReadLine();//skip first line
                    reader.ReadLine();//skip second line
                    Int32.TryParse(reader.ReadLine(), out currentTasks);
                    reader.Close();
                    loggedIn(currentUser, currentTasks);


                    //we dont need to create user files if they already exist so lets just go right in
                }





            }
        }

        private void loggedIn(string user, int tasks)
        {
            loginButton.Text = "Successfully logged in!";
            this.Hide();
            TaskPage newPage = new TaskPage(currentUser, tasks);

            newPage.ShowDialog();
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }
    }
}
