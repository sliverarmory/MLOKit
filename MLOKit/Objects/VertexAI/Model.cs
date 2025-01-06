using System;


namespace MLOKit.Objects.VertexAI
{
    class Model
    {


        public string modelID { get; set; }
        public string modelDisplayName { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }
        public string sourceType { get; set; }
        public string exportableFormat { get; set; }
        public string region { get; set; }

        public Model(string modelID, string modelDisplayName, string createTime, string updateTime, string sourceType, string region, string exportableFormat)
        {
            this.modelID = modelID;
            this.modelDisplayName = modelDisplayName;
            this.createTime = createTime;
            this.updateTime = updateTime;
            this.sourceType = sourceType;
            this.region = region;
            this.exportableFormat = exportableFormat;
        }


    }
}
