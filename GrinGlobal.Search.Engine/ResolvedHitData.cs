using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GrinGlobal.Search.Engine {
    [DataContract]
    public class ResolvedHitData {

        [DataMember]
        public string IndexName;
        [DataMember]
        public string FieldName;
        [DataMember]
        public int PrimaryKeyID;
        [DataMember]
        public int KeywordIndex;
        [DataMember]
        public List<int> ResolvedIDList;

    }
}
