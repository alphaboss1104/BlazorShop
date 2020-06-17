﻿namespace BlazorShop.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    using static ModelConstants.Region;

    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> region)
        {
            region
                .Property(r => r.Name)
                .HasMaxLength(MaxNameLength)
                .IsRequired();
        }
    }
}
