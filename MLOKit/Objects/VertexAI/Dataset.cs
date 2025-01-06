using System;

namespace MLOKit.Objects.VertexAI
{
    class Dataset
    {

        public string datasetID { get; set; }
        public string datasetDisplayName { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }
        public string uri { get; set; }
        public string region { get; set; }

        public Dataset(string datasetID, string datasetDisplayName, string createTime, string updateTime, string uri, string region)
        {
            this.datasetID = datasetID;
            this.datasetDisplayName = datasetDisplayName;
            this.createTime = createTime;
            this.updateTime = updateTime;
            this.uri = uri;
            this.region = region;
        }

    }
}
