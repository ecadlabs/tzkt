﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tzkt.Data.Models
{
    public class ExplorationPeriod : VotingPeriod
    {
        public int ProposalId { get; set; }

        public int TotalStake { get; set; }
        public int Participation { get; set; }
        public int Quorum { get; set; }

        public int Abstainings { get; set; }
        public int Approvals { get; set; }
        public int Refusals { get; set; }

        #region relations
        [ForeignKey(nameof(ProposalId))]
        public Proposal Proposal { get; set; }
        #endregion
    }

    public static class ExplorationPeriodModel
    {
        public static void BuildExplorationPeriodModel(this ModelBuilder modelBuilder)
        {

        }
    }
}