﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tzkt.Api.Models
{
    public class ProposalOperation : Operation
    {
        public override string Type => OpTypes.Proposal;

        public override int Id { get; set; }

        public int Level { get; set; }

        public DateTime Timestamp { get; set; }

        public string Block { get; set; }

        public string Hash { get; set; }

        public PeriodInfo Period { get; set; }

        public ProposalAlias Proposal { get; set; }

        public Alias Delegate { get; set; }

        public int Rolls { get; set; }

        public bool Duplicated { get; set; }
    }
}
