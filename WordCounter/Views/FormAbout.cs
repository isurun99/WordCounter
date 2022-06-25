using System;
using System.Windows.Forms;

namespace WordCounter.Views
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            // Call about text populate function
            SetAboutText();
        }
    
        private void SetAboutText()
        {
            // Set text for labels
            lblText1.Text = "Word Counter is a tool that will tell you how many words are in a text file. It's \ndesigned to help you determine the occurrence of words in any text file.";
            lblText2.Text = "Input a text file into the program, and the 'Count' button will tell you how many \nwords are in that file. The application will count the words in the text file and \nreturn the total to you immediately.";
            lblText3.Text = "Contact isuruperez@gmail.com for more details.";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }
     
    }
}
