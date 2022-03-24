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

    [DebuggerDisplay("{Name}")]
    [DataContract]
    [Serializable]
    public class Action
    {
        [DataMember()]
        [JsonProperty]
        public string Description { get; set; }

        [DataMember()]
        [JsonProperty]
        public bool IsSelected { get; set; } = false;

        [DataMember()]
        [JsonProperty]
        public bool Enable { get; set; } = true;

        [DataMember()]
        [JsonProperty]
        public bool IsExpanded { get; set; }

        [DataMember()]
        [JsonProperty]
        public Object Value { get; set; }

        [DataMember()]
        [JsonProperty]
        public virtual string Icon { get; set; }

        [DataMember()]
        [JsonProperty]
        public virtual string Name { get; set; }

        [DataMember()]
        [JsonProperty]
        public virtual List<Action> Items { get; set; } = new List<Action>();
    }
}
