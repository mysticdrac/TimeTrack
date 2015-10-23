using System.Runtime.Serialization;
namespace DCW
{
    /*params:
                                           {
                               "status": "ok",
                               "time": "10",
                               "project": {
                                   "name": "TimeTrack",
                                   "Task": [
                                       {
                                           "name": "design v1",
                                           "time": "15000"
                                       },
                                       {
                                           "name": "design v1",
                                           "time": "25000"
                                       }
                                   ]
                               }
                           }
                   */
    [DataContract]
    internal class JsonResp
    {
        [DataMember(Name = "status")]
        internal string status { get; set; }

        [DataMember(Name = "time")]
        internal uint time { get; set; }

        [DataMember(Name = "Project")]
        internal Project[] Project { get; set; }
    }

    [DataContract]
    internal class Project {

        [DataMember(Name = "name")]
        internal string name { get; set; }

        [DataMember(Name = "Task")]
        internal Task[] Task { get; set; }

    }

    [DataContract]
    internal class Task {

        [DataMember(Name = "name")]
        internal string name { get; set; }

        [DataMember(Name = "time")]
        internal uint time { get; set; }

    }

    [DataContract]
    internal class ScResp
    {

        [DataMember(Name = "status")]
        internal string status { get; set; }

        [DataMember(Name = "time")]
        internal uint time { get; set; }

        [DataMember(Name = "tottime")]
        internal uint tottime { get; set; }

    }
}
