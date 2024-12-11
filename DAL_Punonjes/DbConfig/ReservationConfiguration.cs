using DAL_Punonjes.Entities;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.DbConfig
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");
            builder.HasOne<Employer>()
                .WithMany()
                .HasForeignKey(x=>x.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(c => c.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x=>x.Offer).HasPrecision(18,2);
        }
    }
}