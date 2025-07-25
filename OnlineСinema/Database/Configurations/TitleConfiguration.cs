﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineСinema.Database;
using OnlineСinema.Models;
using System;
using System.Collections.Generic;

namespace OnlineСinema.Database.Configurations
{
    public partial class TitleConfiguration : IEntityTypeConfiguration<Title>
    {
        public void Configure(EntityTypeBuilder<Title> entity)
        {
            entity.HasKey(e => e.Id).HasName("title_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Coverimage).WithMany(p => p.TitleCoverimages)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fktitle479355");

            entity.HasOne(d => d.Tileimage).WithMany(p => p.TitleTileimages)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fktitle660657");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Title> entity);
    }
}
