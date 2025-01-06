using System;

namespace MLOKit.Objects.AzureML
{
    class Model
    {

        public string modelID { get; set; }
        public string modelName { get; set; }
        public string assetID { get; set; }
        public string createdTime { get; set; }

        public string modifiedTime { get; set; }
        public string provisioningState { get; set; }
        public string modelType { get; set; }


        public Model(string modelID, string modelName, string assetID, string createdTime, string modifiedTime, string provisioningState, string modelType)
        {
            this.modelID = modelID;
            this.modelName = modelName;
            this.assetID = assetID;
            this.createdTime = createdTime;
            this.modifiedTime = modifiedTime;
            this.provisioningState = provisioningState;
            this.modelType = modelType;


        }


    }
}
