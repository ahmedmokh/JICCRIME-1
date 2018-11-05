﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Base.Interfaces
{
    public interface IDefendantChargesRepository
    {
        void SyncDefendantCharges(long defendantID, List<int> crimes);
    }
}
