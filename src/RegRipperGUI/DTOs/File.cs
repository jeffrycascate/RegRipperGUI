using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RegRipperGUI.DTOs
{
    [DebuggerDisplay("{Name}-'{LastWrite}'")]
    [DataContract]
    [Serializable]
    public class File : DTOs.Action
    {
        [DataMember()]
        [JsonProperty]
        public System.DateTime LastWrite { get; set; }

        [DataMember()]
        [JsonProperty]
        public bool IsFolder { get; set; }

        [DataMember()]
        [JsonProperty]
        public string PathFullName { get; set; }

        [DataMember()]
        [JsonProperty]
        public string PathRelative { get; set; }

        [DataMember()]
        [JsonProperty]
        public List<File> Childs { get; set; }

        [DataMember()]
        [JsonProperty]
        public double Size { get; set; }

        [DataMember()]
        [JsonProperty]
        public bool Selected { get; set; }

        [DataMember()]
        [JsonProperty]
        public string FullPath { get; set; }

        [DataMember()]
        [JsonProperty]
        public string Path { get; set; }

        [DataMember()]
        [JsonProperty]
        public bool InCatalog { get; set; }

        [DataMember()]
        [JsonProperty]
        public string Extencion { get; set; }

        [DataMember()]
        [JsonProperty]
        public byte[] BodyBinary { get; set; }

        [DataMember()]
        [JsonProperty]
        public DateTime ModifyDate { get; set; }


    }
}
