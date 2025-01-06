using System;

namespace MLOKit.Objects.BigML
{
    class Dataset
    {


        public string datasetName { get; set; }
        public string datasetID { get; set; }
        public string associatedDataSource { get; set; }
        public string associatedProject { get; set; }
        public string visibility { get; set; }

        public string dateCreated { get; set; }

        public string dateUpdated { get; set; }

        public Dataset(string datasetName, string datasetID, string associatedDataSource, string associatedProject, string visibility, string dateCreated, string dateUpdated)
        {
            this.datasetName = datasetName;
            this.datasetID = datasetID;
            this.associatedDataSource = associatedDataSource;
            this.associatedProject = associatedProject;
            this.visibility = visibility;
            this.dateCreated = dateCreated;
            this.dateUpdated = dateUpdated;
        }

    }
}
