﻿using JIC.Base.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Base.Interfaces
{
    public interface IPersonInformationRepository 
    {
        vw_InformationPerson GetInformationPerson(string NatNo,string Name);
    }
     
}