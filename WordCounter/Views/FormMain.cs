using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace WordCounter.Views
{
    public partial class FormMain : Form
    {
        private Form activeForm = null;        

        // Form main constructor
        public FormMain()
        {
            InitializeComponent();            
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Set version of the software
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            // Get version
            string version = fileVersionInfo.ProductVersion;
            version = version.Substring(0, 3);
            lblVersion.Text = "Ver. " + version;

            // Load word count form as child form on initial page load
            OpenChildForm(new Views.FormWordCount());
        }

     #region Child form handle
        private void OpenChildForm(Form childForm)
        {
            // Set child form and set as active form
            if (activeForm != null) activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;

            // remove form boders
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Add child form to the panel
            pnlChildForm.Controls.Add(childForm);
            pnlChildForm.Tag = childForm;
            childForm.BringToFront();

            // show the child form
            childForm.Show();
        }
     #endregion

     #region button click events

        private void btnCountWord_Click(object sender, EventArgs e)
        {
            // Open word count form
            OpenChildForm(new Views.FormWordCount());
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            // Open about form
            OpenChildForm(new Views.FormAbout());
        }

     #endregion

    }
}
