using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.AtrakModels;

public partial class AtrakContext : DbContext
{
    public AtrakContext(DbContextOptions<AtrakContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SmaxTransaction> SmaxTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SmaxTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SMaxTransaction");

            entity.HasIndex(e => new { e.TrChId, e.TrTime }, "IDX_SMAXTXN_STAFFID_TR_TIME");

            entity.HasIndex(e => new { e.TrDate, e.TrTime, e.TrChId, e.TrIpaddress, e.TrTtype }, "NonClusteredIndex-20160522-114948").IsUnique();

            entity.Property(e => e.DeName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
            entity.Property(e => e.DeReadertype)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DE_READERTYPE");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Isprocessed).HasColumnName("isprocessed");
            entity.Property(e => e.SmaxId).HasColumnName("SMAX_Id");
            entity.Property(e => e.TrCardNumber)
                .HasMaxLength(20)
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrChId)
                .HasMaxLength(20)
                .HasColumnName("Tr_ChId");
            entity.Property(e => e.TrCreated)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Created");
            entity.Property(e => e.TrDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrId).HasColumnName("TR_ID");
            entity.Property(e => e.TrIpaddress)
                .HasMaxLength(15)
                .HasColumnName("Tr_IPAddress");
            entity.Property(e => e.TrLnId).HasColumnName("Tr_LnId");
            entity.Property(e => e.TrMessage)
                .HasMaxLength(500)
                .HasColumnName("Tr_Message");
            entity.Property(e => e.TrNodeId).HasColumnName("Tr_NodeId");
            entity.Property(e => e.TrOpName)
                .HasMaxLength(50)
                .HasColumnName("Tr_OpName");
            entity.Property(e => e.TrReason).HasColumnName("Tr_Reason");
            entity.Property(e => e.TrSourceCreatedOn)
                .HasColumnType("datetime")
                .HasColumnName("Tr_SourceCreatedOn");
            entity.Property(e => e.TrTime)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Time");
            entity.Property(e => e.TrTrackCard).HasColumnName("Tr_TrackCard");
            entity.Property(e => e.TrTtype).HasColumnName("Tr_TType");
            entity.Property(e => e.TrUnit).HasColumnName("Tr_Unit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
