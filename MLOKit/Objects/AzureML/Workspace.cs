using System;

namespace MLOKit.Objects.AzureML
{
    class Workspace
    {


        public string workspaceName { get; set; }
        public string workspaceID { get; set; }
        public string workspaceRegion { get; set; }
        public string creationTime { get; set; }

        public string createdBy { get; set; }

        public string resourceGroup { get; set; }



        public Workspace(string workspaceName, string workspaceID, string workspaceRegion, string creationTime, string createdBy, string resourceGroup)
        {
            this.workspaceName = workspaceName;
            this.workspaceID = workspaceID;
            this.workspaceRegion = workspaceRegion;
            this.creationTime = creationTime;
            this.createdBy = createdBy;
            this.resourceGroup = resourceGroup;


        }


    }
}
