using System;

namespace MLOKit.Objects.AzureML
{
    class Dataset
    {

        public string datasetID { get; set; }
        public string datasetState { get; set; }
        public string datasetFilePath { get; set; }
        public string datasetFileType { get; set; }
        public string datastoreName { get; set; }



        public Dataset(string datasetID, string datasetState, string datasetFilePath, string datasetFileType, string datastoreName)
        {
            this.datasetID = datasetID;
            this.datasetState = datasetState;
            this.datasetFilePath = datasetFilePath;
            this.datasetFileType = datasetFileType;
            this.datastoreName = datastoreName;



        }

    }
}
