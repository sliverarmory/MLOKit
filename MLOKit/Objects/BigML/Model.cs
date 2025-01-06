using System;

namespace MLOKit.Objects.BigML
{
    class Model
    {

        public string modelName { get; set; }
        public string modelID { get; set; }
        public string createdBy { get; set; }
        public string dateCreated { get; set; }
        public string dateUpdated { get; set; }

        public string associatedProject { get; set; }

        public string associatedDataset { get; set; }
        public string visibility { get; set; }

        public Model(string modelName, string modelID, string createdBy, string dateCreated, string dateUpdated, string associatedProject, string associatedDataset, string visibility)
        {
            this.modelName = modelName;
            this.modelID = modelID;
            this.createdBy = createdBy;
            this.dateCreated = dateCreated;
            this.dateUpdated = dateUpdated;
            this.associatedProject = associatedProject;
            this.associatedDataset = associatedDataset;
            this.visibility = visibility;

        }

    }
}
