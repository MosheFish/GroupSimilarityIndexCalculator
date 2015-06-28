using System.Collections.Generic;

namespace GroupSimilarityIndexLogicUnit
{
    public class ColumnInformation
    {
        public string OriginalColumnName { get; set; }
        public List<string> SubColumnsNamesList { get; set; }
        public double R_Index { get; set; }
        public List<double> GSI_Indexes { get; set; }
        public int NumberOfSubColumns { get; set; }
        public string type { get; set; }
        public List<int> NumericRanges { get; set; }

        public ColumnInformation()
        {
            SubColumnsNamesList = new List<string>();
            GSI_Indexes = new List<double>();
            NumericRanges = new List<int>();
        }
    }
}
