﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Fault.Entities.Models
{
    public class Service_CaseDefendantProsecution
    {
        [Key,ForeignKey("Cases_CaseDefendant")]
        public long CaseDefendantID { get; set; }
        public int ProsecutionID { get; set; }
        public virtual Cases_CaseDefendants Cases_CaseDefendant { get; set; }

    }
}
