﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class ScheduleManagementContext : DbContext
    {
        public ScheduleManagementContext()
        {
        }

        public ScheduleManagementContext(DbContextOptions<ScheduleManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<GroupClass> GroupClasses { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<ScheduleDetail> ScheduleDetails { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<SlotTemplate> SlotTemplates { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=(local); database = ScheduleManagement; uid=sa;pwd=1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.Details)
                    .HasMaxLength(255)
                    .HasColumnName("details");
            });

            modelBuilder.Entity<GroupClass>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BuildingId).HasColumnName("buildingId");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("rooms_buildingid_foreign");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId).HasColumnName("classId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.SlotTemplateId).HasColumnName("slotTemplateId");

                entity.Property(e => e.SubjectId).HasColumnName("subjectId");

                entity.Property(e => e.TeacherId).HasColumnName("teacherId");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("schedule_classid_foreign");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("schedule_roomid_foreign");

                entity.HasOne(d => d.SlotTemplate)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SlotTemplateId)
                    .HasConstraintName("FK_Schedule_SlotTemplate");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("schedule_subjectid_foreign");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("schedule_teacherid_foreign");
            });

            modelBuilder.Entity<ScheduleDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.IsChanged).HasColumnName("isChanged");

                entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");

                entity.Property(e => e.SlotId).HasColumnName("slotId");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.ScheduleDetails)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__ScheduleD__sched__66603565");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.ScheduleDetails)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK__ScheduleD__slotI__6754599E");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Details)
                    .HasMaxLength(255)
                    .HasColumnName("details");

                entity.Property(e => e.SlotName)
                    .HasMaxLength(255)
                    .HasColumnName("slotName");
            });

            modelBuilder.Entity<SlotTemplate>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Detail)
                    .IsUnicode(false)
                    .HasColumnName("detail");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.Details)
                    .HasMaxLength(255)
                    .HasColumnName("details");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("fullName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
