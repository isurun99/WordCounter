using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace WordCounter.Views
{
    public partial class FormWordCount : Form
    {
        // Variable declaration
        private OpenFileDialog ofdTextFile = null;
        Dictionary<string, int> wordsDictionary = new Dictionary<string, int>();

        // Word cont form constructor
        public FormWordCount()
        {
            InitializeComponent();
            InitialiseOpenFileDialog();

            // Load data grid columns
            LoadResultTable();

            pnlProcess.Visible = false;
        }

        #region Button Events
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Call browse function
            BrowseFile();
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            if (ofdTextFile.FileName.Length > 0)
            {
                // check if the background worker is already busy with the operation
                if (backgroundWorker.IsBusy == false)
                {
                    // Show panel process
                    pnlProcess.Visible = true;

                    // Start the execution asynchronously in the background
                    backgroundWorker.RunWorkerAsync();

                }
            }
            else 
            {
                MessageBox.Show(this, "Please select text file first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy == true)
            {
                // Cancel the background operation if still in progress.
                backgroundWorker.CancelAsync();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        #endregion

        #region Browse file
        private void InitialiseOpenFileDialog()
        {
            try
            {
                // Initialize open file dialog
                ofdTextFile = new OpenFileDialog()
                {
                    // Set open file dialog properties
                    InitialDirectory = @"C:\",
                    Title = "Browse Text Files",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    // Set allowed file types
                    DefaultExt = "txt",
                    Filter = "txt files (*.txt)|*.txt",
                    FilterIndex = 2,
                    RestoreDirectory = true,

                    // Set read only property
                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Browse the file
        private void BrowseFile()
        {
            try
            {
                // Check dialog result 
                if (ofdTextFile.ShowDialog() == DialogResult.OK)
                {
                    // IF result is OK, then set the file name to the text box
                    txtFileName.Text = System.IO.Path.GetFileName(ofdTextFile.FileName);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Result table
        //Load Data Grid Design
        public void LoadResultTable()
        {
            try
            {
                // Clear existing columns
                dgvResult.Columns.Clear();

                // Set column count
                dgvResult.ColumnCount = 2;

                // Define new columns 
                dgvResult.Columns[0].Name = "Word";
                dgvResult.Columns[1].Name = "Occurrence";

                // Define the column sizes
                dgvResult.Columns[0].Width = 360;
                dgvResult.Columns[1].Width = 153;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Populate word result table
        private void PopulateResultTable(Dictionary<string, int> processedDictionary)
        {
            try
            {
                int totalWords = 0;
                List<DataGridViewRow> Results = new List<DataGridViewRow>();

                // Clear existing rows in the data grid
                dgvResult.Rows.Clear();

                // Iterate all words
                foreach (KeyValuePair<string, int> item in processedDictionary)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    // Create data grid rows
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Key });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Value });

                    // Add data grid rows to the list
                    Results.Add(row);

                    // Calculate total words
                    totalWords += item.Value;
                }

                // Add all data grid rows at once
                // AddRange() method is much faster than AddRows() method
                dgvResult.Rows.AddRange(Results.ToArray());

                // Set Word conts for the display lables
                lblTotalWordCount.Text = totalWords.ToString();
                lblUniqueWordCount.Text = processedDictionary.Count.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Background worker
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Set background process
                backgroundWorker.ReportProgress(10);

                /* Current thread is put into sleep for 2seconds.
                 because text file is too small and for the demonstration purposes*/
                Thread.Sleep(2000);

                // Read text file
                WordCounterFacade.FileProcess objFile = new WordCounterFacade.FileProcess();
                string fileText = objFile.ReadTextFile(ofdTextFile.FileName);

                /* Check cancell process is pending in each step*/
                if (backgroundWorker.CancellationPending)
                {
                    // Cancel the process
                    e.Cancel = true;
                    backgroundWorker.ReportProgress(0);
                    return;
                }

                // Set background process
                backgroundWorker.ReportProgress(40);

                /* Current thread is put into sleep for 1 second.
                 because text file is too small and for the demonstration purposes*/
                Thread.Sleep(1000);

                // Start the word count
                WordCounterFacade.WordProcess objWord = new WordCounterFacade.WordProcess(fileText);
                wordsDictionary = objWord.CountWords();

                /* Check cancell process is pending in each step*/
                if (backgroundWorker.CancellationPending)
                {
                    // Cancel the process
                    e.Cancel = true;
                    backgroundWorker.ReportProgress(0);
                    return;
                }

                // Set background process
                backgroundWorker.ReportProgress(80);

                /* Current thread is put into sleep for 1 second.
                 because text file is too small and for the demonstration purposes*/
                Thread.Sleep(1000);

                // Sort the word dictionary
                wordsDictionary = objWord.SortWordDictionary(wordsDictionary);

                /* Check cancell process is pending in each step*/
                if (backgroundWorker.CancellationPending)
                {
                    // Cancel the process
                    e.Cancel = true;
                    backgroundWorker.ReportProgress(0);
                    return;
                }

                // Set background process
                backgroundWorker.ReportProgress(100);

                /* Current thread is put into sleep for for 1 second.
                 because text file is too small and for the demonstration purposes*/
                Thread.Sleep(1000);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Set progress bar value
            progressBar.Value = e.ProgressPercentage;

            // Set progress label value
            lblProgress.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Check form is disposed
                if (!this.IsDisposed)
                {
                    // Check process has cancelled
                    if (e.Cancelled)
                    {
                        // Show message 
                        MessageBox.Show(this, "Process cancelled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    // Check process has errors
                    else if (e.Error != null)
                    {
                        // Show message 
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Process has successfully completed
                    else
                    {
                        // Call populate results function
                        PopulateResultTable(wordsDictionary);
                    }

                    // hide the process panel
                    pnlProcess.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        #endregion

    }
}
