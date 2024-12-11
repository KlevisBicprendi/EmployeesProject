using DAL_Punonjes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.DbConfig
{
    public class EmployerUserConfiguration : IEntityTypeConfiguration<EmployerToUser>
    {
        public void Configure(EntityTypeBuilder<EmployerToUser> builder)
        {
            builder.ToTable("EmployerToUser");
            builder.HasKey(x=> new {x.EmployerId, x.UserId});

        }
    }
}
