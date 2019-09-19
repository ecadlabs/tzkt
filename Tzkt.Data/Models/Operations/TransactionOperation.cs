﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Tzkt.Data.Models.Base;

namespace Tzkt.Data.Models
{
    public class TransactionOperation : InternalOperation
    {
        public int TargetId { get; set; }
        public bool TargetAllocated { get; set; }

        public long Amount { get; set; }

        #region relations
        [ForeignKey(nameof(TargetId))]
        public Account Target { get; set; }
        #endregion

        #region indirect relations
        public List<DelegationOperation> InternalDelegations { get; set; }
        public List<OriginationOperation> InternalOriginations { get; set; }
        public List<TransactionOperation> InternalTransactions { get; set; }
        #endregion
    }

    public static class TransactionOperationModel
    {
        public static void BuildTransactionOperationModel(this ModelBuilder modelBuilder)
        {
            #region indexes
            modelBuilder.Entity<TransactionOperation>()
                .HasIndex(x => x.Level);

            modelBuilder.Entity<TransactionOperation>()
                .HasIndex(x => x.OpHash);

            modelBuilder.Entity<TransactionOperation>()
                .HasIndex(x => x.SenderId);

            modelBuilder.Entity<TransactionOperation>()
                .HasIndex(x => x.TargetId);
            #endregion

            #region keys
            modelBuilder.Entity<TransactionOperation>()
                .HasKey(x => x.Id);
            #endregion
            
            #region props
            modelBuilder.Entity<TransactionOperation>()
                .Property(x => x.OpHash)
                .IsFixedLength(true)
                .HasMaxLength(51)
                .IsRequired();
            #endregion
            
            #region relations
            modelBuilder.Entity<TransactionOperation>()
                .HasOne(x => x.Block)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.Level)
                .HasPrincipalKey(x => x.Level);

            modelBuilder.Entity<TransactionOperation>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.InternalTransactions)
                .HasForeignKey(x => x.ParentId);

            modelBuilder.Entity<TransactionOperation>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.SentTransactions)
                .HasForeignKey(x => x.SenderId);

            modelBuilder.Entity<TransactionOperation>()
                .HasOne(x => x.Target)
                .WithMany(x => x.ReceivedTransactions)
                .HasForeignKey(x => x.TargetId);
            #endregion
        }
    }
}