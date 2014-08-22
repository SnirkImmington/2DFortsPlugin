using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace SnirkPlugin
{
    /// <summary>
    /// Basic data of the list of downloads available for the repo.
    /// </summary>
    [DataContract]
    class ReleaseList
    {
        /// <summary>
        /// The downloads available.
        /// </summary>
        [DataMember]
        public ReleaseInfo[] Releases { get; set; }
    }

    /// <summary>
    /// Contains information for each download available for the repo.
    /// </summary>
    [DataContract]
    class ReleaseInfo
    {
        /// <summary>
        /// The download URL.
        /// </summary>
        [DataMember(Name="url")]
        public string URL { get; set; }
        /// <summary>
        /// GitHub's ID of the release.
        /// </summary>
        [DataMember(Name="id")]
        public int ID { get; set; }
        /// <summary>
        /// An alternate URL
        /// </summary>
        [DataMember(Name="html_url")]
        public string HTMLURL { get; set; }

        /// <summary>
        /// The name of the release/file.
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
        /// <summary>
        /// Description provided of the release.
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }


        /// <summary>
        /// ISO string of the creation time.
        /// </summary>
        [DataMember(Name="created_at")]
        public string CreationTime { get; set; }
        /// <summary>
        /// The size of the file.
        /// </summary>
        [DataMember(Name="size")]
        public uint Size { get; set; }

        /// <summary>
        /// Number of downloads.
        /// </summary>
        [DataMember(Name="download_count")]
        public uint Downloads { get; set; }
        /// <summary>
        /// Type of content - should contain "application"
        /// </summary>
        [DataMember(Name="content_type")]
        public string ContentType { get; set; }
    }
}
