namespace JIC.Fault.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cases_CaseDefendants
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cases_CaseDefendants()
        {
            Cases_DefendantsDecision = new HashSet<Cases_DefendantsDecision>();
            Z_DefendantsSessionLawyers = new HashSet<Z_DefendantsSessionLawyers>();
            Cases_DefendatnsCaseLog = new HashSet<Cases_DefendatnsCaseLog>();
            Cases_DefendatnsSessionsLog = new HashSet<Cases_DefendatnsSessionsLog>();
        }

        public long ID { get; set; }

        public long PersonID { get; set; }

        public int CaseID { get; set; }

        public bool IsActive { get; set; }

        public bool IsCivilRightProsecutor { get; set; }

        public int? DefendantIndex { get; set; }

        public virtual Cases_Cases Cases_Cases { get; set; }

        public virtual Configurations_Persons Configurations_Persons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cases_DefendantsDecision> Cases_DefendantsDecision { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Z_DefendantsSessionLawyers> Z_DefendantsSessionLawyers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cases_DefendatnsCaseLog> Cases_DefendatnsCaseLog { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cases_DefendatnsSessionsLog> Cases_DefendatnsSessionsLog { get; set; }

        public virtual Service_CaseDefendantProsecution Service_CaseDefendantProsecution { get; set; }
    }
}