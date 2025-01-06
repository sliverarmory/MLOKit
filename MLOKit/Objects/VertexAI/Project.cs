using System;

namespace MLOKit.Objects.VertexAI
{
    class Project
    {

        public string projectName { get; set; }
        public string projectNumber { get; set; }
        public string projectState { get; set; }
        public string projectCreationDate { get; set; }
        public string projectID { get; set; }

        public Project(string projectName, string projectNumber, string projectState, string projectCreationDate, string projectID)
        {
            this.projectName = projectName;
            this.projectNumber = projectNumber;
            this.projectState = projectState;
            this.projectCreationDate = projectCreationDate;
            this.projectID = projectID;
        }

    }
}
