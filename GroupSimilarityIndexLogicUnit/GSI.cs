using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using log4net;
using System.Threading.Tasks;

namespace GroupSimilarityIndexLogicUnit
{
    public class GSI
    {
        #region Private Members

        private string _idColumnName;
        private string _resultColumnName;
        private DataTable _originalDataTable;
        private DataTable _originalValidationDataTable;
        private DataTable _originalRolloutDataTable;
        private DataTable _trainingDataTable;
        private DataTable _validationDataTable;
        private DataTable _rolloutDataTable;

        private List<string> _headerLine;
        private List<string> _typeLine;
        private static ILog Logger = LogManager.GetLogger("GSI");
        private GsiAndShdIndexes[][] indexMatrix = null;
        private List<ColumnInformation> _columnsInformationList;

        private DataTable _processedPositive;
        private DataTable _processedNegetive;

        private List<int> _positiveRowsIndexList;
        private List<int> _negetiveRowsIndexList;
        Dictionary<string, double> positiveGSI;
        Dictionary<string, double> negetiveGSI;
        private int _range;

        #endregion

        #region Constructor

        public GSI(DataTable ds, List<string> headerLine, List<string> typeLine, string idColumnName, string resultColumnName, int range)
        {
            _range = range;
            _originalDataTable = ds;
            _headerLine = headerLine;
            _typeLine = typeLine;
            _idColumnName = idColumnName;
            _resultColumnName = resultColumnName;
            _columnsInformationList = new List<ColumnInformation>();
            _negetiveRowsIndexList = new List<int>();
            _positiveRowsIndexList = new List<int>();

        }

        #endregion

        #region Public Methods

        public Dictionary<string, int> Run()
        {
            Dictionary<string, int> result = null;
            try
            {
                // Convert data
                // 1. Split to smaller columns
                // 2. Transform data
                ConvertToBooleanDS();

                // Run PSI and SHD
                //  FindPSIandSHD();
                SplitProcessDataTable();

                // Run GSI
                CalculateGSI();
                result = CreateConfusionMatrix();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }

            return result;
        }

        public Dictionary<string, int> RunTest(DataTable testDataTable)
        {
            Dictionary<string, int> result = null;
            _originalValidationDataTable = testDataTable;

            try
            {
                // Convert data
                // 1. Split to smaller columns
                // 2. Transform data
                ConvertToBooleanValidation();

                // Run PSI and SHD
                //  FindPSIandSHD();
                SplitProcessDataTable(true);

                // Run GSI
                //CalculateGSI();
                result = CreateConfusionMatrix(true);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }

            return result;
        }

        public string RunRollout(DataTable rolloutDataTable)
        {
            string rollout = string.Empty;
            _originalRolloutDataTable = rolloutDataTable;

            try
            {
                // Convert data
                // 1. Split to smaller columns
                // 2. Transform data
                ConvertToBooleanRollout();

                rollout = CreateRolloutOutput();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }

            return rollout;
        }

        private string CreateRolloutOutput()
        {
            string output = string.Empty;

            int go = 0;
            int noGo = 0;
            int rowCounter = -1;
            foreach (DataRow row in _rolloutDataTable.Rows)
            {
                rowCounter++;
                double positiveRank = GradeDataRow(row, positiveGSI);
                double negetiveRank = GradeDataRow(row, negetiveGSI);

                if (positiveRank > negetiveRank)
                {
                    output += "1" + Environment.NewLine;
                    go++;
                }
                else
                {
                    output += "0" + Environment.NewLine;
                    noGo++;
                }
            }

            return output;
        }

        #endregion

        #region PGD Calculations

        private Dictionary<string, int> CreateConfusionMatrix(bool useValidationDataTable = false)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            List<int> ActualAndForecasted = new List<int>();
            List<int> ActualAndNotForecasted = new List<int>();
            List<int> NotActualAndForecasted = new List<int>();
            List<int> NotActualAndNotForecasted = new List<int>();

            DataTable tableToCheck = _trainingDataTable;
            if (useValidationDataTable)
            {
                tableToCheck = _validationDataTable;
            }
            int rowCounter = -1;
            foreach (DataRow row in tableToCheck.Rows)
            {
                rowCounter++;
                double positiveRank = GradeDataRow(row, positiveGSI);
                double negetiveRank = GradeDataRow(row, negetiveGSI);

                if ((positiveRank > negetiveRank) && (_positiveRowsIndexList.Contains(rowCounter)))
                {
                    ActualAndForecasted.Add(rowCounter);
                    if (!result.ContainsKey("Actual Forecasted"))
                    {
                        result.Add("Actual Forecasted", 0);
                    }
                    result["Actual Forecasted"]++;
                    continue;
                }

                if ((positiveRank < negetiveRank) && (_negetiveRowsIndexList.Contains(rowCounter)))
                {
                    NotActualAndNotForecasted.Add(rowCounter);
                    if (!result.ContainsKey("Not Actual Not Forecasted"))
                    {
                        result.Add("Not Actual Not Forecasted", 0);
                    }
                    result["Not Actual Not Forecasted"]++;
                    continue;
                }

                if ((positiveRank > negetiveRank) && (!_positiveRowsIndexList.Contains(rowCounter)))
                {
                    NotActualAndForecasted.Add(rowCounter);
                    if (!result.ContainsKey("Not Actual Forecasted"))
                    {
                        result.Add("Not Actual Forecasted", 0);
                    }
                    result["Not Actual Forecasted"]++;
                    continue;
                }

                if ((positiveRank < negetiveRank) && (!_negetiveRowsIndexList.Contains(rowCounter)))
                {
                    ActualAndNotForecasted.Add(rowCounter);
                    if (!result.ContainsKey("Actual Not Forecasted"))
                    {
                        result.Add("Actual Not Forecasted", 0);
                    }
                    result["Actual Not Forecasted"]++;
                    continue;
                }

                if (positiveRank == negetiveRank)
                {

                }
            }
            return result;
        }

        private double GradeDataRow(DataRow row, Dictionary<string, double> gsiDic)
        {
            double rank = 0;

            foreach (string colName in gsiDic.Keys)
            {
                if (row[colName].ToString().ToLower() == "true")
                {
                    rank += gsiDic[colName];
                }
            }

            return rank;
        }

        private void CalculateGSI()
        {
            positiveGSI = new Dictionary<string, double>();
            negetiveGSI = new Dictionary<string, double>();

            int rowCount = _trainingDataTable.Rows.Count;
            int propertiesCount = _columnsInformationList.Count;
            int newColCounter = 0;
            foreach (ColumnInformation colInfo in _columnsInformationList)
            {
                int subColCounter = 0;
                foreach (string subColName in colInfo.SubColumnsNamesList)
                {
                    int trueCounter = 0;
                    foreach (DataRow row in _trainingDataTable.Rows)
                    {
                        if (row[subColName].ToString().ToLower() == "true")
                        {
                            trueCounter++;
                        }
                    }

                    //     double GSI = (double)trueCounter / (double)(rowCount * propertiesCount);
                    double GSI = (double)trueCounter / (double)(rowCount);
                    colInfo.GSI_Indexes.Add(GSI);

                    trueCounter = 0;
                    foreach (DataRow row in _processedPositive.Rows)
                    {
                        if (row[subColName].ToString().ToLower() == "true")
                        {
                            trueCounter++;
                        }
                    }

                    GSI = (double)((double)trueCounter * (double)colInfo.R_Index) / (double)(_processedPositive.Rows.Count * propertiesCount);
                    positiveGSI.Add(subColName, GSI);


                    trueCounter = 0;
                    foreach (DataRow row in _processedNegetive.Rows)
                    {
                        if (row[subColName].ToString().ToLower() == "true")
                        {
                            trueCounter++;
                        }
                    }

                    GSI = (double)((double)trueCounter * (double)colInfo.R_Index) / (double)(_processedNegetive.Rows.Count * propertiesCount);
                    negetiveGSI.Add(subColName, GSI);

                    subColCounter++;
                    newColCounter++;
                }
            }

            negetiveGSI.OrderBy(x => x.Value);
            positiveGSI.OrderBy(x => x.Value);
        }

        private void SplitProcessDataTable(bool splitValidationDataTable = false)
        {

            _negetiveRowsIndexList.Clear();
            _positiveRowsIndexList.Clear();

            int rowCounter = 0;
            DataTable tableToSplit = _originalDataTable;
            DataTable tableToCopy = _trainingDataTable;

            if (splitValidationDataTable)
            {
                tableToSplit = _originalValidationDataTable;
                tableToCopy = _validationDataTable;
            }

            foreach (DataRow row in tableToSplit.Rows)
            {

                if (row[_resultColumnName].ToString() == "1")
                {
                    DataRow newRow = _processedPositive.NewRow();
                    newRow.ItemArray = tableToCopy.Rows[rowCounter].ItemArray;
                    _processedPositive.Rows.Add(newRow);
                    _positiveRowsIndexList.Add(rowCounter);
                }
                else
                {
                    DataRow newRow = _processedNegetive.NewRow();
                    newRow.ItemArray = tableToCopy.Rows[rowCounter].ItemArray;
                    _processedNegetive.Rows.Add(newRow);
                    _negetiveRowsIndexList.Add(rowCounter);
                }

                rowCounter++;
            }
        }

        #endregion

        #region Calculate Indexes

        private void FindPSIandSHD()
        {
            int rowCount = _trainingDataTable.Rows.Count;
            int numberofAttributes = _headerLine.Count - 2;
            indexMatrix = new GsiAndShdIndexes[rowCount][];

            for (int i = 0; i < rowCount; i++)
            {
                indexMatrix[i] = new GsiAndShdIndexes[i + 1];
            }

            int totalCounter = 0;
            Parallel.For(0, rowCount, i =>
            {
                DataRow rowA = _trainingDataTable.Rows[i];

                int k = i + 1;
                for (int j = k; j < rowCount; j++)
                {

                    DataRow rowB = _trainingDataTable.Rows[j];
                    GsiAndShdIndexes newIndex = new GsiAndShdIndexes();
                    newIndex.PsiIndex = CalculatePSI(rowA, rowB, numberofAttributes);
                    newIndex.ShdIndex = CalculateSHD(rowA, rowB);

                    indexMatrix[j][i] = newIndex;
                    totalCounter++;
                }
            }
            );
        }

        private double CalculatePSI(DataRow rowA, DataRow rowB, int numberofAttributes)
        {

            int simalrityIndex = 0;

            for (int k = 0; k < rowA.ItemArray.Length; k++)
            {
                if ((rowA[k].ToString().ToLower() == "true") && (rowB[k].ToString().ToLower() == "true"))
                {
                    simalrityIndex++;
                }
            }

            double result = (double)simalrityIndex / (double)numberofAttributes;
            return result;
        }

        private double CalculateSHD(DataRow rowA, DataRow rowB)
        {
            int simalrityIndex = 0;
            int vectorLength = rowA.ItemArray.Length;
            for (int k = 0; k < vectorLength; k++)
            {
                if (rowA[k].ToString() != rowB[k].ToString())
                {
                    simalrityIndex++;
                }
            }

            double result = (double)(vectorLength - simalrityIndex) / (double)vectorLength;
            return result;
        }

        #endregion

        #region Convert Data

        private void ConvertToBooleanDS()
        {
            int colIndex = -1;

            _trainingDataTable = new DataTable();
            _processedPositive = new DataTable();
            _processedNegetive = new DataTable();

            for (int i = 0; i < _originalDataTable.Rows.Count; i++)
            {
                _trainingDataTable.Rows.Add(_trainingDataTable.NewRow());
            }

            foreach (DataColumn col in _originalDataTable.Columns)
            {
                colIndex++;

                if (col.ColumnName.CompareTo(_idColumnName) == 0 ||
                    col.ColumnName.CompareTo(_resultColumnName) == 0)
                {
                    continue;
                }

                AnalyzeAndAddColumn(col, colIndex, _typeLine[colIndex]);
            }
        }

        private void ConvertToBooleanRollout()
        {

            int colIndex = -1;
            _rolloutDataTable = new DataTable();

            for (int i = 0; i < _originalRolloutDataTable.Rows.Count; i++)
            {
                _rolloutDataTable.Rows.Add(_rolloutDataTable.NewRow());
            }

            foreach (DataColumn col in _originalRolloutDataTable.Columns)
            {
                colIndex++;

                if (col.ColumnName.CompareTo(_idColumnName) == 0 ||
                    col.ColumnName.CompareTo(_resultColumnName) == 0)
                {
                    continue;
                }

                AnalyzeAndAddColumnToRolloutSet(col, colIndex, _typeLine[colIndex]);
            }
        }

        private void ConvertToBooleanValidation()
        {

            int colIndex = -1;
            _validationDataTable = new DataTable();
            _processedPositive = new DataTable();
            _processedNegetive = new DataTable();

            for (int i = 0; i < _originalValidationDataTable.Rows.Count; i++)
            {
                _validationDataTable.Rows.Add(_validationDataTable.NewRow());
            }

            foreach (DataColumn col in _originalValidationDataTable.Columns)
            {
                colIndex++;

                if (col.ColumnName.CompareTo(_idColumnName) == 0 ||
                    col.ColumnName.CompareTo(_resultColumnName) == 0)
                {
                    continue;
                }

                AnalyzeAndAddColumnToValidationSet(col, colIndex, _typeLine[colIndex]);
            }
        }

        private void AnalyzeAndAddColumnToRolloutSet(DataColumn col, int colIndex, string colType)
        {
            int disitinctValueCount = 0;

            Dictionary<object, int> distinctValuesDic = new Dictionary<object, int>();

            object rowValue = null;
            foreach (DataRow row in _originalRolloutDataTable.Rows)
            {
                rowValue = row[colIndex];

                if (!distinctValuesDic.ContainsKey(rowValue))
                {
                    distinctValuesDic.Add(rowValue, 1);
                }
                else
                {
                    distinctValuesDic[rowValue]++;
                }
            }

            Type type = rowValue.GetType();

            disitinctValueCount = distinctValuesDic.Count;

            if (colType == "bool")
            {
                AddNewColumn(col.ColumnName + "_False", false, true);
                AddNewColumn(col.ColumnName + "_True", false, true);

                FillRolloutDoubleBooleanValues(col, colIndex);
            }

            if (colType == "numeric")
            {
                ColumnInformation currentCol = null;
                foreach (ColumnInformation trainingCol in _columnsInformationList)
                {
                    if (trainingCol.OriginalColumnName == col.ColumnName)
                    {
                        currentCol = trainingCol;
                    }
                }

                for (int i = 0; i < currentCol.NumberOfSubColumns; i++)
                {
                    AddNewColumn(currentCol.SubColumnsNamesList[i], false, true);
                }

                FillRolloutNumricValues(currentCol, colIndex);
            }

            if (colType == "nominal")
            {
                List<object> sortedKeys = distinctValuesDic.Keys.ToList<object>();

                List<string> columnsNames = new List<string>();

                foreach (KeyValuePair<object, int> pair in distinctValuesDic)
                {
                    string columnName = col.ColumnName + "_" + pair.Key.ToString();
                    AddNewColumn(columnName, false, true);
                    FillRolloutNominalValues(pair.Key.ToString(), colIndex);
                }
            }
        }

        private void AnalyzeAndAddColumnToValidationSet(DataColumn col, int colIndex, string colType)
        {
            int disitinctValueCount = 0;

            Dictionary<object, int> distinctValuesDic = new Dictionary<object, int>();

            object rowValue = null;
            foreach (DataRow row in _originalValidationDataTable.Rows)
            {
                rowValue = row[colIndex];

                if (!distinctValuesDic.ContainsKey(rowValue))
                {
                    distinctValuesDic.Add(rowValue, 1);
                }
                else
                {
                    distinctValuesDic[rowValue]++;
                }
            }

            Type type = rowValue.GetType();

            disitinctValueCount = distinctValuesDic.Count;

            if (colType == "bool")
            {
                AddNewColumn(col.ColumnName + "_False", true);
                AddNewColumn(col.ColumnName + "_True", true);

                FillValidationDoubleBooleanValues(col, colIndex);
            }

            if (colType == "numeric")
            {
                ColumnInformation currentCol = null;
                foreach (ColumnInformation trainingCol in _columnsInformationList)
                {
                    if (trainingCol.OriginalColumnName == col.ColumnName)
                    {
                        currentCol = trainingCol;
                    }
                }

                for (int i = 0; i < currentCol.NumberOfSubColumns; i++)
                {
                    AddNewColumn(currentCol.SubColumnsNamesList[i], true);
                }

                FillValidationNumricValues(currentCol, colIndex);
            }

            if (colType == "nominal")
            {
                List<object> sortedKeys = distinctValuesDic.Keys.ToList<object>();

                List<string> columnsNames = new List<string>();

                foreach (KeyValuePair<object, int> pair in distinctValuesDic)
                {
                    string columnName = col.ColumnName + "_" + pair.Key.ToString();
                    AddNewColumn(columnName, true);
                    FillValidationNominalValues(pair.Key.ToString(), colIndex);
                }
            }
        }

        private void AnalyzeAndAddColumn(DataColumn col, int colIndex, string colType)
        {
            int disitinctValueCount = 0;

            ColumnInformation newColumn = new ColumnInformation();
            newColumn.OriginalColumnName = col.ColumnName;


            Dictionary<object, int> distinctValuesDic = new Dictionary<object, int>();

            object rowValue = null;
            foreach (DataRow row in _originalDataTable.Rows)
            {
                rowValue = row[colIndex];

                if (!distinctValuesDic.ContainsKey(rowValue))
                {
                    distinctValuesDic.Add(rowValue, 1);
                }
                else
                {
                    distinctValuesDic[rowValue]++;
                }
            }

            Type type = rowValue.GetType();

            disitinctValueCount = distinctValuesDic.Count;

            if (colType == "bool")
            {
                AddNewColumn(col.ColumnName + "_False");
                AddNewColumn(col.ColumnName + "_True");


                FillDoubleBooleanValues(col, colIndex);

                newColumn.NumberOfSubColumns = 2;
                newColumn.SubColumnsNamesList.Add(col.ColumnName + "_False");
                newColumn.SubColumnsNamesList.Add(col.ColumnName + "_True");
                newColumn.R_Index = 0.75;

            }

            if (colType == "numeric")
            {
                int range = _range;
                Dictionary<int, int> sortedDistinctValuesDic = TransformIntDicToRangeColumn(distinctValuesDic, range);

                List<int> sortedKeys = sortedDistinctValuesDic.Keys.ToList<int>();
                sortedKeys.Sort();

                newColumn.NumericRanges = sortedKeys;
                List<string> columnsNames = new List<string>();

                if (sortedKeys.Count < range)
                {
                    range = sortedKeys.Count;
                }

                newColumn.NumberOfSubColumns = range;

                for (int i = 0; i < range; i++)
                {

                    string columnName = col.ColumnName + "_" + sortedKeys[i].ToString();
                    columnsNames.Add(columnName);
                    AddNewColumn(columnName);
                    newColumn.SubColumnsNamesList.Add(columnName);

                }

                FillNumricValues(sortedKeys, columnsNames, colIndex);
                newColumn.R_Index = (double)(range + 1) / (double)(range * 2);
            }

            if (colType == "nominal")
            {
                List<object> sortedKeys = distinctValuesDic.Keys.ToList<object>();

                List<string> columnsNames = new List<string>();

                int range = newColumn.NumberOfSubColumns = distinctValuesDic.Count;

                foreach (KeyValuePair<object, int> pair in distinctValuesDic)
                {
                    string columnName = col.ColumnName + "_" + pair.Key.ToString();
                    columnsNames.Add(columnName);
                    AddNewColumn(columnName);
                    FillNominalValues(pair.Key.ToString(), colIndex);
                    newColumn.SubColumnsNamesList.Add(columnName);
                }

                newColumn.R_Index = (double)(range + 1) / (double)(range * 2);
            }

            _columnsInformationList.Add(newColumn);
        }

        private void AddNewColumn(string newColumnName, bool addToValidationDataTable = false, bool addToRolloutDataTable = false)
        {
            DataColumn newFalseCol = new DataColumn(newColumnName, typeof(bool));
            newFalseCol.DefaultValue = false;

            DataColumn newFalseCol1 = new DataColumn(newColumnName, typeof(bool));
            newFalseCol.DefaultValue = false;

            DataColumn newFalseCol2 = new DataColumn(newColumnName, typeof(bool));
            newFalseCol.DefaultValue = false;

            if (addToValidationDataTable)
            {
                _validationDataTable.Columns.Add(newFalseCol);
            }
            else if (addToRolloutDataTable)
            {
                _rolloutDataTable.Columns.Add(newFalseCol);
            }
            else
            {
                _trainingDataTable.Columns.Add(newFalseCol);
            }

            if (!addToRolloutDataTable)
            {
                _processedPositive.Columns.Add(newFalseCol1);
                _processedNegetive.Columns.Add(newFalseCol2);
            }
        }

        private void FillRolloutNominalValues(string key, int colIndex)
        {
            int totalNewColumnNumber = _rolloutDataTable.Columns.Count;

            for (int rowCounter = 0; rowCounter < _originalRolloutDataTable.Rows.Count; rowCounter++)
            {
                if (_originalRolloutDataTable.Rows[rowCounter][colIndex].ToString() == key)
                {
                    _rolloutDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
            }
        }

        private void FillValidationNominalValues(string key, int colIndex)
        {
            int totalNewColumnNumber = _validationDataTable.Columns.Count;

            for (int rowCounter = 0; rowCounter < _originalValidationDataTable.Rows.Count; rowCounter++)
            {
                if (_originalValidationDataTable.Rows[rowCounter][colIndex].ToString() == key)
                {
                    _validationDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
            }
        }

        private void FillNominalValues(string key, int colIndex)
        {
            int totalNewColumnNumber = _trainingDataTable.Columns.Count;

            for (int rowCounter = 0; rowCounter < _originalDataTable.Rows.Count; rowCounter++)
            {
                if (_originalDataTable.Rows[rowCounter][colIndex].ToString() == key)
                {
                    _trainingDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
            }
        }

        private void FillRolloutDoubleBooleanValues(DataColumn col, int colIndex)
        {
            int totalNewColumnNumber = _rolloutDataTable.Columns.Count;

            for (int rowCounter = 0; rowCounter < _originalRolloutDataTable.Rows.Count; rowCounter++)
            {
                if (_originalRolloutDataTable.Rows[rowCounter][colIndex].ToString() == "1")
                {
                    _rolloutDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
                else
                {
                    _rolloutDataTable.Rows[rowCounter][totalNewColumnNumber - 2] = true;
                }
            }
        }

        private void FillValidationDoubleBooleanValues(DataColumn col, int colIndex)
        {
            int totalNewColumnNumber = _validationDataTable.Columns.Count;

            for (int rowCounter = 0; rowCounter < _originalValidationDataTable.Rows.Count; rowCounter++)
            {
                if (_originalValidationDataTable.Rows[rowCounter][colIndex].ToString() == "1")
                {
                    _validationDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
                else
                {
                    _validationDataTable.Rows[rowCounter][totalNewColumnNumber - 2] = true;
                }
            }
        }

        private void FillDoubleBooleanValues(DataColumn col, int colIndex)
        {
            int totalNewColumnNumber = _trainingDataTable.Columns.Count;

            for (int rowCounter = 0; rowCounter < _originalDataTable.Rows.Count; rowCounter++)
            {
                if (_originalDataTable.Rows[rowCounter][colIndex].ToString() == "1")
                {
                    _trainingDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
                else
                {
                    _trainingDataTable.Rows[rowCounter][totalNewColumnNumber - 2] = true;
                }
            }
        }

        private void FillBooleanValues(DataColumn col, int colIndex)
        {
            int totalNewColumnNumber = _trainingDataTable.Columns.Count;

            for (int rowCounter = 0; rowCounter < _originalDataTable.Rows.Count; rowCounter++)
            {
                if (_originalDataTable.Rows[rowCounter][colIndex].ToString() == "1")
                {
                    _trainingDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
            }
        }

        private void FillRolloutNumricValues(ColumnInformation currentCol, int colIndex)
        {
            for (int rowCounter = 0; rowCounter < _originalRolloutDataTable.Rows.Count; rowCounter++)
            {
                int originalValue = int.Parse(_originalRolloutDataTable.Rows[rowCounter][colIndex].ToString());

                int newColumnIndexOffset = currentCol.NumberOfSubColumns;
                int totalNewColumnNumber = _rolloutDataTable.Columns.Count;
                bool insertToLastColumn = true;

                foreach (int key in currentCol.NumericRanges)
                {
                    if (originalValue <= key)
                    {
                        _rolloutDataTable.Rows[rowCounter][totalNewColumnNumber - newColumnIndexOffset] = true;
                        insertToLastColumn = false;
                        break;
                    }

                    newColumnIndexOffset--;
                }

                if (insertToLastColumn)
                {
                    _rolloutDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
            }
        }

        private void FillValidationNumricValues(ColumnInformation currentCol, int colIndex)
        {
            for (int rowCounter = 0; rowCounter < _originalValidationDataTable.Rows.Count; rowCounter++)
            {
                int originalValue = int.Parse(_originalValidationDataTable.Rows[rowCounter][colIndex].ToString());

                int newColumnIndexOffset = currentCol.NumberOfSubColumns;
                int totalNewColumnNumber = _validationDataTable.Columns.Count;
                bool insertToLastColumn = true;

                foreach (int key in currentCol.NumericRanges)
                {
                    if (originalValue <= key)
                    {
                        _validationDataTable.Rows[rowCounter][totalNewColumnNumber - newColumnIndexOffset] = true;
                        insertToLastColumn = false;
                        break;
                    }

                    newColumnIndexOffset--;
                }

                if (insertToLastColumn)
                {
                    _validationDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
            }
        }

        private void FillNumricValues(List<int> sortedKeys, List<string> columnsNames, int colIndex)
        {
            for (int rowCounter = 0; rowCounter < _originalDataTable.Rows.Count; rowCounter++)
            {
                int originalValue = int.Parse(_originalDataTable.Rows[rowCounter][colIndex].ToString());

                int newColumnIndexOffset = columnsNames.Count;
                int totalNewColumnNumber = _trainingDataTable.Columns.Count;
                bool insertToLastColumn = true;

                foreach (int key in sortedKeys)
                {
                    if (originalValue <= key)
                    {
                        _trainingDataTable.Rows[rowCounter][totalNewColumnNumber - newColumnIndexOffset] = true;
                        insertToLastColumn = false;
                        break;
                    }

                    newColumnIndexOffset--;
                }

                if (insertToLastColumn)
                {
                    _trainingDataTable.Rows[rowCounter][totalNewColumnNumber - 1] = true;
                }
            }
        }

        private Dictionary<int, int> TransformIntDicToRangeColumn(Dictionary<object, int> distinctValuesDic, int numberOfRanges)
        {

            Dictionary<int, int> sortedDistinctValuesDic = new Dictionary<int, int>();

            foreach (KeyValuePair<object, int> pair in distinctValuesDic)
            {
                sortedDistinctValuesDic.Add(int.Parse(pair.Key.ToString()), pair.Value);
            }

            List<int> sortedKeys = sortedDistinctValuesDic.Keys.ToList<int>();
            sortedKeys.Sort();

            int currentNumberOfRanges = sortedDistinctValuesDic.Count;

            while (currentNumberOfRanges > numberOfRanges)
            {
                int minValue = int.MaxValue;
                int minKey = 0;

                KeyValuePair<int, int> previousPair = new KeyValuePair<int, int>(0, 0);
                KeyValuePair<int, int> nextPair = new KeyValuePair<int, int>(0, 0);
                KeyValuePair<int, int> minPair = new KeyValuePair<int, int>(0, 0);
                KeyValuePair<int, int> tempPair = new KeyValuePair<int, int>(0, 0);

                bool pairWasSelected = false;
                foreach (int sortedKey in sortedKeys)
                {
                    KeyValuePair<int, int> pair = new KeyValuePair<int, int>(sortedKey, sortedDistinctValuesDic[sortedKey]);

                    if (pairWasSelected)
                    {
                        nextPair = new KeyValuePair<int, int>(pair.Key, pair.Value);
                        pairWasSelected = false;
                    }

                    if (minValue > pair.Value)
                    {
                        minValue = pair.Value;
                        minKey = pair.Key;
                        pairWasSelected = true;
                        minPair = new KeyValuePair<int, int>(minKey, minValue);
                        previousPair = new KeyValuePair<int, int>(tempPair.Key, tempPair.Value);
                        nextPair = new KeyValuePair<int, int>(0, 0);
                    }

                    tempPair = new KeyValuePair<int, int>(pair.Key, pair.Value);
                }


                if (previousPair.Value == 0)
                {
                    sortedDistinctValuesDic[minPair.Key] += nextPair.Value;
                    sortedDistinctValuesDic.Remove(nextPair.Key);
                    sortedKeys.Remove(nextPair.Key);
                }
                else if (nextPair.Value == 0)
                {
                    sortedDistinctValuesDic[previousPair.Key] += minPair.Value;
                    sortedDistinctValuesDic.Remove(minPair.Key);
                    sortedKeys.Remove(minPair.Key);
                }
                else if (nextPair.Value > previousPair.Value)
                {
                    sortedDistinctValuesDic[previousPair.Key] += minPair.Value;
                    sortedDistinctValuesDic.Remove(minPair.Key);
                    sortedKeys.Remove(minPair.Key);
                }
                else
                {
                    sortedDistinctValuesDic[minPair.Key] += nextPair.Value;
                    sortedDistinctValuesDic.Remove(nextPair.Key);
                    sortedKeys.Remove(nextPair.Key);
                }

                currentNumberOfRanges--;
            }

            return sortedDistinctValuesDic;

        }

        #endregion
    }
}
