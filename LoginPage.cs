using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
           



            if (nameBox.TextLength < 1 || passwordBox.TextLength < 1)
                MessageBox.Show("Please enter a name and password!");
            else
            {
                //create files and path
                currentUser = nameBox.Text;
                string path = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/{currentUser}.txt";
                string passwordPath = $"C:/Users/1259809181117005/source/repos/TaskManager/bin/Debug/users/{currentUser}/{currentUser}pw.txt";

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
                            

                        }
                        using (FileStream text = new FileStream(passwordPath, FileMode.Create))
                        {
                            
                            text.Close();

                            byte[] salt;
                            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                            var pbkdf2 = new Rfc2898DeriveBytes(passwordBox.Text, salt, 1000);
                            byte[] hash = pbkdf2.GetBytes(20);

                            byte[] hashBytes = new byte[36];
                            Array.Copy(salt, 0, hashBytes, 0, 16);
                            Array.Copy(hash, 0, hashBytes, 16, 20);
                            StreamWriter writer = new StreamWriter(passwordPath);

                            // add necessary data to user file, name, date tasks
                            
                            writer.WriteLine(Convert.ToBase64String(hashBytes));

                            writer.Close();

                            loggedIn(currentUser, currentTasks);

                        }


                    }
                }
                else
                {

                    //check user
                    //check password


                    StreamReader reader = new StreamReader(path);
                    StreamReader password = new StreamReader(passwordPath);

                    if (nameBox.Text == reader.ReadLine())
                    {
                        byte[] salt;
                        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                        string savedPw = password.ReadLine();
                        byte[] hashBytes = Convert.FromBase64String(savedPw);

                        /* Get the salt */
                        salt = new byte[16];
                        Array.Copy(hashBytes, 0, salt, 0, 16);

                        /* Compute the hash on the password the user entered */
                        var pbkdf2 = new Rfc2898DeriveBytes(passwordBox.Text, salt, 1000);
                        byte[] hash = pbkdf2.GetBytes(20);

                        /* Compare the results */

                        for (int i = 0; i < 20; i++)
                        {
                            if (hashBytes[i + 16] != hash[i])
                            {
                                MessageBox.Show("Incorrect login information");
                                break;
                            }
                            else
                            {

                                reader.ReadLine();//skip second line
                                Int32.TryParse(reader.ReadLine(), out currentTasks);
                                reader.Close();
                                loggedIn(currentUser, currentTasks);

                            }
                        }
                            
                    }
                      

                    //if (nameBox.Text == reader.ReadLine() && passwordBox.Text == password.ReadLine())
                    //{
                        
                    //}
                    //else
                    //    MessageBox.Show("Incorrect login");
                    //reader.ReadLine();//skip first line
                    


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
