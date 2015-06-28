using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using log4net;
using System.Threading;

using GroupSimilarityIndexLogicUnit;

namespace GSI
{
    public partial class MainForm : Form
    {

        #region Private Members

        private static ILog Logger = LogManager.GetLogger("GSI");
        private string _filePath;
        private string _fileName;
        private string _fullFileName;
        private string headerLine;
        private string typeLine;
        private List<List<string>> lines;
        private DataTable ds;
        private DataTable testDataTable;
        private DataTable rolloutDataTable;
        private DataTable pagedDS;
        private int pageIndex = 0;
        private int pageSize = 40;
        private GroupSimilarityIndexLogicUnit.GSI _gsi;

        #endregion

        public MainForm()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("*****   Start Application *********");
            InitializeComponent();
        }

        /// <summary>
        /// Select training file to open.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runButton.Enabled = false;

            // Get training file.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _fullFileName = openFileDialog1.FileName;
            }

            if (ValidateSet())
            {
                BindGrid();
                runButton.Enabled = true;
            }

            object[] values = headerLine.Split(',');
            idColumnComboBox.Items.AddRange(values);
            idColumnComboBox.SelectedIndex = 0;
            resultColumnComboBox.Items.AddRange(values);
            resultColumnComboBox.SelectedIndex = values.Length - 1;
        }

        private void BindGrid()
        {
            try
            {
                string[] lines = File.ReadAllLines(_fullFileName);

                if (lines != null && lines.Length > 1)
                {
                    ds = new DataTable();

                    pagedDS = new DataTable();
                    testDataTable = new DataTable();
                    rolloutDataTable = new DataTable();

                    headerLine = lines[0];
                    HandleHeader(headerLine);
                    typeLine = lines[1];
                    HandleLines(lines, 2);
                }
                else
                {
                    MessageBox.Show("File does not contain data." + _fullFileName);
                }
            }
            catch (IOException ioEx)
            {
                Logger.Error(ioEx);
                MessageBox.Show("Failed to read file. Message - " + ioEx.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("Unexpected error.");
            }
        }

        private void HandleLines(string[] fileLines, int startIndex, bool isValidationSet = false,
            bool isRolloutSet = false)
        {
            if (startIndex > fileLines.Length || startIndex < 0)
            {
                MessageBox.Show("File does not contain data." + _fullFileName);
                return;
            }

            lines = new List<List<string>>();

            ds.Rows.Clear();

            if (isValidationSet)
            {
                testDataTable.Rows.Clear();
            }
            if (isRolloutSet)
            {
                rolloutDataTable.Rows.Clear();
            }
            if (!isRolloutSet && !isValidationSet)
            {
                ds.Rows.Clear();
            }

            int counter = -1;
            foreach (string line in fileLines)
            {
                counter++;
                if (counter < startIndex)
                {
                    continue;
                }
                List<string> lineValues = line.Split(',').ToList<string>();

                if (isValidationSet)
                {
                    testDataTable.Rows.Add(line.Split(','));
                    continue;
                }
                else if (isRolloutSet)
                {
                    rolloutDataTable.Rows.Add(line.Split(','));
                    continue;
                }

                lines.Add(lineValues);
                ds.Rows.Add(line.Split(','));

            }

            if (!isValidationSet && !isRolloutSet)
            {
                pageIndex = 0;
                SetPagedGrid();
            }
        }

        private void HandleHeader(string headerLine)
        {
            string[] columnsNames = headerLine.Split(',');

            foreach (string column in columnsNames)
            {
                ds.Columns.Add(column);
                pagedDS.Columns.Add(column);
                testDataTable.Columns.Add(column);
                rolloutDataTable.Columns.Add(column);
            }
        }

        private bool ValidateSet()
        {
            if (string.IsNullOrEmpty(_fullFileName))
            {
                MessageBox.Show("Training set was not selected.");
                return false;
            }

            string[] fileParts = _fullFileName.Split('.');
            if (fileParts.Length < 1)
            {
                MessageBox.Show("Training set file is invalid. " + _fullFileName);
                return false;
            }

            if (fileParts[fileParts.Length - 1] != "csv")
            {
                MessageBox.Show("Training set file is type is not - csv. " + fileParts[fileParts.Length - 1]);
                return false;
            }

            int index = fileParts[fileParts.Length - 2].LastIndexOf('\\');

            if (index == -1)
            {
                MessageBox.Show("Training set file path is invalid " + fileParts[fileParts.Length - 2]);
                return false;
            }

            _filePath = fileParts[fileParts.Length - 2].Substring(0, index);

            _fileName = fileParts[fileParts.Length - 2].Substring(index + 1);

            _fileName += "." + fileParts[fileParts.Length - 1];

            return true;
        }

        /// <summary>
        /// Exit program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Info("*****   End Application *********");
            this.Close();
        }

        private void pagingStartButton_Click(object sender, EventArgs e)
        {
            pageIndex = 0;
            SetPagedGrid();
        }

        private void SetPagedGrid()
        {
            if (ds != null)
            {
                pagedDS.Clear();

                for (int i = pageIndex * pageSize; i < (pageIndex + 1) * pageSize; i++)
                {
                    if (ds.Rows.Count > i)
                    {
                        pagedDS.Rows.Add(ds.Rows[i].ItemArray);
                    }
                }

                trainingSetGrid.DataSource = pagedDS;
                pagingTextBox.Text = ((pageIndex * pageSize) + 1) + "/" + ds.Rows.Count;
            }
        }

        private void pagingPreviousButton_Click(object sender, EventArgs e)
        {
            if (pageIndex > 0)
            {
                pageIndex--;
            }

            SetPagedGrid();
        }

        private void pagingNextButton_Click(object sender, EventArgs e)
        {
            if ((pageIndex + 1) * pageSize < ds.Rows.Count)
            {
                pageIndex++;
            }
            SetPagedGrid();
        }

        private void pagingEndButton_Click(object sender, EventArgs e)
        {
            int rowCount = ds.Rows.Count;

            pageIndex = rowCount / pageSize;

            if (rowCount % pageSize == 0)
            {
                pageIndex--;
            }

            SetPagedGrid();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            if (idColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select ID column.");
                return;
            }

            if (resultColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select result column.");
                return;
            }

            runButton.Enabled = false;
            string confMatrixStr = null;
            string idColumnName = idColumnComboBox.SelectedItem.ToString();
            string resultColumnName = resultColumnComboBox.SelectedItem.ToString();
            string rangeValue = rangeTextBox.Text;
            int range = 5;

            if (!int.TryParse(rangeValue, out range))
            {
                range = 5;
            }
            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;
                _gsi = new GroupSimilarityIndexLogicUnit.GSI(ds,
                    headerLine.Split(',').ToList(),
                    typeLine.Split(',').ToList(),
                    idColumnName,
                    resultColumnName,
                    range
                    );

                Dictionary<string, int> result = _gsi.Run();
                if (result != null)
                {
                    MessageBox.Show("Operation finished!");
                    confMatrixStr = CreateConfsionMatrixLabel(result, "Training Set");
                }
                else
                {
                    MessageBox.Show("Operation failed!");
                }
            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate(object o, ProgressChangedEventArgs args)
            {
                progressLabel.Text = string.Format("{0}% Completed", args.ProgressPercentage);
            });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                progressLabel.Text = "Finished!";
                trainingConfusionMatrixLabel.Text = confMatrixStr;
            });

            bw.RunWorkerAsync();

            runButton.Enabled = true;

        }

        private string CreateConfsionMatrixLabel(Dictionary<string, int> result, string setName)
        {
            StringBuilder sb = new StringBuilder();
            int index = setName.Length;
            sb.Append("Total size - " + (result["Not Actual Forecasted"] + result["Actual Forecasted"] + result["Actual Not Forecasted"]
                + result["Not Actual Not Forecasted"]) + Environment.NewLine);
            sb.Append(setName);
            for (int i = index; i < 18; i++)
            {
                sb.Append(' ');
            }
            sb.Append("|  Actual     | Not Actual" + Environment.NewLine + "Forecasted        |  " + result["Actual Forecasted"].ToString());

            index = result["Actual Forecasted"].ToString().Length;
            for (int i = index; i < 13; i++)
            {
                sb.Append(' ');
            }
            sb.Append("| " + result["Not Actual Forecasted"].ToString());
            index = result["Not Actual Forecasted"].ToString().Length;
            for (int i = index; i < 13; i++)
            {
                sb.Append(' ');
            }
            sb.Append(Environment.NewLine + "Not Forecasted |  " + result["Actual Not Forecasted"].ToString());
            index = result["Actual Not Forecasted"].ToString().Length;
            for (int i = index; i < 13; i++)
            {
                sb.Append(' ');
            }
            sb.Append("| " + result["Not Actual Not Forecasted"]);
            return sb.ToString();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string aboutStr = 
                "This program was created by Moshe Fishelevich for Data Mining and Knowledge Discovery Course in Tel Aviv University, MBA program." + Environment.NewLine +
                "This program can not be used for any purpose without the explicit permission of the author." + Environment.NewLine+
                "All Rights Reserved.";
            MessageBox.Show(aboutStr);
        }

        private void testSetButton_Click(object sender, EventArgs e)
        {
            if (idColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select ID column.");
                return;
            }

            if (resultColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select result column.");
                return;
            }

            testSetButton.Enabled = false;
            string confMatrixStr = null;
            string idColumnName = idColumnComboBox.SelectedItem.ToString();
            string resultColumnName = resultColumnComboBox.SelectedItem.ToString();

            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;

                Dictionary<string, int> result = _gsi.RunTest(testDataTable);
                if (result != null)
                {
                    confMatrixStr = CreateConfsionMatrixLabel(result, "Validation Set");
                    MessageBox.Show("Operation finished!");
                }
                else
                {
                    MessageBox.Show("Operation failed!");
                }
            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate(object o, ProgressChangedEventArgs args)
            {
                progressLabel.Text = string.Format("{0}% Test Completed", args.ProgressPercentage);
            });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                validationConfisionMatrixLabel.Text = confMatrixStr;
                progressLabel.Text = "Test Finished!";
            });

            bw.RunWorkerAsync();

            testSetButton.Enabled = true;
        }

        private void selectValidationSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testSetButton.Visible = true;
            testSetButton.Enabled = false;

            // Get test file.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _fullFileName = openFileDialog1.FileName;
            }

            if (ValidateSet())
            {
                PrepairValidationSet();
                testSetButton.Enabled = true;
            }
        }

        private void PrepairValidationSet()
        {
            try
            {
                string[] lines = File.ReadAllLines(_fullFileName);

                if (lines != null && lines.Length > 1)
                {
                    HandleLines(lines, 2, true);
                }
                else
                {
                    MessageBox.Show("File does not contain data." + _fullFileName);
                }
            }
            catch (IOException ioEx)
            {
                Logger.Error(ioEx);
                MessageBox.Show("Failed to read file. Message - " + ioEx.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("Unexpected error.");
            }
        }

        private void PrepairRolloutSet()
        {
            try
            {
                string[] lines = File.ReadAllLines(_fullFileName);

                if (lines != null && lines.Length > 1)
                {
                    HandleLines(lines, 2, false, true);
                }
                else
                {
                    MessageBox.Show("File does not contain data." + _fullFileName);
                }
            }
            catch (IOException ioEx)
            {
                Logger.Error(ioEx);
                MessageBox.Show("Failed to read file. Message - " + ioEx.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("Unexpected error.");
            }
        }

        private void resetTrainingSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testSetButton.Visible = false;
            testSetButton.Enabled = false;
            runButton.Enabled = false;
            validationConfisionMatrixLabel.Text = "Not Evaluated Yet";
            trainingConfusionMatrixLabel.Text = "Not Evaluated Yet";
            trainingSetGrid.ClearSelection();
        }

        private void selectRolloutFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rolloutButton.Visible = true;
            rolloutButton.Enabled = false;

            // Get test file.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _fullFileName = openFileDialog1.FileName;
            }

            if (ValidateSet())
            {
                PrepairRolloutSet();
                rolloutButton.Enabled = true;
            }
        }

        private void rolloutButton_Click(object sender, EventArgs e)
        {
            if (idColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select ID column.");
                return;
            }

            if (resultColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select result column.");
                return;
            }

            rolloutButton.Enabled = false;
            string output = null;
            string idColumnName = idColumnComboBox.SelectedItem.ToString();
            string resultColumnName = resultColumnComboBox.SelectedItem.ToString();

            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;

                 output = _gsi.RunRollout(rolloutDataTable);
                if (!string.IsNullOrEmpty(output))
                {
                    MessageBox.Show("Operation finished!");
                }
                else
                {
                    MessageBox.Show("Operation failed!");
                }
            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate(object o, ProgressChangedEventArgs args)
            {
                progressLabel.Text = string.Format("{0}% Test Completed", args.ProgressPercentage);
            });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                SaveOutputFile(output);
                progressLabel.Text = "Test Finished!";
            });

            bw.RunWorkerAsync();

            testSetButton.Enabled = true;
        }

        private void SaveOutputFile(string output)
        {
            saveFileDialog1.FileName = "recommendations.txt";

            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                {
                    sw.WriteLine(output);
                }
            };
        }
    }
}
