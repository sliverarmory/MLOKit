using System;

namespace MLOKit.Objects.BigML
{
    class Project
    {


        public string projectName { get; set; }
        public string projectCreator { get; set; }
        public string visibility { get; set; }
        public string projectCreationDate { get; set; }
        public string projectID { get; set; }

        public Project(string projectName, string projectCreator, string visibility, string projectCreationDate, string projectID)
        {
            this.projectName = projectName;
            this.projectCreator = projectCreator;
            this.visibility = visibility;
            this.projectCreationDate = projectCreationDate;
            this.projectID = projectID;
        }

    }
}
