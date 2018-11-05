namespace JIC.Crime.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cases_DefendatnsSessionsLog
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long DefendantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SessionID { get; set; }

        public int PresenceStatusID { get; set; }

        public int? CourtStatusID { get; set; }

        public virtual Cases_CaseDefendants Cases_CaseDefendants { get; set; }

        public virtual Cases_CaseSessions Cases_CaseSessions { get; set; }

        public virtual Configurations_Lookups Configurations_Lookups { get; set; }

        public virtual Configurations_Lookups Configurations_Lookups1 { get; set; }
    }
}
