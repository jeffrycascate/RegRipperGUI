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
    [DebuggerDisplay("{Name}-'{Description}'")]
    [DataContract]
    [Serializable]
    public class AddIn : DTOs.Action
    {

        [DataMember()]
        [JsonProperty]
        public List<string> Filters { get; set; }

    }
}
