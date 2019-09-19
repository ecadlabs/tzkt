﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Tzkt.Data.Models.Base;

namespace Tzkt.Data.Models
{
    public class BallotOperation : BaseOperation
    {
        public int PeriodId { get; set; }
        public int ProposalId { get; set; }
        public int SenderId { get; set; }

        public Vote Vote { get; set; }

        #region relations
        [ForeignKey(nameof(SenderId))]
        public Delegate Sender { get; set; }

        [ForeignKey(nameof(ProposalId))]
        public Proposal Proposal { get; set; }

        [ForeignKey(nameof(PeriodId))]
        public VotingPeriod Period { get; set; }
        #endregion
    }

    public static class BallotOperationModel
    {
        public static void BuildBallotOperationModel(this ModelBuilder modelBuilder)
        {
            #region indexes
            modelBuilder.Entity<BallotOperation>()
                .HasIndex(x => x.Level);

            modelBuilder.Entity<BallotOperation>()
                .HasIndex(x => x.OpHash);

            modelBuilder.Entity<BallotOperation>()
                .HasIndex(x => x.SenderId);
            #endregion

            #region keys
            modelBuilder.Entity<BallotOperation>()
                .HasKey(x => x.Id);
            #endregion

            #region props
            modelBuilder.Entity<BallotOperation>()
                .Property(x => x.OpHash)
                .IsFixedLength(true)
                .HasMaxLength(51)
                .IsRequired();
            #endregion

            #region relations
            modelBuilder.Entity<BallotOperation>()
                .HasOne(x => x.Block)
                .WithMany(x => x.Ballots)
                .HasForeignKey(x => x.Level)
                .HasPrincipalKey(x => x.Level);

            modelBuilder.Entity<BallotOperation>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.Ballots)
                .HasForeignKey(x => x.SenderId);

            modelBuilder.Entity<BallotOperation>()
                .HasOne(x => x.Proposal)
                .WithMany(x => x.Ballots)
                .HasForeignKey(x => x.ProposalId);

            modelBuilder.Entity<BallotOperation>()
                .HasOne(x => x.Period)
                .WithMany(x => x.Ballots)
                .HasForeignKey(x => x.PeriodId);
            #endregion
        }
    }

    public enum Vote
    {
        Yay,
        Nay,
        Pass
    }
}