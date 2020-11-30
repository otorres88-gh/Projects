using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessEntities
{
    public class Configuration
    {
        [XmlArrayItem("SearchRunner")]
        public List<SerializableSearchRunner> SearchRunners { get; set; }
    }
}
