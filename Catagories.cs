using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class Catagories : Form
    {
        private string CurrentUser;
        private int userTasks;
        
        private List<String> catagories = new List<string>();
        private List<Color> colors = new List<Color>();

        public Catagories()
        {
            InitializeComponent();
        }
        public Catagories(string user, int tasks)
        {
            InitializeComponent();
            CurrentUser = user;
            userTasks = tasks;
        }

        private void Catagories_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;


        }

        private void catagoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //catagoryBox.Anchor;
            catagorytextbox.Text = catagoryBox.SelectedItem.ToString();
            int item = catagories.IndexOf(catagorytextbox.Text);
            colorButton.BackColor = colors[item];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            TaskPage page = new TaskPage(CurrentUser, userTasks, catagories, colors);
            page.Show();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if(catagorytextbox.Text.Length > 0)
            {
                catagoryBox.Items.Add(catagorytextbox.Text);

                catagories.Add(catagorytextbox.Text);
                colors.Add(colorButton.BackColor);
                catagorytextbox.Clear();
                colorButton.BackColor = Color.White;
                MessageBox.Show("Catagory added successfully!");

            }
        }
                

        private void colorButton_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if  (color.ShowDialog() == DialogResult.OK)
            {
                colorButton.BackColor = color.Color;
                
            }    
        }
    }
}
